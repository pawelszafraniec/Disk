using Microsoft.AspNetCore.Mvc.Testing;

using System.Net.Http;

using Xunit;

using App = Disk.Api.Startup;

namespace Disk.Api.Test
{
    public class RestIntegrationTest : IClassFixture<WebApplicationFactory<App>>
    {
        protected WebApplicationFactory<App> Factory { get; }
        protected HttpClient Client { get; }

        public RestIntegrationTest(WebApplicationFactory<App> factory)
        {
            this.Factory = factory;
            this.Client = factory.CreateClient();
        }
    }
}
