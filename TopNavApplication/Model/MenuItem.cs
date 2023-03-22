using Microsoft.AspNetCore.Http;
using Npgsql;
using System.ComponentModel.DataAnnotations;

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


        public static Dictionary<Int32, MenuItem> CreateMenuItemFromResultSet(NpgsqlDataReader rs)
        {
            Dictionary<Int32, MenuItem> menuItemsMap = new Dictionary<Int32, MenuItem>();
			while (rs.Read()) {
				MenuItem mi = new MenuItem();
				mi.menuItemID = (Int32) rs[0];
				mi.menuItemName = (String) rs[1];
				mi.menuURL = (String) rs[2];
				mi.menuElementClass = (String) rs[3];
				mi.menuElementID = (String)rs[4];
                mi.menuTypeID = (Int32)rs[5];
                mi.menuTypeName = (String)rs[6];
                mi.menuTypeDescription = (String)rs[7];
                mi.menuActionID = (Int32)rs[8];
                mi.menuActionName = (String)rs[9];
                mi.menuActionDescription = (String)rs[10];
                mi.menuLoadConfigID = (Int32)rs[11];
                mi.menuLoadConfigName = (String)rs[12];
                mi.menuLoadConfigDescription = (String)rs[13];
                mi.menuLayoutID = (Int32)rs[14];
                mi.menuLayoutName = (String)rs[15];
                mi.menuLayoutDescription = (String)rs[16];
                menuItemsMap.Add(mi.menuItemID, mi);
			}
			return menuItemsMap;
		}

    }
}
