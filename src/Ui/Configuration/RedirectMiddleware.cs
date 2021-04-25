using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Threading.Tasks;

namespace Disk.Ui.Configuration
{
    public class RedirectMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public RedirectMiddleware(RequestDelegate next, ILogger<RedirectMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await this.next.Invoke(httpContext);

            if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                this.logger.LogInformation("Redirecting to login page");
                httpContext.Response.Redirect("/login");
            }
        }
    }
}
