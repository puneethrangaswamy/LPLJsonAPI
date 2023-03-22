using Microsoft.AspNetCore.Http;
using NHibernate.Cache;
using Npgsql;
using System.Data;

namespace TopNavApplication.Model
{
    public class PreAuthMenuItem
    {
        public int parentMenuItemId;
        public int childMenuItemId;
        public int viewOrder;
        public DateTime startDate;
        public DateTime endDate;
        public int entitlementTypeID;
        public String entitlementName;
        public String entitlementDescription;

        public static List<PreAuthMenuItem> CreatePreAuthMenuItemFromResultSet(NpgsqlDataReader rs)
        {
            List<PreAuthMenuItem> preAuthMenuItemsList = new List<PreAuthMenuItem>();
            while (rs.Read())
            {
                PreAuthMenuItem mi = new PreAuthMenuItem();
                mi.parentMenuItemId = (Int32)rs[0];
                mi.childMenuItemId = (Int32)rs[1];
                mi.viewOrder = (Int32)rs[2];
                mi.entitlementTypeID = (Int32) rs[3];
                mi.entitlementName = (String)rs[4];
                mi.entitlementDescription = (String) rs[5];
                mi.startDate = (DateTime)rs[6];
                mi.endDate = (DateTime)rs[7];
                preAuthMenuItemsList.Add(mi);
            }
            return preAuthMenuItemsList;
        }
    }
}
