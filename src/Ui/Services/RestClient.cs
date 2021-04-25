using Disk.Common;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Disk.Ui.Services
{
    [Service]
    public class RestClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly IObjectSerializer serializer;

        private Uri BaseUri
        {
            get
            {
                return new Uri(this.configuration.GetRequiredValue("App:ApiUrl"));
            }
        }


        public RestClient(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IObjectSerializer serializer)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            this.serializer = serializer;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T request) where T : class
        {
            return await this.SendAsync(this.GetRequest(request, uri, HttpMethod.Post));
        }

        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            return await this.SendAsync(this.GetRequest<object>(null, uri, HttpMethod.Get));
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage r)
        {
            using HttpClient httpClient = this.httpClientFactory.CreateClient(nameof(RestClient));
            return await httpClient.SendAsync(r);
        }

        private HttpRequestMessage GetRequest<T>(T? request, string url, HttpMethod method) where T : class
        {
            var httpRequest = new HttpRequestMessage(method, new Uri(this.BaseUri, url))
            {
                Content = request switch
                {
                    { } _ => new StringContent(this.serializer.Serialize(request), Encoding.UTF8, "application/json"),
                    null => null
                }
            };

            string? token = this.GetAuthenticationToken();
            if (token != null)
            {
                var cookie = new CookieHeaderValue(Cookies.Authentication, token);
                httpRequest.Headers.AddCookies(cookie);
            }

            return httpRequest;
        }

        public string? GetAuthenticationToken()
        {
            return this.httpContextAccessor.HttpContext.Request.Cookies[Cookies.Authentication];
        }
    }
}