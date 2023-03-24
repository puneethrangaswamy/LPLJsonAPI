using Microsoft.AspNetCore.Http;
using Npgsql;

namespace TopNavApplication.Model
{
    public class Application
    {
        public int Id { get; set; }

        public string? name { get; set; }

        public string? description { get; set; }

        public string? layoutName { get; set; }

        public static Application CreateApplicationFromResultSet(NpgsqlDataReader rs)
        {
		    while (rs.Read()) {
			    Application application = new Application();
                application.Id = (int) rs[0];
			    application.name = (String) rs[1];
			    application.layoutName = (String) rs[3];
			    application.description = (String) rs[2];
			    return application;
            }
		    return null;
	    }

        public static Dictionary<Int32, Application> CreateApplicationsFromResultSet(NpgsqlDataReader rs)
        {
            Dictionary<Int32, Application> appMap = new Dictionary<Int32, Application>();
            while (rs.Read())
            {
                Application application = new Application();
                application.Id = (int)rs[0];
                application.name = (String)rs[1];
                application.layoutName = (String)rs[2];
                application.description = (String)rs[3];
                appMap.Add(application.Id, application);
            }
            return appMap;
	    }

    }



}