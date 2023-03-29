﻿using Npgsql;

namespace TopNavApplication.Model.response
{
    public class EntitlementGroup
    {
        public int ID { get; set; }
        public string? Name;
        public string? Description;

        public static string GetRoleByUserName(NpgsqlDataReader rs)
        {
            EntitlementGroup eg = null;
            while (rs.Read())
            {
                eg = new EntitlementGroup();
                if (rs[0] != DBNull.Value)
                    eg.ID = (int)rs[0];

                if (rs[1] != DBNull.Value)
                    eg.Name = rs[1].ToString();

                if (rs[2] != DBNull.Value)
                    eg.Description = rs[2].ToString();

            }
            if(eg != null && !string.IsNullOrEmpty(eg.Name))
            {
                return eg.Name;
            }
            return "";
        }


    }

  
}
