using System;
using System.Net.Http;

namespace Simionic.CustomProfiles.Web
{
    public static class HttpClientFactory
    {
        public const string BaseAddress = "https://localhost:7071";

        public static HttpClient Client => new HttpClient() { BaseAddress = new Uri(BaseAddress) };
    }
}
