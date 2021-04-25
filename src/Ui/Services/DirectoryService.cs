using Disk.Common.DTO;

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Disk.Ui.Services
{
    public class DirectoryService
    {
        private readonly RestClient restClient;
        private readonly IObjectSerializer serializer;

        public DirectoryService(RestClient restClient, IObjectSerializer serializer)
        {
            this.restClient = restClient;
            this.serializer = serializer;
        }

        public async Task<DirectoryResponse> GetDirectoryAsync(Guid? id, string? query, bool? includeSubdirectories)
        {
            HttpResponseMessage httpResponse = await this.restClient.GetAsync(
                string.IsNullOrWhiteSpace(query)
                ? $"/v1/api/rest/directory/{id?.ToString() ?? ""}"
                : $"/v1/api/rest/directory/{id?.ToString() ?? ""}?query={this.EncodeQuery(query)}&includeSubdirectories={includeSubdirectories ?? false}"
                );

            httpResponse.EnsureSuccessStatusCode();

            return await this.serializer.DeserializeAsync<DirectoryResponse>(httpResponse);
        }

        public async Task<DirectoryBaseResponse> CreateDirectoryAsync(Guid? parentId, string name)
        {
            HttpResponseMessage httpResponse = await this.restClient.PostAsync($"/v1/api/rest/directory/{parentId}",
                                                                               new DirectoryRequest { Name = name });

            httpResponse.EnsureSuccessStatusCode();

            return await this.serializer.DeserializeAsync<DirectoryResponse>(httpResponse);
        }

        private string EncodeQuery(string s)
        {
            Encoding encoding = Encoding.UTF8;
            return HttpUtility.UrlEncode(Convert.ToBase64String(encoding.GetBytes(s)));
        }

    }
}