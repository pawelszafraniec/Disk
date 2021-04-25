using Microsoft.Net.Http.Headers;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Disk.Common
{
    public static class HttpResponseMessageExtensions
    {
        public static string GetCookie(this HttpResponseMessage response, string cookie)
        {
            return response.GetCookies()[cookie];
        }

        public static IDictionary<string, string> GetCookies(this HttpResponseMessage response)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();
            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                SetCookieHeaderValue.ParseList(values.ToList()).ToList().ForEach(cookie =>
                {
                    result.Add(cookie.Name.Value, cookie.Value.Value);
                });
            }
            return result;
        }
    }
}
