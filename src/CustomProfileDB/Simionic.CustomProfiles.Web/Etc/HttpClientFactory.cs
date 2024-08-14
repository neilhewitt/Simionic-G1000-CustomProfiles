using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Simionic.CustomProfiles.Web
{
    public static class HttpClientFactory
    {
#if DEBUG
        public const string BaseAddress = "http://localhost:7071";
        public const string ApiHostKey = null;
#else
        public const string BaseAddress = "https://simionic-functions.azurewebsites.net/";
        public const string ApiHostKey = "EGwwcYzIxh1Y7rpLRfCDZyyLJvz8UyBeUbryUsg21BZQAzFuPlZf8g==";
#endif
        private static HttpClient _httpClient;

        public static HttpClient Client => _httpClient;

        static HttpClientFactory()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseAddress);
            _httpClient.DefaultRequestHeaders.Add("x-functions-key", ApiHostKey);
        }
    }
}

