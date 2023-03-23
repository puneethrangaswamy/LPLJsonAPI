using AutoMapper;
using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TopNavApplication.Helper;
using TopNavApplication.Model;
using TopNavApplication.Model.response;

namespace TopNavApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LPLMenuAPIController : ControllerBase
    {

        [HttpPost]
        public String Auth(Login login)
        {
            Console.WriteLine("Username => " + login.username);
            Console.WriteLine("Password => " + login.password);

            if (String.IsNullOrEmpty(login.username) || String.IsNullOrEmpty(login.password))
            {

            }

            return "Login Successfull!!";
        }

        [HttpGet ("preAuth/")]
        public async Task<ActionResult> GetPreAuth([Required] String appName)
        {
            PreAuthMenu preAuthMenuResponse = new PreAuthMenu();
            Dictionary<Int32, MenuItemResp> menuMap = new Dictionary<Int32, MenuItemResp>();

            try
            {   
                if(appName == null || appName.IsEmpty())
                    return BadRequest("Please provide appname");

                // Get Data From Database
                Application application = LPLMenuDataContext.getApplication(appName);
                if (null == application)
                    return BadRequest("Looks like application is configured by this name");

                Dictionary<Int32, MenuItem> menuItemsMap = LPLMenuDataContext.getMenuItems();
                List<PreAuthMenuItem> preAuthMenuItemList = LPLMenuDataContext.getPreAuthMenuItems(application.Id);


                List<PreAuthMenuItem> tPreAuthMenuItemList = new List<PreAuthMenuItem>();
                tPreAuthMenuItemList.AddRange(preAuthMenuItemList);

                MenuItemResp tmpMenuItem = new MenuItemResp();
                MenuItemResp tmpCMenuItem = new MenuItemResp();
                MenuItem dbPMenuItem = new MenuItem();
                MenuItem dbCMenuItem = new MenuItem();

                for (int i = 0; i < preAuthMenuItemList.Count(); i++)
                {

                    PreAuthMenuItem preAuthMenuItem = preAuthMenuItemList[i];
                    int parentMenuItemID = preAuthMenuItem.parentMenuItemId;

                    if (!menuMap.ContainsKey(parentMenuItemID))
                    {
                        // Check if it is already under any Parent
                        List<PreAuthMenuItem> pHierarchy = ApplicationUtil.GetParentHierarchy(preAuthMenuItem, tPreAuthMenuItemList);

                        // Get Parent 
                        dbPMenuItem = menuItemsMap[parentMenuItemID];
                        tmpMenuItem = MenuItemResp.CreateMenuItemFromDBResponse(dbPMenuItem, preAuthMenuItem);

                        if (pHierarchy.IsEmpty())
                            menuMap.Add(parentMenuItemID, tmpMenuItem);
                    }
                }

                int counter = 0;
                // Iterate Menu Items and get all the children
                foreach (PreAuthMenuItem preAuthMenuItem in preAuthMenuItemList)
                {
                    int parentMenuItemID = preAuthMenuItem.parentMenuItemId;

                    // Check if it's exist in Pre Auth Menu Map
                    if (menuMap.ContainsKey(parentMenuItemID))
                    {
                        tmpMenuItem = menuMap[parentMenuItemID];

                        // Get Child
                        dbCMenuItem = menuItemsMap[preAuthMenuItem.childMenuItemId];
                        tmpCMenuItem = MenuItemResp.CreateMenuItemFromDBResponse(dbCMenuItem, preAuthMenuItem);

                        // Add child if not exist
                        MenuItemResp CMenuItem = ApplicationUtil.IsChildMenuItemParent(tmpCMenuItem, tmpMenuItem.childMenuItems);
                        if (null == CMenuItem)
                            tmpMenuItem.childMenuItems.Add(tmpCMenuItem);

                        menuMap.Remove(parentMenuItemID);
                        menuMap.Add(parentMenuItemID, tmpMenuItem);
                    }
                    else
                    {

                        MenuItemResp rootMenuItem = new MenuItemResp();
                        MenuItemResp cldRootMenuItem = new MenuItemResp();
                        MenuItemResp cldMenuItem = new MenuItemResp();

                        // Check if it is already under any Parent
                        List<PreAuthMenuItem> pHierarchy = ApplicationUtil.GetParentHierarchy(preAuthMenuItem, tPreAuthMenuItemList);
                        bool isFirst = true;
                        if (!pHierarchy.IsEmpty())
                        {
                            foreach (PreAuthMenuItem preAuthMenuItemHie in pHierarchy)
                            {
                                if (isFirst)
                                {
                                    rootMenuItem = menuMap[preAuthMenuItemHie.parentMenuItemId];
                                    if (null == rootMenuItem)
                                        break;
                                    isFirst = false;
                                }
                                else
                                {
                                    // Get Parent 
                                    dbPMenuItem = menuItemsMap[preAuthMenuItemHie.parentMenuItemId];
                                    cldRootMenuItem = MenuItemResp.CreateMenuItemFromDBResponse(dbPMenuItem, preAuthMenuItemHie);

                                    // Get Child
                                    dbCMenuItem = menuItemsMap[preAuthMenuItemHie.childMenuItemId];
                                    cldMenuItem = MenuItemResp.CreateMenuItemFromDBResponse(dbCMenuItem, preAuthMenuItemHie);

                                    // Add child if not exist
                                    MenuItemResp CMenuItem = ApplicationUtil.IsChildMenuItemParent(cldMenuItem, cldRootMenuItem.childMenuItems);
                                    if (null == CMenuItem)
                                    {
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
                    mItemList.Add(entry.Value);

                preAuthMenuResponse.parentMenuItems = mItemList;


                preAuthMenuResponse.application = application;
                String json = JsonConvert.SerializeObject(preAuthMenuResponse);
                return Ok(json);

            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
                Application application = LPLMenuDataContext.getApplication(appName);
                if (null == application)
                    return BadRequest("Looks like application is configured by this name");

                // Get Data From Database
                Dictionary<Int32, MenuItem> menuItemsMap = LPLMenuDataContext.getMenuItems();
                List<PostAuthMenuItem> postAuthMenuItemList = LPLMenuDataContext.getPostAuthMenuItems(groupName, application.Id);


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
                        dbCMenuItem = menuItemsMap[postAuthMenuItem.childMenuItemId];
                        tmpCMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbCMenuItem, postAuthMenuItem);

                        // Add child if not exist
                        MenuItemResp CMenuItem = ApplicationUtil.IsChildMenuItemParent(tmpCMenuItem, tmpMenuItem.childMenuItems);
                        if (null == CMenuItem)
                        {
                            tmpMenuItem.childMenuItems.Add(tmpCMenuItem);
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
                                    cldRootMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbPMenuItem, postAuthMenuItemHie);

                                    // Get Child
                                    dbCMenuItem = menuItemsMap[postAuthMenuItemHie.childMenuItemId];
                                    cldMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbCMenuItem, postAuthMenuItemHie);

                                    // Add child if not exist
                                    MenuItemResp CMenuItem = ApplicationUtil.IsChildMenuItemParent(cldMenuItem, cldRootMenuItem.childMenuItems);
                                    if (null == CMenuItem)
                                    {
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
                postAuthMenu.parentMenuItems = mItemList;

                postAuthMenu.application = application;
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
