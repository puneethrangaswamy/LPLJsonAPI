namespace TopNavApplication.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(LoginCredentials loginCredentials);

        bool IsValidToken(string token);

        string? GetUserName(string token);

        string? GetRoleName(string token);
    }
}
