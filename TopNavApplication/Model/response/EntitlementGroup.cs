using Npgsql;

namespace TopNavApplication.Model.response
{
    public class EntitlementGroup
    {
        public int ID { get; set; }
        public string name;
        public string description;

        public static string GetRoleByUserName(NpgsqlDataReader rs)
        {
            EntitlementGroup eg = null;
            while (rs.Read())
            {
                eg = new EntitlementGroup();
                if (rs[0] != DBNull.Value)
                    eg.ID = (int)rs[0];

                if (rs[1] != DBNull.Value)
                    eg.name = rs[1].ToString();

                if (rs[2] != DBNull.Value)
                    eg.description = rs[2].ToString();

            }
            return eg.name;
        }


    }

  
}
