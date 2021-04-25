using Microsoft.AspNetCore.Mvc.Testing;

using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace Disk.Api.Test.Controllers
{
    [Collection("Controllers")]
    public class HealthCheckControllerTest : RestIntegrationTest
    {
        public HealthCheckControllerTest(WebApplicationFactory<Startup> factory) : base(factory)
        {

        }

        [Fact]
        public async Task ReturnsOk()
        {
            HttpResponseMessage response = await this.Client.GetAsync("/v1/api/rest/health-check");
            response.EnsureSuccessStatusCode();
            Assert.Contains("Healthy", await response.Content.ReadAsStringAsync());
        }
    }
}
