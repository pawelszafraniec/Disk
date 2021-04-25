using Microsoft.Net.Http.Headers;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Disk.Common
{
    public static class HttpRequestHeadersExtensions
    {
        public static void AddCookies(this HttpRequestHeaders headers, IEnumerable<CookieHeaderValue> cookies)
        {
            headers.Add("Cookie", string.Join("; ", cookies.Select(cookie => $"{cookie.Name}={cookie.Value}")));
        }

        public static void AddCookies(this HttpRequestHeaders headers, params CookieHeaderValue[] cookies)
        {
            AddCookies(headers, cookies as IEnumerable<CookieHeaderValue>);
        }
    }
}
