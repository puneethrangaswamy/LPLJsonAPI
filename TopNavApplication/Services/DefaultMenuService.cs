using FluentNHibernate.Conventions;
using TopNavApplication.Repositories;
using TopNavApplication.Models;
using TopNavApplication.Models.response;

namespace TopNavApplication.Services
{
    public class DefaultMenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        public DefaultMenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<PostAuthMenu> GetMenuItems(string groupName, string appName)
        {
            var application = await LPLMenuDataContext.GetApplication(appName);

            if (application == null)
            {
                throw new Exception("App noes not exist");
            }

            var menuMap = new Dictionary<int, MenuItemResp>();

            var menuItemsCollection = await _menuRepository.GetMenuItems();
            var postAuthMenuItemList = await _menuRepository.GetMenuItems(groupName, application.Id);

            var tempMenuItem = new MenuItemResp();
            MenuItemResp tempChildMenuItem;
            MenuItem dbChildMenuItem = new MenuItem();

            // Iterate Menu Items and Get the Parents
            foreach (var postAuthMenuItem in postAuthMenuItemList)
            {
                int parentMenuItemID = postAuthMenuItem.parentMenuItemId;

                if (!menuMap.ContainsKey(parentMenuItemID))
                {
                    // Check if it is already under any Parent
                    var parentHierarchy = GetParentHierarchyPostAuth(postAuthMenuItem, postAuthMenuItemList);

                    if (parentHierarchy.IsEmpty())
                    {
                        var dbParentMenuItem = menuItemsCollection[parentMenuItemID];
                        tempMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbParentMenuItem, postAuthMenuItem);

                        menuMap.Add(parentMenuItemID, tempMenuItem);
                    }
                }
            }

            // Iterate Menu Items and get all the children
            foreach (var postAuthMenuItem in postAuthMenuItemList)
            {
                int parentMenuItemID = postAuthMenuItem.parentMenuItemId;

                // Check if it's exist in Pre Auth Menu Map
                if (menuMap.ContainsKey(parentMenuItemID))
                {
                    tempMenuItem = menuMap[parentMenuItemID];

                    // Get Child
                    if (menuItemsCollection.Contains(postAuthMenuItem.childMenuItemId))
                    {
                        dbChildMenuItem = menuItemsCollection[postAuthMenuItem.childMenuItemId];
                        if (dbChildMenuItem != null)
                        {
                            tempChildMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbChildMenuItem, postAuthMenuItem);

                            // Add child if not exist
                            var childMenuItem = IsChildMenuItemParent(tempChildMenuItem, tempMenuItem.childMenuItems);

                            if (childMenuItem == null)
                            {
                                tempMenuItem.childMenuItems.Add(tempChildMenuItem);
                            }
                        }
                    }

                    menuMap.Remove(parentMenuItemID);
                    menuMap.Add(parentMenuItemID, tempMenuItem);
                }
                else
                {

                    MenuItemResp rootMenuItem = new MenuItemResp();
                    MenuItemResp cldRootMenuItem = new MenuItemResp();
                    MenuItemResp cldMenuItem = new MenuItemResp();

                    // Check if it is already under any Parent
                    var parentHierarchy = GetParentHierarchyPostAuth(postAuthMenuItem, postAuthMenuItemList);
                    bool isFirst = true;

                    if (!parentHierarchy.IsEmpty())
                    {
                        foreach (var postAuthMenuItemHie in parentHierarchy)
                        {
                            if (isFirst)
                            {
                                rootMenuItem = menuMap[postAuthMenuItemHie.parentMenuItemId];
                                if (rootMenuItem == null)
                                {
                                    break;
                                }
                                isFirst = false;
                            }
                            else
                            {
                                // Get Parent 
                                var dbParentMenuItem = menuItemsCollection[postAuthMenuItemHie.parentMenuItemId];
                                MenuItemResp? cMenuItem = null;

                                if (dbChildMenuItem != null)
                                {
                                    cldRootMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbParentMenuItem, postAuthMenuItemHie);

                                    // Get Child
                                    dbChildMenuItem = menuItemsCollection[postAuthMenuItemHie.childMenuItemId];
                                    cldMenuItem = MenuItemResp.CreateMenuItemFromDBResponsePostAuth(dbChildMenuItem, postAuthMenuItemHie);

                                    // Add child if not exist
                                    cMenuItem = IsChildMenuItemParent(cldMenuItem, cldRootMenuItem.childMenuItems);
                                    if (cMenuItem == null)
                                    {
                                        tempMenuItem.childMenuItems.Add(cldMenuItem);
                                    }
                                }

                                // Add child parent to root if not exist
                                cMenuItem = IsChildMenuItemParent(cldRootMenuItem, rootMenuItem.childMenuItems);
                                if (cMenuItem == null)
                                {
                                    rootMenuItem.childMenuItems.Add(cldRootMenuItem);
                                    rootMenuItem = cldRootMenuItem;
                                }
                            }
                        }
                    }
                }
            }

            var postAuthMenu = new PostAuthMenu()
            {
                parentMenuItems = menuMap.Values.ToList(),
                application = application
            };

            return postAuthMenu;
        }

        private List<PostAuthMenuItem> GetParentHierarchyPostAuth(PostAuthMenuItem postAuthMItem, IEnumerable<PostAuthMenuItem> postAuthMenuItemList)
        {
            List<PostAuthMenuItem> pHierarchy = new List<PostAuthMenuItem>();
            List<PostAuthMenuItem> pRHierarchy = new List<PostAuthMenuItem>();
            bool val = IsMenuItemParent(postAuthMItem, postAuthMenuItemList);
            while (!val)
            {
                bool isFound = false;
                if (postAuthMenuItemList != null)
                {
                    foreach (PostAuthMenuItem postAuthMenuItem in postAuthMenuItemList)
                    {
                        if (postAuthMenuItem.childMenuItemId == postAuthMItem.parentMenuItemId)
                        {
                            isFound = true;
                            pHierarchy.Add(postAuthMenuItem);
                            val = IsMenuItemParent(postAuthMenuItem, postAuthMenuItemList);
                            postAuthMItem = postAuthMenuItem;
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        break;
                    }
                }
            }

            for (int i = pHierarchy.Count() - 1; i >= 0; i--)
            {
                pRHierarchy.Add(pHierarchy[i]);
            }

            return pRHierarchy;
        }

        public bool IsMenuItemParent(PostAuthMenuItem postAuthMItem, IEnumerable<PostAuthMenuItem> postAuthMenuItemList)
        {
            bool exists = postAuthMenuItemList.Where(item => item.childMenuItemId == item.parentMenuItemId).Any();

            return exists;
        }

        public MenuItemResp? IsChildMenuItemParent(MenuItemResp childMenu, IEnumerable<MenuItemResp> childMenuItems)
        {
            if (childMenuItems != null)
            {
                foreach (MenuItemResp cMenu in childMenuItems)
                {
                    if (cMenu.menuItemID == childMenu.menuItemID)
                    {
                        return cMenu;
                    }
                }
            }

            return null;
        }

    }
}
