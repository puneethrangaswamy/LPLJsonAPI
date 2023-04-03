using Microsoft.Extensions.Options;
using Npgsql;
using TopNavApplication.Models;

namespace TopNavApplication.Repositories
{
    public sealed class DefaultMenuRepository : RepositoryBase, IMenuRepository
    {
        public DefaultMenuRepository(IOptions<ConnectionStringsOptions> options) 
            : base(options)
        {

        }

        public async Task<MenuItemCollection> GetMenuItems()
        {
            string sqlQuery = $@"select 
            	mi.ID as menu_item_id, mi.NAME as menu_item_name, mi.url, mi.ELEMENT_CLASS, mi.ELEMENT_ID, mi.IS_DELETED, 
                mt.id as menu_type_id, mt.name as menu_type_name, mt.description as menu_type_description, 
                ma.id as menu_action_id, ma.name as menu_action_name, ma.description as menu_action_description, 
                mlc.id as menu_load_config_id, mlc.name as menu_load_config_name, mlc.description as menu_load_config_description, 
                ml.id as menu_layout_id, ml.name as menu_layout_name, ml.description as menu_layout_description 
                from menu_item mi 
                left join menu_type mt on mi.type_id = mt.id 
                left join menu_action ma on mi.action_id = ma.id 
                left join menu_load_config mlc on mi.LOAD_CONFIG_ID = mlc.id 
                left join menu_layout ml on mi.LAYOUT_ID = ml.id 
                where mi.is_deleted = 0";

            MenuItemCollection menuItems;

            await using (var dbConnection = await OpenDbConnection())
            {
                await using (var dataReader = await ExecuteQueryAsync(sqlQuery, dbConnection))
                {
                    menuItems = await CreateMenuItemFromResultSet(dataReader);
                }
            }

            return menuItems;
        }

        public async Task<IEnumerable<PostAuthMenuItem>> GetMenuItems(string groupName, int appId)
        {
            string sqlQuery = $@"select 
                amm1.PARENT_MENU_ITEM_ID as post_auth_parent_menu_item_id, amm1.CHILD_MENU_ITEM_ID as post_auth_child_menu_item_id, amm1.PARENT_VIEW_ORDER, amm1.CHILD_VIEW_ORDER, amm.start_date, amm.end_date, 
                eg.id as entitlement_group_id, eg.name as entitlement_group_name, eg.description as Entitlement_Group_Description, 
                et.id as entitlement_type_id, et.name as entitlement_name, et. description as Entitlement_Description 
                from auth_menu_mapping amm 
                join application_menu_mapping amm1 on amm1.ID = amm.APPLICATION_MENU_ITEM_ID 
                join entitlement_group eg on eg.id = amm.group_id and LOWER(eg.NAME) = '{groupName.ToLower()}' 
                left join entitlement_type et on et.id = amm.entitlement_type_id 
                where amm1.APPLICATION_ID = {appId} 
                order by amm1.PARENT_MENU_ITEM_ID";

            IEnumerable<PostAuthMenuItem> menuItems;

            await using (var dbConnection = await OpenDbConnection())
            {
                await using (var dataReader = await ExecuteQueryAsync(sqlQuery, dbConnection))
                {
                    menuItems = await CreatePostAuthMenuItemFromResultSet(dataReader);
                }
            }

            return menuItems;
        }

        private async Task<IEnumerable<PostAuthMenuItem>> CreatePostAuthMenuItemFromResultSet(NpgsqlDataReader dataReader)
        {
            var menuItems = new List<PostAuthMenuItem>();

            while (await dataReader.ReadAsync())
            {
                var menuItem = new PostAuthMenuItem
                {
                    parentMenuItemId = GetValue(dataReader, 0, -1),
                    childMenuItemId = GetValue(dataReader, 1, 0),
                    prtViewOrder = GetValue(dataReader, 2, 0),
                    chldViewOrder = GetValue(dataReader, 3, 0),
                    startDate = GetValue(dataReader, 4, default(DateTime)),
                    endDate = GetValue(dataReader, 5, default(DateTime)),
                    entitlementTypeID = GetValue(dataReader, 6, 0),
                    entitlementName = GetValue(dataReader, 7, string.Empty),
                    entitlementDescription = GetValue(dataReader, 8, string.Empty),
                    entitlementGroupID = GetValue(dataReader, 9, 0),
                    entitlementGroup = GetValue(dataReader, 10, string.Empty),
                    entitlementGroupDescription = GetValue(dataReader, 11, string.Empty)
                };

                menuItems.Add(menuItem);
            }

            return menuItems;
        }

        private async Task<MenuItemCollection> CreateMenuItemFromResultSet(NpgsqlDataReader dataReader)
        {
            var menuItems = new MenuItemCollection();

            while (await dataReader.ReadAsync())
            {
                MenuItem menuItem = new MenuItem
                {
                    menuItemID = GetValue(dataReader, 0, -1),
                    menuItemName = GetValue(dataReader, 1, string.Empty),
                    menuURL = GetValue(dataReader, 2, string.Empty),
                    menuElementClass = GetValue(dataReader, 3, string.Empty),
                    menuElementID = GetValue(dataReader, 4, string.Empty),
                    menuTypeID = GetValue(dataReader, 6, -1),
                    menuTypeName = GetValue(dataReader, 7, string.Empty),
                    menuTypeDescription = GetValue(dataReader, 8, string.Empty),
                    menuActionID = GetValue(dataReader, 9, -1),
                    menuActionName = GetValue(dataReader, 10, string.Empty),
                    menuActionDescription = GetValue(dataReader, 11, string.Empty),
                    menuLoadConfigID = GetValue(dataReader, 12, -1),
                    menuLoadConfigName = GetValue(dataReader, 13, string.Empty),
                    menuLoadConfigDescription = GetValue(dataReader, 14, string.Empty),
                    menuLayoutID = GetValue(dataReader, 15, -1),
                    menuLayoutName = GetValue(dataReader, 16, string.Empty),
                    menuLayoutDescription = GetValue(dataReader, 17, string.Empty),
                };

                menuItems.Add(menuItem);
            }

            return menuItems;
        }
    }
}
