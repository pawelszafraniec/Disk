using System.Net.Http;
using System.Threading.Tasks;

namespace Disk.Ui.Services
{
    public static class IObjectSerializerExtensions
    {
        public static async Task<T> DeserializeAsync<T>(this IObjectSerializer serializer, HttpResponseMessage response)
        {
            return serializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
        }
    }
}