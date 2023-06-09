﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TopNavApplication.Model.response;

namespace TopNavApplication.Util
{
    public class TokenUtil
    {
        private static long expires = 300000; //System.currentTimeMillis() + 1000L * 60 * 60;

        private static string MAGIC_KEY = "eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9kYXRlb2ZiaXJ0aCI6IjIvMjUvMTk5MSAxMjowMDowMCBBTSIsIm5iZiI6MTY0MTQwNjk2MCwiZXhwIjoxNjQxNDEwNTYwLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDAwLyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcwMDAvIn0=";

        private static  string ID = "lplauth";
	
        private static  string ISSUER = "lplauthissuer";

        public static string createToken(Login login)
        {
            Console.WriteLine(" ----- Create Token ------");
            StringBuilder tokenBuilder = new StringBuilder();
            tokenBuilder.Append(login.username);
            return generateJWT("lplauth", "lplauthissuer", tokenBuilder.ToString(), expires);
        }

        private static string generateJWT(string id, string issuer, string subject, long ttlMillis)
        {

            DateTime now = DateTime.UtcNow;

            //if it has been specified, let's add the expiration

            DateTime exp = now.AddMinutes(5);

            //We will sign our JWT with our ApiKey secret

            byte[] apiKeySecretBytes = Convert.FromBase64String(MAGIC_KEY);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Name, subject),
        }),
                Expires = exp,
                Issuer = issuer,
                //     Audience = myAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(apiKeySecretBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool validateToken(string authToken)
        {
            Console.WriteLine("=== Validate Token ===");
            if (null == authToken)
                return false;

            Dictionary<string, string> authMap = verifyJWT(authToken);

            if (authMap.IsNullOrEmpty())
            {
                return false;
            }

            string tIssuer = authMap["iss"];
            string tID = authMap[ClaimTypes.NameIdentifier];

            long tExpires = long.Parse( authMap["exp"] );


            if (!tID.Equals(ID) || !tIssuer.Equals(ISSUER))
            {
                return false;
            }

            if (tExpires < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                return false;
            }

            return true;
        }

        private static Dictionary<string, string> verifyJWT(string jwt)
        {

            var TokenInfo = new Dictionary<string, string>();

            byte[] key = Convert.FromBase64String(MAGIC_KEY);


            var handler = new JwtSecurityTokenHandler();

            SecurityToken validatedToken = null;


            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(jwt, validations, out validatedToken);
           

            foreach (var claim in claims.Claims)
            {
                TokenInfo.Add(claim.Type, claim.Value);
            }

            return TokenInfo;
           
        }

        public static string getUserNameFromToken(string authToken)
        {
            Console.WriteLine("----- Get Username From TOken ----");
            if (null == authToken)
            {
                return null;
            }

            Dictionary<string, string> authMap = verifyJWT(authToken);
            if (authMap.IsNullOrEmpty())
            {
                return null;
            }

            return authMap[ClaimTypes.Name];
        }

    }
}
