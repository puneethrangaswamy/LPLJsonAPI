using TopNavApplication.Repositories;

namespace TopNavApplication.Services
{
    public class SimpleAuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public SimpleAuthenticationService(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<bool> Authenticate(LoginCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.Username) ||
                string.IsNullOrEmpty(credentials.Password))
            {
                return false;
            }

            var userExists = await _userRepository.UserExists(credentials.Username, credentials.Password);

            return userExists;
        }

        public Task<bool> ValidateToken(string token)
        {
            var validToken = _tokenService.IsValidToken(token);

            return Task.FromResult(validToken);
        }
    }
}
