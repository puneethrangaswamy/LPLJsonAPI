using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TopNavApplication.Helper;
using TopNavApplication.Model;
using TopNavApplication.Model.response;
using TopNavApplication.Util;

namespace TopNavApplication.ApiControllers
{
    [ApiController]
    [Route("[controller]")]
    public class LPLMenuAPIController : ControllerBase
    {

        [HttpPost("auth")]

        public Task<IActionResult> Authenticate([FromBody]Login login)
        {
            Console.WriteLine("Username => " + login.Username);
            Console.WriteLine("Password => " + login.Password);

            if (string.IsNullOrEmpty(login.Username) || 
                string.IsNullOrEmpty(login.Password) ||
                !login.Password.Equals("password"))
            {
               return Task.FromResult((IActionResult)BadRequest("Please provide valid credentials"));      
            }

            string token = TokenUtil.CreateToken(login);

            string role = LPLMenuDataContext.GetRoleByUserName(login.Username, login.Password);

            HttpContext.Response.Headers.Add("role", role);
            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers","*");
            HttpContext.Response.Headers.Add("x-auth-token", token);
            return Task.FromResult((IActionResult)Ok());
        }


        [HttpGet]
        public Task<IActionResult> ValidateAuthenticationToken(string authToken)
        {
            if (String.IsNullOrEmpty(authToken)){
                return Task.FromResult((IActionResult)BadRequest("Please provide valid token"));
            }

            if (!TokenUtil.ValidateToken(authToken)){
                return Task.FromResult((IActionResult)BadRequest("Auth Token Expires"));
            }

            string userName = TokenUtil.GetUserNameFromToken(authToken);

            HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*"); 
            HttpContext.Response.Headers.Add("userName", userName);
            HttpContext.Response.Headers.Add("x-auth-token", authToken);

            return Task.FromResult((IActionResult)Ok());
        }


