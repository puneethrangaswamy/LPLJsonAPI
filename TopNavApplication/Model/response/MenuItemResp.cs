namespace TopNavApplication.Model.response
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
        public MenuType menuType;
        public MenuAction menuAction;
        public MenuLoadConfig menuLoadConfig;
        public MenuLayout menuLayout;
        public EntitlementType menuEntitlementType;
        public List<MenuItemResp> childMenuItems;

        public MenuItemResp()
        {
            childMenuItems = new List<MenuItemResp>();
        }

        public static MenuItemResp CreateMenuItemFromDBResponse(MenuItem dbMenuItem, PreAuthMenuItem preAuthMItem)
        {
            MenuItemResp respMenuItem = new MenuItemResp();
            respMenuItem.menuItemID = dbMenuItem.menuItemID;
            respMenuItem.menuItemName = dbMenuItem.menuItemName != null ? dbMenuItem.menuItemName : "";
            respMenuItem.menuURL = dbMenuItem.menuURL != null ? dbMenuItem.menuURL : "";
            respMenuItem.menuElementClass = dbMenuItem.menuElementClass != null ? dbMenuItem.menuElementClass : "";
            respMenuItem.menuElementID = dbMenuItem.menuElementID;

            MenuType mType = new MenuType();
            mType.Name = dbMenuItem.menuTypeName != null ? dbMenuItem.menuTypeName : "";
            mType.Description = dbMenuItem.menuTypeDescription != null ? dbMenuItem.menuTypeDescription : "";
            respMenuItem.menuType = mType;

            MenuAction mAction = new MenuAction();
            mAction.Name = dbMenuItem.menuActionName != null ? dbMenuItem.menuActionName : "";
            mAction.Description = dbMenuItem.menuActionDescription != null ? dbMenuItem.menuActionDescription : "";
            respMenuItem.menuAction = mAction;

            MenuLoadConfig mLoadConfig = new MenuLoadConfig();
            mLoadConfig.Name = dbMenuItem.menuLoadConfigName != null ? dbMenuItem.menuLoadConfigName : "";
            mLoadConfig.Description = dbMenuItem.menuLoadConfigDescription != null ? dbMenuItem.menuLoadConfigDescription : "";
            respMenuItem.menuLoadConfig = mLoadConfig;

            MenuLayout mLayout = new MenuLayout();
            mLayout.Name = dbMenuItem.menuLayoutName != null ? dbMenuItem.menuLayoutName : "";
            mLayout.Description = dbMenuItem.menuLayoutDescription != null ? dbMenuItem.menuLayoutDescription : "";
            respMenuItem.menuLayout =mLayout;

            EntitlementType mEntitlementType = new EntitlementType();
            mEntitlementType.Name = preAuthMItem.EntitlementName != null? preAuthMItem.EntitlementName : "";
            mEntitlementType.Description = preAuthMItem.EntitlementDescription != null ? preAuthMItem.EntitlementDescription : "";
            respMenuItem.menuEntitlementType = mEntitlementType;

            return respMenuItem;
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
            mType.Name = dbMenuItem.menuTypeName != null ? dbMenuItem.menuTypeName : "";
            mType.Description = dbMenuItem.menuTypeDescription != null ? dbMenuItem.menuTypeDescription : "";
            respMenuItem.menuType = mType;

            MenuAction mAction = new MenuAction();
            mAction.Name = dbMenuItem.menuActionName != null ? dbMenuItem.menuActionName : "";
            mAction.Description = dbMenuItem.menuActionDescription != null ? dbMenuItem.menuActionDescription : "";
            respMenuItem.menuAction = mAction;

            MenuLoadConfig mLoadConfig = new MenuLoadConfig();
            mLoadConfig.Name = dbMenuItem.menuLoadConfigName != null ? dbMenuItem.menuLoadConfigName : "";
            mLoadConfig.Description = dbMenuItem.menuLoadConfigDescription != null ? dbMenuItem.menuLoadConfigDescription : "";
            respMenuItem.menuLoadConfig = mLoadConfig;

            MenuLayout mLayout = new MenuLayout();
            mLayout.Name = dbMenuItem.menuLayoutName != null ? dbMenuItem.menuLayoutName : "";
            mLayout.Description = dbMenuItem.menuLayoutDescription != null ? dbMenuItem.menuLayoutDescription : "";
            respMenuItem.menuLayout = mLayout;

            EntitlementType mEntitlementType = new EntitlementType();
            mEntitlementType.Name = postAuthMItem.entitlementName != null ? postAuthMItem.entitlementName : "";
            mEntitlementType.Description = postAuthMItem.entitlementDescription != null ? postAuthMItem.entitlementDescription : "";
            respMenuItem.menuEntitlementType = mEntitlementType;

            return respMenuItem;
        }
    }
}
