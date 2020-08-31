using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using LevenshteinDistance.Models;

namespace LevenshteinDistance.Controllers
{
    public class AccountController : ApiController
    {
        [Route("api/account/validate")]
        [HttpGet]        
        public TokenResponse Validate(string jwtToken, string userName)
        {
            string message;

            var tokenUsername = AuthenticationModule.ValidateToken(jwtToken, out message);

            if (userName.Equals(tokenUsername))
            {
                return new TokenResponse
                {
                    Status = "Success",
                    Message = "User validated successfully."
                };
            }
            return new TokenResponse
            {
                Status = "Invalid",
                Message = message
            };
        }

        [Route("api/account/generate")]
        [HttpPost]
        public TokenResponse GenerateToken([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                return new TokenResponse
                {
                    Status = "Success",
                    JwtToken = AuthenticationModule.GenerateToken(user.UserName),
                    Message = "Token generated successfully."
                };
            }
            return null;
        }
    }
}