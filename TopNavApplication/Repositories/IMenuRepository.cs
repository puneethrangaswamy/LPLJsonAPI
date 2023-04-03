using TopNavApplication.Models;

namespace TopNavApplication.Repositories
{
    public interface IMenuRepository
    {
        Task<MenuItemCollection> GetMenuItems();

        Task<IEnumerable<PostAuthMenuItem>> GetMenuItems(string groupName, int appId);
    }
}
