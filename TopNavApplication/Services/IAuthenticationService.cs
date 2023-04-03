using System.Diagnostics;

namespace TopNavApplication.Services
{
    public interface IAuthenticationService
    {
        Task<bool> Authenticate(LoginCredentials credentials);

        Task<bool> ValidateToken(string token);
    }
}
