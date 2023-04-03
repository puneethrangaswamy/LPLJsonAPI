namespace TopNavApplication.Repositories
{
    using Models;
    using Npgsql;
    using TopNavApplication.Models.response;

    public class LPLMenuDataContext
    {
        private static NpgsqlConnection? conn = null;

        private static readonly string sqlGetApplications = "select a.ID, a.NAME, a.DESCRIPTION, al.NAME as LAYOUT_NAME from Application a "
                + "join Application_Layout al on al.id = a.layout_id";

        private static readonly string sqlGetApplication = "select a.ID, a.NAME, a.DESCRIPTION, al.NAME as LAYOUT_NAME from Application a "
                + "join Application_Layout al on al.id = a.layout_id where LOWER(a.Name) = ";

        private static readonly string sqlGetRole = "select  eg.id as entitlement_group_id, eg.name as entitlement_group_name, eg.description as entitlement_group_description " +
            "            from entitlement_group eg" +
            "            join user_entitlement_mapping uem  on eg.id = uem.group_id" +
            "            join   appuser au on uem.user_id = au.id " +
            "            where (au.username = 'USERNAME')";


        public static async Task<Dictionary<int, Application>> GetApplications()
        {
            string query = sqlGetApplications;
            Dictionary<int, Application> appMap = new Dictionary<int, Application>(); ;
            try {
                NpgsqlDataReader rs = await ExecuteQueryAsync(query);
                appMap = Application.CreateApplicationsFromResultSet(rs);
                rs.Close();
            }
            catch (Exception) 
            { 
            
            }

            closeDBConnection();
            return appMap;
	    }

        public static async Task<Application> GetApplication(string appName)
        {
            string query = sqlGetApplication + "'" + appName.ToLower() + "'";
            
            Application? application = new Application();
            try
            {
                var rs = await ExecuteQueryAsync(query);
                application = Application.CreateApplicationFromResultSet(rs);
                rs.Close();
            }
            catch (Exception) 
            { 

            }

            closeDBConnection();

            return application!;
	    }

        public static async Task<string> GetRoleByUserName(string userName)
        {
            string query = sqlGetRole;
            query = query.Replace("USERNAME", userName);
            string? role;

            try
            {
                NpgsqlDataReader rs = await ExecuteQueryAsync(query);
                role = EntitlementGroup.GetRoleByUserName(rs);
                rs.Close();
            }
            catch (Exception e) {

                return e.Message;
            }

            closeDBConnection();

            return role;
        }

        private static void closeDBConnection()
        {
            if (conn != null)
                conn.Close();
        }

        private static async Task<NpgsqlDataReader> ExecuteQueryAsync(string sqlQuery)
        {
            await CreateDBConnection();
            NpgsqlCommand command = new NpgsqlCommand(sqlQuery, conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            return dr;
        }

        private static async Task CreateDBConnection()
        {
            if (conn == null)
            {
                conn = new NpgsqlConnection(" User ID = postgres; Password = root; Server = localhost; Port = 5432; Database = lplmenu; Integrated Security = true; Pooling = true;");
            }

            if (conn.State == System.Data.ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
        }
    }
}
