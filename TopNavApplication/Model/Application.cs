using Npgsql;

namespace TopNavApplication.Model
{
    public class Application
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? LayoutName { get; set; }

        public static Application CreateApplicationFromResultSet(NpgsqlDataReader rs)
        {
		    while (rs.Read()) {
			    Application application = new Application();
                application.Id = (int) rs[0];
			    application.Name = (String) rs[1];
			    application.LayoutName = (String) rs[3];
			    application.Description = (String) rs[2];
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
                application.Name = (String)rs[1];
                application.LayoutName = (String)rs[2];
                application.Description = (String)rs[3];
                appMap.Add(application.Id, application);
            }
            return appMap;
	    }

    }



}