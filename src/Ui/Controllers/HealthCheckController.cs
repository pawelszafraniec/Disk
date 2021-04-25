using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

using System.Threading.Tasks;

namespace Disk.Ui.Controllers
{
    [Route("v1/ui-api/rest/health-check")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService healthCheckService;
        private readonly HealthCheckOptions healthCheckOptions;

        public HealthCheckController(IOptions<HealthCheckOptions> healthCheckOptions, HealthCheckService healthCheckService)
        {
            this.healthCheckService = healthCheckService;
            this.healthCheckOptions = healthCheckOptions.Value;
        }

        [HttpGet]
        public async Task GetHealthCheck()
        {
            HealthReport health = await this.healthCheckService.CheckHealthAsync();
            await UIResponseWriter.WriteHealthCheckUIResponse(this.HttpContext, health);
        }
    }
}
