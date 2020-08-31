using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using Microsoft.IdentityModel.Tokens;

namespace LevenshteinDistance.Models
{
    public class AuthenticationModule
    {
        private static string SecretKey = string.Empty;

        private static int ExpireTimeInMinute = Convert.ToInt16(ConfigurationManager.AppSettings["ExpireTimeInMinute"]);

        private static string GetSecretKey()
        {
            HMACSHA256 hmac = new HMACSHA256();
            string secretKey = Convert.ToBase64String(hmac.Key);
            return secretKey;
        }

        public static string GenerateToken(string username)
        {
            SecretKey = GetSecretKey();

            byte[] key = Convert.FromBase64String(SecretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username)}),
                Expires = DateTime.UtcNow.AddMinutes(ExpireTimeInMinute),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

        public static ClaimsPrincipal GetPrincipal(string token, out string message)
        {
            message = string.Empty;

            try
            {               
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null) return null;
                    
                byte[] key = Convert.FromBase64String(SecretKey);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);

                return principal;
            }
            catch(Exception ex)
            {
                message = ex.Message?.ToString();
                return null;
            }
        }

        public static string ValidateToken(string token, out string message)
        {           
            string username = null;
            message = string.Empty;

            ClaimsPrincipal principal = GetPrincipal(token, out message);

            if (principal == null)
                return null;

            ClaimsIdentity identity = null;

            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }
    }
}