using Disk.Api.Data;
using Disk.Common;
using Disk.Common.DTO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Disk.Api.Controllers
{


    /// <summary>
    /// Responsible for authentication features.
    /// </summary>
    [ApiController]
    [Route("/v1/api/rest/authentication")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        public LoginController(IConfiguration config, ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            this.configuration = config;
            this.dbContext = context;
            this.passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Logs in user.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest userModel)
        {
            IActionResult response = this.Unauthorized();
            User? user = this.AuthenticateUser(userModel);
            if (user != null)
            {
                var tokenString = this.GenerateJSONWebToken(user);
                this.HttpContext.Response.Cookies.Append(Cookies.Authentication, tokenString, new CookieOptions
                {
                    SameSite = SameSiteMode.None,
                    Secure = false,
                    HttpOnly = false,
                    IsEssential = true,
                });
                response = this.Ok(new BaseResponse());
            }
            return response;
        }

        private string GenerateJSONWebToken(User userinfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                this.configuration["Jwt:Issuer"],
                this.configuration["Jwt:Audience"],
                new[]
                {
                        new Claim(ClaimTypes.NameIdentifier, userinfo.Login),
                        new Claim(ClaimTypes.Email, userinfo.EmailAddress),
                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Returns information about logged in user.
        /// </summary>
        [Authorize]
        [HttpGet("check")]
        public IActionResult CheckIfLoggedIn()
        {
            return this.Ok(new UserResponse()
            {
                Login = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Email = this.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value
            });
        }

        private User? AuthenticateUser(LoginRequest userinfo)
        {

            IQueryable<User>? userQuery = from e in this.dbContext.Users
                                          where e.Login == userinfo.Login
                                          select e;


            User? user = userQuery.FirstOrDefault();

            if (user != null)
            {
                PasswordVerificationResult veryficationResult = this.passwordHasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    userinfo.Password);

                if (veryficationResult == PasswordVerificationResult.Success ||
                    veryficationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
