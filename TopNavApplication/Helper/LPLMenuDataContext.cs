namespace TopNavApplication.Helper
{

    using Microsoft.EntityFrameworkCore;
    using Model;
    using Npgsql;
    using TopNavApplication.Model.response;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    public class LPLMenuDataContext
    {
        private static NpgsqlConnection conn = null;

        private static String sqlGetMenuItems = "select "
            + "	mi.ID as menu_item_id, mi.NAME as menu_item_name, mi.url, mi.ELEMENT_CLASS, mi.ELEMENT_ID, mi.IS_DELETED, "
            + "    mt.id as menu_type_id, mt.name as menu_type_name, mt.description as menu_type_description, "
            + "    ma.id as menu_action_id, ma.name as menu_action_name, ma.description as menu_action_description, "
            + "    mlc.id as menu_load_config_id, mlc.name as menu_load_config_name, mlc.description as menu_load_config_description, "
            + "    ml.id as menu_layout_id, ml.name as menu_layout_name, ml.description as menu_layout_description "
            + "from menu_item mi "
            + "left join menu_type mt on mi.type_id = mt.id "
            + "left join menu_action ma on mi.action_id = ma.id "
            + "left join menu_load_config mlc on mi.LOAD_CONFIG_ID = mlc.id "
            + "left join menu_layout ml on mi.LAYOUT_ID = ml.id "
            + "where mi.is_deleted = 0";

        private static String sqlGetPreAuthMenuItems = "select "
                + "    amm.PARENT_MENU_ITEM_ID as pre_auth_parent_menu_item_id, amm.CHILD_MENU_ITEM_ID as pre_auth_child_menu_item_id, amm.view_order, pamm.start_date, pamm.END_DATE, "
                + "    et.id as ENTITLEMENT_TYPE_ID, et.name as Entitlement_Name, et.description as Entitlement_Description "
                + "from pre_auth_menu_mapping pamm "
                + "join application_menu_mapping amm on amm.ID = pamm.APPLICATION_MENU_ITEM_ID "
                + "join entitlement_type et on pamm.entitlement_type_id = et.id "
                + "where amm.APPLICATION_ID = ";

        private static String sqlGetPostAuthMenuItems = "select "
                + "			amm1.PARENT_MENU_ITEM_ID as post_auth_parent_menu_item_id, amm1.CHILD_MENU_ITEM_ID as post_auth_child_menu_item_id, amm1.view_order, amm.start_date, amm.end_date, "
                + "			eg.id as entitlement_group_id, eg.name as entitlement_group_name, eg.description as Entitlement_Group_Description, "
                + "			et.id as entitlement_type_id, et.name as entitlement_name, et. description as Entitlement_Description "
                + "			from auth_menu_mapping amm "
                + "            join application_menu_mapping amm1 on amm1.ID = amm.APPLICATION_MENU_ITEM_ID "
                + "			join entitlement_group eg on eg.id = amm.group_id and LOWER(eg.NAME) = 'GROUP_SUBSITUTE' "
                + "			left join entitlement_type et on et.id = amm.entitlement_type_id "
                + "            where amm1.APPLICATION_ID = APPLICATION_ID_SUBSITUTE "
                + "			order by amm1.PARENT_MENU_ITEM_ID";

        private static String sqlGetApplications = "select a.ID, a.NAME, a.DESCRIPTION, al.NAME as LAYOUT_NAME from Application a "
                + "join Application_Layout al on al.id = a.layout_id";

        private static String sqlGetApplication = "select a.ID, a.NAME, a.DESCRIPTION, al.NAME as LAYOUT_NAME from Application a "
                + "join Application_Layout al on al.id = a.layout_id where LOWER(a.Name) = ";

        private static String sqlGetLoginUserDetails = "SELECT * FROM USER where USERNAME = ";

        private static string sqlGetRole = "select  eg.id as entitlement_group_id, eg.name as entitlement_group_name, eg.description as entitlement_group_description " +
            "            from entitlement_group eg" +
            "            join user_entitlement_mapping uem  on eg.id = uem.group_id" +
            "            join   appuser au on uem.user_id = au.id " +
            "            where (au.username = 'USERNAME' AND au.password = 'PASSWORD')";


        public static Dictionary<Int32, Application> getApplications()
        {
            String query = sqlGetApplications;
            Dictionary<Int32, Application> appMap = new Dictionary<int, Application>(); ;
            try {
                NpgsqlDataReader rs = executeQuery(query);
                appMap = Application.CreateApplicationsFromResultSet(rs);
                rs.Close();
            }
            catch (Exception ex) { }
            closeDBConnection();
            return appMap;
	    }

        public static Application getApplication(String appName)
        {
            String query = sqlGetApplication + "'" + appName.ToLower() + "'";
            
            Application application = new Application();
            try
            {
                NpgsqlDataReader rs = executeQuery(query);
                application = Application.CreateApplicationFromResultSet(rs);
                rs.Close();
            }
            catch (Exception ex) { }
            closeDBConnection();
            return application;
	    }

        public static Dictionary<Int32, MenuItem> getMenuItems()
        {
            String query = sqlGetMenuItems;
            Dictionary<Int32, MenuItem> menuItemsMap = new Dictionary<int, MenuItem>();
            try
            {
                NpgsqlDataReader rs = executeQuery(query);
                menuItemsMap = MenuItem.CreateMenuItemFromResultSet(rs);
                rs.Close();
            }
            catch (Exception ex) { }
            closeDBConnection();
            return menuItemsMap;
	    }

        public static List<PreAuthMenuItem> getPreAuthMenuItems(int applicationID)
        {
            String query = sqlGetPreAuthMenuItems + applicationID;
            List<PreAuthMenuItem> preAuthMenuItemsList = new List<PreAuthMenuItem> ();
            try
            {
                NpgsqlDataReader rs = executeQuery(query);
                preAuthMenuItemsList = PreAuthMenuItem.CreatePreAuthMenuItemFromResultSet(rs);
                rs.Close();
            }
            catch (Exception ex) { }
            closeDBConnection();
            return preAuthMenuItemsList;
	    }

        public static string getRoleByUserName(string userName, string password)
        {
            string query = sqlGetRole;
            query = query.Replace("USERNAME", userName);
            query = query.Replace("PASSWORD", password);
            string role=null;

            try
            {
                NpgsqlDataReader rs = executeQuery(query);
                role = EntitlementGroup.GetRoleByUserName(rs);
                rs.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);

            }

            return role;
        }


        public static List<PostAuthMenuItem> getPostAuthMenuItems(String groupName, int appID)
        {
            String query = sqlGetPostAuthMenuItems;
            query = query.Replace("GROUP_SUBSITUTE", groupName.ToLower());
		    query = query.Replace("APPLICATION_ID_SUBSITUTE", ""+appID);
            List<PostAuthMenuItem> postAuthMenuItemsList = null;
            try
            {
                NpgsqlDataReader rs = executeQuery(query);
                postAuthMenuItemsList = PostAuthMenuItem.CreatePostAuthMenuItemFromResultSet(rs);
                rs.Close();
            }
            catch (Exception ex) { }
            return postAuthMenuItemsList;
	    }
        


        private static void createDBConnection()
        {
            if(conn == null)
                conn = new NpgsqlConnection("Server=127.0.0.1;User Id=postgres;Password=root;Database=lplmenu;");

            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
        }

        private static void closeDBConnection()
        {
            if (conn != null)
                conn.Close();
        }

        private static NpgsqlDataReader executeQuery(String sqlQuery)
        {
            createDBConnection();
            NpgsqlCommand command = new NpgsqlCommand(sqlQuery, conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            return dr;
        }
    }
}
