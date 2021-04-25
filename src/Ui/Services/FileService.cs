using Disk.Common;
using Disk.Common.DTO;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Disk.Ui.Services
{
    public class FileService
    {
        private readonly RestClient restClient;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IObjectSerializer serializer;
        private readonly IConfiguration configuration;

        public FileService(RestClient restClient,
                           IObjectSerializer serializer,
                           IHttpClientFactory httpClientFactory,
                           IConfiguration configuration)
        {
            this.restClient = restClient;
            this.serializer = serializer;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task<FileResponse> GetFileInfo(Guid id)
        {
            HttpResponseMessage httpResponse = await this.restClient.GetAsync($"/v1/api/rest/file/{id}/info");

            httpResponse.EnsureSuccessStatusCode();

            return await this.serializer.DeserializeAsync<FileResponse>(httpResponse);
        }

        public async Task UploadFile(Guid directoryId, IFormFile file)
        {
            using HttpClient client = this.httpClientFactory.CreateClient();


            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"{this.configuration.GetRequiredValue("App:ApiUrl")}/v1/api/rest/file/{directoryId}")
            {
                Content = new MultipartFormDataContent
                {
                    { new StreamContent(file.OpenReadStream()), "file", file.FileName }
                }
            };


            string? token = this.restClient.GetAuthenticationToken();

            if (token != null)
            {
                var cookie = new CookieHeaderValue(Cookies.Authentication, token);
                request.Headers.AddCookies(cookie);
            }

            using HttpResponseMessage message = await client.SendAsync(request);

            message.EnsureSuccessStatusCode();
        }
    }
}
