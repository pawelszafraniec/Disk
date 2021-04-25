using AspNetCore.Proxy;

using Disk.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System.Threading.Tasks;

namespace Disk.Ui.Controllers
{
    [Controller]
    public class ApiProxyController : Controller
    {
        private readonly IConfiguration configuration;

        public ApiProxyController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [Route("/v1/{**rest}")]
        public Task ApiProxyCatchAll(string rest)
        {
            var queryString = this.Request.QueryString.Value;
            return this.HttpProxyAsync($"{this.configuration.GetRequiredValue("App:ApiUrl")}/v1/{rest}{queryString}");
        }

        [Route("/swagger/{**rest}")]
        public Task SwaggerProxyCatchAll(string rest)
        {
            var queryString = this.Request.QueryString.Value;
            return this.HttpProxyAsync($"{this.configuration.GetRequiredValue("App:ApiUrl")}/swagger/{rest}{queryString}");
        }
    }
}
