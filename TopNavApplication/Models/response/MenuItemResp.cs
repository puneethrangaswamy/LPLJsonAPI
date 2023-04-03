namespace TopNavApplication.Models.response
{
    public class MenuItemResp
    {
        public int menuItemID;
        public string? menuItemName;
        public string? menuURL;
        public int prtViewOrder;
        public int chldViewOrder;
        public string? menuElementClass;
        public string? menuElementID;
        public MenuType? menuType;
        public MenuAction? menuAction;
        public MenuLoadConfig? menuLoadConfig;
        public MenuLayout? menuLayout;
        public EntitlementType? menuEntitlementType;
        public List<MenuItemResp> childMenuItems;

        public MenuItemResp()
        {
            childMenuItems = new List<MenuItemResp>();
        }


        public static MenuItemResp CreateMenuItemFromDBResponsePostAuth(MenuItem dbMenuItem, PostAuthMenuItem postAuthMItem)
        {
            MenuItemResp respMenuItem = new MenuItemResp();
            respMenuItem.menuItemID = dbMenuItem.menuItemID;
            respMenuItem.menuItemName = dbMenuItem.menuItemName != null? dbMenuItem.menuItemName : "";
            respMenuItem.menuURL = dbMenuItem.menuURL != null? dbMenuItem.menuURL : "";
            respMenuItem.menuElementClass = dbMenuItem.menuElementClass != null ? dbMenuItem.menuElementClass : "";
            respMenuItem.menuElementID = dbMenuItem.menuElementID != null ? dbMenuItem.menuElementID : "";
            respMenuItem.prtViewOrder = postAuthMItem.prtViewOrder;
            respMenuItem.chldViewOrder = postAuthMItem.chldViewOrder;

            MenuType mType = new MenuType();
            mType.name = dbMenuItem.menuTypeName != null ? dbMenuItem.menuTypeName : "";
            mType.description = dbMenuItem.menuTypeDescription != null ? dbMenuItem.menuTypeDescription : "";
            respMenuItem.menuType = mType;

            MenuAction mAction = new MenuAction();
            mAction.name = dbMenuItem.menuActionName != null ? dbMenuItem.menuActionName : "";
            mAction.description = dbMenuItem.menuActionDescription != null ? dbMenuItem.menuActionDescription : "";
            respMenuItem.menuAction = mAction;

            MenuLoadConfig mLoadConfig = new MenuLoadConfig();
            mLoadConfig.name = dbMenuItem.menuLoadConfigName != null ? dbMenuItem.menuLoadConfigName : "";
            mLoadConfig.description = dbMenuItem.menuLoadConfigDescription != null ? dbMenuItem.menuLoadConfigDescription : "";
            respMenuItem.menuLoadConfig = mLoadConfig;

            MenuLayout mLayout = new MenuLayout();
            mLayout.name = dbMenuItem.menuLayoutName != null ? dbMenuItem.menuLayoutName : "";
            mLayout.description = dbMenuItem.menuLayoutDescription != null ? dbMenuItem.menuLayoutDescription : "";
            respMenuItem.menuLayout = mLayout;

            EntitlementType mEntitlementType = new EntitlementType();
            mEntitlementType.name = postAuthMItem.entitlementName != null ? postAuthMItem.entitlementName : "";
            mEntitlementType.description = postAuthMItem.entitlementDescription != null ? postAuthMItem.entitlementDescription : "";
            respMenuItem.menuEntitlementType = mEntitlementType;

            return respMenuItem;
        }
    }
}
