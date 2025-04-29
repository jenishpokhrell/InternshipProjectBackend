using backend.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class GenerateJWTToken
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public GenerateJWTToken(UserManager<ApplicationUser> userManager, IConfiguration configuration
            )
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        //Helper method for generating token for user
        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            if (!userRoles.Contains("ADMIN"))
            {
                if (!string.IsNullOrEmpty(user.Firstname))
                    authClaims.Add(new Claim("Firstname", user.Firstname));

                if (!string.IsNullOrEmpty(user.Lastname))
                    authClaims.Add(new Claim("Lastname", user.Lastname));
            }

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: signingCredentials
            );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);
            return token;

        }
    }
}
