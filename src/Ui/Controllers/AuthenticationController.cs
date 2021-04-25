using Disk.Common;
using Disk.Ui.Authentication;
using Disk.Ui.Models;
using Disk.Ui.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Disk.Ui.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILogger logger;
        private readonly AuthenticationService authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationService authenticationService)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpGet]
        [Route("/login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? this.RedirectToHome() as IActionResult
                : this.View(new LoginViewModel());
        }

        [HttpPost]
        [Route("/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Request request)
        {
            UserPrincipal? result = await this.authenticationService.AuthenticateAsync(request.Login, request.Password);

            if (result != null)
            {
                this.HttpContext.Response.Cookies.Append(Cookies.Authentication, result.Token, new CookieOptions()
                {

                });

                return this.RedirectToHome();
            }

            return this.View(new LoginViewModel()
            {
                Message = "Incorrect username or password",
                Login = request.Login
            });
        }

        [HttpPost]
        [Route("/logout")]
        public IActionResult Logout()
        {
            this.HttpContext.Response.Cookies.Delete(Cookies.Authentication);
            return this.RedirectToLogin();
        }


        private RedirectResult RedirectToHome() => this.Redirect("/");

        private RedirectResult RedirectToLogin() => this.Redirect("/login");
    }

    public class Request
    {
        [Required]
        [StringLength(32)]
        public string Login { get; set; } = null!;

        [Required]
        [StringLength(32)]
        public string Password { get; set; } = null!;
    }
}
