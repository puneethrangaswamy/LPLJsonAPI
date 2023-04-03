using Npgsql;

namespace TopNavApplication.Models
{
    public class MenuItem
    {
        public int menuItemID;
        public string? menuItemName;
        public string? menuURL;
        public string? menuElementClass;
        public string? menuElementID;
        public int menuTypeID;
        public string? menuTypeName;
        public string? menuTypeDescription;
        public int menuActionID;
        public string? menuActionName;
        public string? menuActionDescription;
        public int menuLoadConfigID;
        public string? menuLoadConfigName;
        public string? menuLoadConfigDescription;
        public int menuLayoutID;
        public string? menuLayoutName;
        public string? menuLayoutDescription;

        public static Dictionary<Int32, MenuItem> CreateMenuItemFromResultSet(NpgsqlDataReader rs)
        {
            Dictionary<Int32, MenuItem> menuItemsMap = new Dictionary<Int32, MenuItem>();
			while (rs.Read()) {
				MenuItem mi = new MenuItem();
				mi.menuItemID = rs[0] != DBNull.Value? (Int32) rs[0]: -1;
				mi.menuItemName = rs[1] != null && rs[1] != DBNull.Value ? (String)rs[1]: string.Empty;
				mi.menuURL = rs[2] != null && rs[2] != DBNull.Value ? (String)rs[2] : string.Empty;
                mi.menuElementClass = rs[3] != null && rs[3] != DBNull.Value ? (String)rs[3] : string.Empty;
                mi.menuElementID = rs[4] != null && rs[4] != DBNull.Value ? (String)rs[4] : string.Empty;
                mi.menuTypeID = rs[6] != DBNull.Value ? (Int32)rs[6] : -1;
                mi.menuTypeName = rs[7] != null && rs[7] != DBNull.Value ? (String)rs[7] : string.Empty;
                mi.menuTypeDescription = rs[8] != null && rs[8] != DBNull.Value ? (String)rs[8] : string.Empty;
                mi.menuActionID = rs[9] != DBNull.Value ? (Int32)rs[9] : -1;
                mi.menuActionName = rs[10] != null && rs[10] != DBNull.Value ? (String)rs[10] : string.Empty;
                mi.menuActionDescription = rs[11] != null && rs[11] != DBNull.Value ? (String)rs[11] : string.Empty;
                mi.menuLoadConfigID = rs[12] != DBNull.Value ? (Int32)rs[12] : -1;
                mi.menuLoadConfigName = rs[13] != null && rs[13] != DBNull.Value ? (String)rs[13] : string.Empty; ;
                mi.menuLoadConfigDescription = rs[14] != null && rs[14] != DBNull.Value ? (String)rs[14] : string.Empty;
                mi.menuLayoutID = rs[15] != DBNull.Value ? (Int32)rs[15] : -1;
                mi.menuLayoutName = rs[16] != null && rs[16] != DBNull.Value ? (String)rs[16] : string.Empty;
                mi.menuLayoutDescription = rs[17] != null && rs[17] != DBNull.Value ? (String)rs[17] : string.Empty;
                menuItemsMap.Add(mi.menuItemID, mi);
			}
			return menuItemsMap;
		}

    }
}
