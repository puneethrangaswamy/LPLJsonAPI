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

        /**
         * "    amm.PARENT_MENU_ITEM_ID as pre_auth_parent_menu_item_id, amm.CHILD_MENU_ITEM_ID as pre_auth_child_menu_item_id, amm.view_order, pamm.start_date, pamm.END_DATE, "
                + "    et.id as ENTITLEMENT_TYPE_ID, et.name as Entitlement_Name, et.description as Entitlement_Description "
         */

        public static List<PreAuthMenuItem> CreatePreAuthMenuItemFromResultSet(NpgsqlDataReader rs)
        {
            List<PreAuthMenuItem> preAuthMenuItemsList = new List<PreAuthMenuItem>();
            while (rs.Read())
            {
                PreAuthMenuItem mi = new PreAuthMenuItem();
                if (rs[0] != DBNull.Value)
                    mi.parentMenuItemId = (Int32)rs[0];
                if (rs[1] != DBNull.Value)
                    mi.childMenuItemId = (Int32)rs[1];
                if (rs[2] != DBNull.Value)
                    mi.viewOrder = (Int32)rs[2];

                if (rs[3] != DBNull.Value)
                    mi.startDate = (DateTime)rs[3];
                if (rs[4] != DBNull.Value)
                    mi.endDate = (DateTime)rs[4];

                if (rs[5] != DBNull.Value)
                    mi.entitlementTypeID = (Int32)rs[5];



                mi.entitlementName = rs[6] != DBNull.Value ? (String)rs[6]: "";
                mi.entitlementDescription = rs[6] != DBNull.Value ? (String)rs[6] : "";

                
                preAuthMenuItemsList.Add(mi);
            }
            return preAuthMenuItemsList;
        }
    }
}
