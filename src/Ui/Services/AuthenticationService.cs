using Disk.Common;
using Disk.Common.DTO;
using Disk.Ui.Authentication;

using Microsoft.AspNetCore.Http;

using System.Net.Http;
using System.Threading.Tasks;

namespace Disk.Ui.Services
{
    public class AuthenticationService
    {
        private readonly RestClient client;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IObjectSerializer serializer;

        public AuthenticationService(
            RestClient restClient,
            IHttpContextAccessor httpContextAccessor,
            IObjectSerializer serializer)
        {
            this.client = restClient;
            this.httpContextAccessor = httpContextAccessor;
            this.serializer = serializer;
        }

        public async Task<UserPrincipal?> AuthenticateAsync(string login, string password)
        {
            try
            {
                HttpResponseMessage response = await this.client.PostAsync(
                    "/v1/api/rest/authentication/login",
                    new LoginRequest(login, password)
                );

                response.EnsureSuccessStatusCode();

                string? token = response.GetCookie(Cookies.Authentication);

                return new UserPrincipal("user", token);
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserPrincipal?> CheckAuthenticationAsync()
        {
            string? token = this.httpContextAccessor.HttpContext.Request.Cookies[Cookies.Authentication];

            if (token == null)
            {
                return null;
            }

            try
            {
                HttpResponseMessage response = await this.client.GetAsync("/v1/api/rest/authentication/check");
                response.EnsureSuccessStatusCode();
                UserResponse? user = await this.serializer.DeserializeAsync<UserResponse>(response);
                return new UserPrincipal(user.Login ?? "[no login]");
            }
            catch
            {
                return null;
            }
        }
    }
}
