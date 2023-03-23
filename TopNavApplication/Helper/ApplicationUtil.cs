using System;
using TopNavApplication.Model;
using TopNavApplication.Model.response;

namespace TopNavApplication.Helper
{
    public class ApplicationUtil
    {
        public static List<PreAuthMenuItem> GetParentHierarchy(PreAuthMenuItem preAuthMItem, List<PreAuthMenuItem> preAuthMenuItemList)
        {
            List<PreAuthMenuItem> pHierarchy = new List<PreAuthMenuItem>();
            List<PreAuthMenuItem> pRHierarchy = new List<PreAuthMenuItem>();
            bool val = IsMenuItemParent(preAuthMItem, preAuthMenuItemList);
            while (!val)
            {
                bool isFound = false;
                if(preAuthMenuItemList != null)
                {
                    foreach (PreAuthMenuItem preAuthMenuItem in preAuthMenuItemList)
                    {
                        if (preAuthMenuItem.childMenuItemId == preAuthMItem.parentMenuItemId)
                        {
                            isFound = true;
                            pHierarchy.Add(preAuthMenuItem);
                            val = IsMenuItemParent(preAuthMenuItem, preAuthMenuItemList);
                            preAuthMItem = preAuthMenuItem;
                            break;
                        }
                    }
                }
                if (!isFound)
                    break;
            }

            for (int i = pHierarchy.Count() - 1; i >= 0; i--)
            {
                pRHierarchy.Add(pHierarchy[i]);
            }
            return pRHierarchy;
        }

        public static bool IsMenuItemParent(PreAuthMenuItem preAuthMItem, List<PreAuthMenuItem> preAuthMenuItemList)
        {
            if(preAuthMenuItemList != null)
            {
                foreach (PreAuthMenuItem preAuthMenuItem in preAuthMenuItemList)
                {
                    if (preAuthMenuItem.childMenuItemId == preAuthMenuItem.parentMenuItemId)
                        return true;
                }
            }
            return false;
        }

        public static List<PostAuthMenuItem> GetParentHierarchyPostAuth(PostAuthMenuItem postAuthMItem, List<PostAuthMenuItem> postAuthMenuItemList)
        {
            List<PostAuthMenuItem> pHierarchy = new List<PostAuthMenuItem>();
            List<PostAuthMenuItem> pRHierarchy = new List<PostAuthMenuItem>();
            bool val = IsMenuItemParentPostAuth(postAuthMItem, postAuthMenuItemList);
            while (!val)
            {
                bool isFound = false;
                if(postAuthMenuItemList != null)
                {
                    foreach (PostAuthMenuItem postAuthMenuItem in postAuthMenuItemList)
                    {
                        if (postAuthMenuItem.childMenuItemId == postAuthMItem.parentMenuItemId)
                        {
                            isFound = true;
                            pHierarchy.Add(postAuthMenuItem);
                            val = IsMenuItemParentPostAuth(postAuthMenuItem, postAuthMenuItemList);
                            postAuthMItem = postAuthMenuItem;
                            break;
                        }
                    }
                    if (!isFound)
                        break;
                }
            }

            for (int i = pHierarchy.Count() - 1; i >= 0; i--)
            {
                pRHierarchy.Add(pHierarchy[i]);
            }

            return pRHierarchy;
        }

        public static bool IsMenuItemParentPostAuth(PostAuthMenuItem postAuthMItem, List<PostAuthMenuItem> postAuthMenuItemList)
        {
            foreach (PostAuthMenuItem postAuthMenuItem in postAuthMenuItemList)
            {
                if (postAuthMenuItem.childMenuItemId == postAuthMenuItem.parentMenuItemId)
                    return true;
            }
            return false;
        }

        public static MenuItemResp IsChildMenuItemParent(MenuItemResp childMenu, List<MenuItemResp> childMenuItems)
        {
            if(childMenuItems != null)
            {
                foreach (MenuItemResp cMenu in childMenuItems)
                {
                    if (cMenu.menuItemID == childMenu.menuItemID)
                        return cMenu;
                }
            }
            return null;
        }

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }
    }
}
