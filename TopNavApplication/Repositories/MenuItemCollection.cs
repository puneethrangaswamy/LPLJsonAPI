using System.Collections.ObjectModel;
using TopNavApplication.Models;

namespace TopNavApplication.Repositories
{
    public sealed class MenuItemCollection : KeyedCollection<int, MenuItem>
    {
        protected override int GetKeyForItem(MenuItem item)
        {
            return item.menuItemID;
        }
    }
}
