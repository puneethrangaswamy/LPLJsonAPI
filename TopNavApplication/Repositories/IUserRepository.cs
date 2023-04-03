namespace TopNavApplication.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string userName, string password);

        Task<string> GetRoleByUserName(string userName);
    }
}
