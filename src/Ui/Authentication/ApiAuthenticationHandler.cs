using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Text.Encodings.Web;
using System.Threading.Tasks;

using AuthenticationService = Disk.Ui.Services.AuthenticationService;

namespace Disk.Ui.Authentication
{
    public class ApiAuthenticationHandler : AuthenticationHandler<ApiAuthenticationOptions>
    {
        private readonly AuthenticationService authenticationService;
        private readonly ILogger logger;

        public ApiAuthenticationHandler(AuthenticationService authenticationService,
                                        IOptionsMonitor<ApiAuthenticationOptions> options,
                                        ILoggerFactory loggerFactory,
                                        UrlEncoder encoder,
                                        ISystemClock clock) : base(options, loggerFactory, encoder, clock)
        {
            this.authenticationService = authenticationService;
            this.logger = loggerFactory.CreateLogger<ApiAuthenticationHandler>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            UserPrincipal? user = await this.authenticationService.CheckAuthenticationAsync();

            return user != null
                ? AuthenticateResult.Success(new AuthenticationTicket(user, "apiAuthentication"))
                : AuthenticateResult.NoResult();
        }
    }
}