        [HttpGet("postAuth/")]
        public async Task<ActionResult> GetPostAuth([Required] String appName, [Required] String groupName)
        {
            PostAuthMenu postAuthMenu = new PostAuthMenu();
            Dictionary<Int32, MenuItemResp> menuMap = new Dictionary<int, MenuItemResp>();
            try
            {

                if (groupName == null || groupName.IsEmpty())
                    return BadRequest("Please provide groupname");

                
                if (appName == null || appName.IsEmpty())
                    return BadRequest("Please provide appname");

                // Get Data From Database
                Application application = LPLMenuDataContext.GetApplication(appName);
                if (null == application)
                    return BadRequest("Looks like application is configured by this name");

                // Get Data From Database
                Dictionary<Int32, MenuItem> menuItemsMap = LPLMenuDataContext.GetMenuItems();
                List<PostAuthMenuItem> postAuthMenuItemList = LPLMenuDataContext.GetPostAuthMenuItems(groupName, application.Id);


                List<PostAuthMenuItem> tPostAuthMenuItemList = new List<PostAuthMenuItem>();
                tPostAuthMenuItemList.AddRange(postAuthMenuItemList);

                MenuItemResp tmpMenuItem = new MenuItemResp();
                MenuItemResp tmpCMenuItem = new MenuItemResp();
                MenuItem dbPMenuItem = new MenuItem();
                MenuItem dbCMenuItem = new MenuItem();

                // Iterate Menu Items and Get the Parents
                for (int i = 0; i < postAuthMenuItemList.Count(); i++)
                {

                    PostAuthMenuItem postAuthMenuItem = postAuthMenuItemList[i];
                    int parentMenuItemID = postAuthMenuItem.parentMenuItemId;

                    if (!menuMap.ContainsKey(parentMenuItemID))
                    {
                        // Check if it is already under any Parent
                        List<PostAuthMenuItem> pHierarchy = ApplicationUtil.GetParentHierarchyPostAuth(postAuthMenuItem, tPostAuthMenuItemList);

                        // Get Parent 
                        dbPMenuItem = menuItemsMap[parentMenuItemID];
                        tmpMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbPMenuItem, postAuthMenuItem);

                        if (pHierarchy.IsEmpty())
                            menuMap.Add(parentMenuItemID, tmpMenuItem);
                    }
                }

                int counter = 0;
                // Iterate Menu Items and get all the children
                foreach (PostAuthMenuItem postAuthMenuItem in postAuthMenuItemList)
                {
                    int parentMenuItemID = postAuthMenuItem.parentMenuItemId;

                    // Check if it's exist in Pre Auth Menu Map
                    if (menuMap.ContainsKey(parentMenuItemID))
                    {
                        tmpMenuItem = menuMap[parentMenuItemID];

                        // Get Child
                        if(menuItemsMap.ContainsKey(postAuthMenuItem.childMenuItemId))
                        {
                            dbCMenuItem = menuItemsMap[postAuthMenuItem.childMenuItemId];
                            if (dbCMenuItem != null)
                            {
                                tmpCMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbCMenuItem, postAuthMenuItem);

                                // Add child if not exist
                                MenuItemResp CMenuItem = ApplicationUtil.IsChildMenuItemParent(tmpCMenuItem, tmpMenuItem.childMenuItems);
                                if (null == CMenuItem)
                                    tmpMenuItem.childMenuItems.Add(tmpCMenuItem);
                            }
                        }

                        menuMap.Remove(parentMenuItemID);
                        menuMap.Add(parentMenuItemID, tmpMenuItem);
                    }
                    else
                    {

                        MenuItemResp rootMenuItem = new MenuItemResp();
                        MenuItemResp cldRootMenuItem = new MenuItemResp();
                        MenuItemResp cldMenuItem = new MenuItemResp();

                        // Check if it is already under any Parent
                        List<PostAuthMenuItem> pHierarchy = ApplicationUtil.GetParentHierarchyPostAuth(postAuthMenuItem, tPostAuthMenuItemList);
                        bool isFirst = true;
                        if (!pHierarchy.IsEmpty())
                        {
                            foreach (PostAuthMenuItem postAuthMenuItemHie in pHierarchy)
                            {
                                if (isFirst)
                                {
                                    rootMenuItem = menuMap[postAuthMenuItemHie.parentMenuItemId];
                                    if (null == rootMenuItem)
                                        break;
                                    isFirst = false;
                                }
                                else
                                {
                                    // Get Parent 
                                    dbPMenuItem = menuItemsMap[postAuthMenuItemHie.parentMenuItemId];
                                    MenuItemResp CMenuItem = null;
                                    if (dbCMenuItem != null)
                                    {
                                        cldRootMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbPMenuItem, postAuthMenuItemHie);

                                        // Get Child
                                        dbCMenuItem = menuItemsMap[postAuthMenuItemHie.childMenuItemId];
                                        cldMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbCMenuItem, postAuthMenuItemHie);

                                        // Add child if not exist
                                        CMenuItem = ApplicationUtil.IsChildMenuItemParent(cldMenuItem, cldRootMenuItem.childMenuItems);
                                        if (null == CMenuItem)
                                            tmpMenuItem.childMenuItems.Add(cldMenuItem);
                                    }

                                    // Add child parent to root if not exist
                                    CMenuItem = ApplicationUtil.IsChildMenuItemParent(cldRootMenuItem, rootMenuItem.childMenuItems);
                                    if (null == CMenuItem)
                                    {
                                        rootMenuItem.childMenuItems.Add(cldRootMenuItem);
                                        rootMenuItem = cldRootMenuItem;
                                    }
                                }
                            }
                        }
                    }
                }


                List<MenuItemResp> mItemList = new List<MenuItemResp>();
                foreach (var entry in menuMap)
                {
                    mItemList.Add(entry.Value);
                }
                postAuthMenu.ParentMenuItems = mItemList;

                postAuthMenu.Application = application;

                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "*");

                String json = JsonConvert.SerializeObject(postAuthMenu);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    
}
