using Microsoft.AspNetCore.Http;
using NHibernate.Cache;
using Npgsql;
using TopNavApplication.Helper;

namespace TopNavApplication.Model
{
    public class PostAuthMenuItem
    {
        public int parentMenuItemId;
        public int childMenuItemId;
        public int prtViewOrder;
        public int chldViewOrder;
        public DateTime startDate;
        public DateTime endDate;
        public int entitlementTypeID;
        public String entitlementName;
        public String entitlementDescription;
        public int entitlementGroupID;
        public String entitlementGroup;
        public String entitlementGroupDescription;

        /**
         * amm1.PARENT_MENU_ITEM_ID as post_auth_parent_menu_item_id, amm1.CHILD_MENU_ITEM_ID as post_auth_child_menu_item_id, amm1.view_order, amm.start_date, amm.end_date, "
                + "			eg.id as entitlement_group_id, eg.name as entitlement_group_name, eg.description as Entitlement_Group_Description, "
                + "			et.id as entitlement_type_id, et.name as entitlement_name, et. description as Entitlement_Description "
         */

        public static List<PostAuthMenuItem> CreatePostAuthMenuItemFromResultSet(NpgsqlDataReader rs)
        {
            List<PostAuthMenuItem> postAuthMenuItemsList = new List<PostAuthMenuItem>();
		    while (rs.Read()) {
			    PostAuthMenuItem mi = new PostAuthMenuItem();
                if(rs[0] != DBNull.Value) 
                    mi.parentMenuItemId = (Int32) rs[0];

                if (rs[1] != DBNull.Value)
                    mi.childMenuItemId = (Int32) rs[1];

                if (rs[2] != DBNull.Value)
                    mi.prtViewOrder = (Int32) rs[2];

                if (rs[3] != DBNull.Value)
                    mi.chldViewOrder = (Int32)rs[3];

                if (rs[4] != DBNull.Value)
                    mi.startDate = (DateTime)rs[4];

                if (rs[5] != DBNull.Value)
                    mi.endDate = (DateTime)rs[5];

                if (rs[6] != DBNull.Value)
                    mi.entitlementTypeID = (Int32) rs[6];

                if (rs[7] != DBNull.Value)
                    mi.entitlementName = (String) rs[7];

                if (rs[8] != DBNull.Value)
                    mi.entitlementDescription = (String) rs[8];

                if (rs[9] != DBNull.Value)
                    mi.entitlementGroupID = (Int32) rs[9];

                if (rs[10] != DBNull.Value)
                    mi.entitlementGroup= (String) rs[10];

                if (rs[11] != DBNull.Value)
                    mi.entitlementGroupDescription = (String) rs[11];

                

			    postAuthMenuItemsList.Add(mi);
            }
		    return postAuthMenuItemsList;
	    }


    }
}
