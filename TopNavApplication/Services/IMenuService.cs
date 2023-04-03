using TopNavApplication.Models;
using TopNavApplication.Models.response;

namespace TopNavApplication.Services
{
    public interface IMenuService
    {
        Task<PostAuthMenu> GetMenuItems(string groupName, string appName);
    }
}
