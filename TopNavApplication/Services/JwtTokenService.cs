using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TopNavApplication.Repositories;

namespace TopNavApplication.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtTokenServiceOptions _options;

        public JwtTokenService(IOptions<JwtTokenServiceOptions> options)
        {
            _options = options.Value;
        }

        public string CreateToken2(LoginCredentials loginCredentials)
        {
            DateTime tokenExpiration = DateTime.UtcNow.AddMinutes(_options.DurationInMinutes);

            byte[] apiKeySecretBytes = Convert.FromBase64String(_options.MagicKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityKey = new SymmetricSecurityKey(apiKeySecretBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, _options.Id),
                    new Claim(ClaimTypes.Name, loginCredentials.Username),
                }),
                Expires = tokenExpiration,
                Issuer = _options.Issuer,
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> CreateToken(LoginCredentials credentials)
        {
            var userRole = await LPLMenuDataContext.GetRoleByUserName(credentials.Username);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _options.Id),
                new Claim(ClaimTypes.Name, credentials.Username),
                new Claim(ClaimTypes.Role, userRole)
        };

            var apiSecretKeyBytes = Encoding.UTF8.GetBytes(_options.MagicKey);

            var securityKey = new SymmetricSecurityKey(apiSecretKeyBytes);

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.DurationInMinutes),
                signingCredentials: signingCredentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string? GetUserName(string token)
        {
            if (token == null)
            {
                return null;
            }

            var claimsMap = ExtractClaims(token);

            var userName = claimsMap.Where(kvp => kvp.Key.Equals(ClaimTypes.Name))
                                    .Select(kvp => kvp.Value)
                                    .SingleOrDefault();

            return userName;
        }

        public string? GetRoleName(string token)
        {
            if (token == null)
            {
                return null;
            }

            var claimsMap = ExtractClaims(token);

            var roleName = claimsMap.Where(kvp => kvp.Key.Equals(ClaimTypes.Role))
                                    .Select(kvp => kvp.Value)
                                    .SingleOrDefault();

            return roleName;
        }

        public bool IsValidToken(string token)
        {
            if (token == null)
            {
                return false;
            }

            string jwt; 

            if (token.StartsWith("bearer"))
            {
                jwt = token.Split(' ')[1];
            }
            else
            {
                jwt = token;
            }

            var claimsMap = ExtractClaims(jwt);

            if (claimsMap.IsNullOrEmpty())
            {
                return false;
            }

            string idClaim = claimsMap[ClaimTypes.NameIdentifier];
            string issuerClaim = claimsMap["iss"];
            long expirationClaim = long.Parse(claimsMap["exp"]);

            if (!idClaim.Equals(_options.Id) ||
                !issuerClaim.Equals(_options.Issuer) ||
                expirationClaim < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                return false;
            }

            return true;
        }

        private IDictionary<string, string> ExtractClaims(string jwt)
        {
            var apiSecretKeyBytes = Encoding.UTF8.GetBytes(_options.MagicKey);

            var handler = new JwtSecurityTokenHandler();

            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(apiSecretKeyBytes),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            SecurityToken? validatedToken = null;
            var claimsPrincipal = handler.ValidateToken(jwt, validations, out validatedToken);

            var claimsMap = claimsPrincipal.Claims.ToDictionary(keySelector => keySelector.Type,
                                                                elementSelector => elementSelector.Value);

            return claimsMap;
        }
    }
}
