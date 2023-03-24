using Microsoft.AspNetCore.Http;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using TopNavApplication.Helper;

namespace TopNavApplication.Model
{
    public class MenuItem
    {
        public int menuItemID;
        public String menuItemName;
        public String menuURL;
        public String menuElementClass;
        public String menuElementID;
        public int menuTypeID;
        public String menuTypeName;
        public String menuTypeDescription;
        public int menuActionID;
        public String menuActionName;
        public String menuActionDescription;
        public int menuLoadConfigID;
        public String menuLoadConfigName;
        public String menuLoadConfigDescription;
        public int menuLayoutID;
        public String menuLayoutName;
        public String menuLayoutDescription;

        /**
         * + "	mi.ID as menu_item_id, mi.NAME as menu_item_name, mi.url, mi.ELEMENT_CLASS, mi.ELEMENT_ID, mi.IS_DELETED, "
            + "    mt.id as menu_type_id, mt.name as menu_type_name, mt.description as menu_type_description, "
            + "    ma.id as menu_action_id, ma.name as menu_action_name, ma.description as menu_action_description, "
            + "    mlc.id as menu_load_config_id, mlc.name as menu_load_config_name, mlc.description as menu_load_config_description, "
            + "    ml.id as menu_layout_id, ml.name as menu_layout_name, ml.description as menu_layout_description "
         */

        public static Dictionary<Int32, MenuItem> CreateMenuItemFromResultSet(NpgsqlDataReader rs)
        {
            Dictionary<Int32, MenuItem> menuItemsMap = new Dictionary<Int32, MenuItem>();
			while (rs.Read()) {
				MenuItem mi = new MenuItem();
				mi.menuItemID = rs[0] != DBNull.Value? (Int32) rs[0]: -1;
				mi.menuItemName = rs[1] != null && rs[1] != DBNull.Value ? (String)rs[1]: "";
				mi.menuURL = rs[2] != null && rs[2] != DBNull.Value ? (String)rs[2] : "";
                mi.menuElementClass = rs[3] != null && rs[3] != DBNull.Value ? (String)rs[3] : "";
                mi.menuElementID = rs[4] != null && rs[4] != DBNull.Value ? (String)rs[4] : "";
                mi.menuTypeID = rs[6] != DBNull.Value ? (Int32)rs[6] : -1;
                mi.menuTypeName = rs[7] != null && rs[7] != DBNull.Value ? (String)rs[7] : "";
                mi.menuTypeDescription = rs[8] != null && rs[8] != DBNull.Value ? (String)rs[8] : "";
                mi.menuActionID = rs[9] != DBNull.Value ? (Int32)rs[9] : -1;
                mi.menuActionName = rs[10] != null && rs[10] != DBNull.Value ? (String)rs[10] : "";
                mi.menuActionDescription = rs[11] != null && rs[11] != DBNull.Value ? (String)rs[11] : "";
                mi.menuLoadConfigID = rs[12] != DBNull.Value ? (Int32)rs[12] : -1;
                mi.menuLoadConfigName = rs[13] != null && rs[13] != DBNull.Value ? (String)rs[13] : ""; ;
                mi.menuLoadConfigDescription = rs[14] != null && rs[14] != DBNull.Value ? (String)rs[14] : "";
                mi.menuLayoutID = rs[15] != DBNull.Value ? (Int32)rs[15] : -1;
                mi.menuLayoutName = rs[16] != null && rs[16] != DBNull.Value ? (String)rs[16] : "";
                mi.menuLayoutDescription = rs[17] != null && rs[17] != DBNull.Value ? (String)rs[17] : "";
                menuItemsMap.Add(mi.menuItemID, mi);
			}
			return menuItemsMap;
		}

    }
}
