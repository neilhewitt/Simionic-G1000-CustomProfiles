using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Simionic.CustomProfiles.Web
{
    public static class HttpClientFactory
    {
        public const string BaseAddress = "https://simionic-g1000-profile-database-functions.azurewebsites.net/";
        public const string ApiHostKey = "RdyGRIvTPLAkTlJGe8f8hOaFtdmZSK3sdcFCeKHf0239EqqwzsUc3w==";

        public static HttpClient Client
        {
            get
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(BaseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("x-functions-key", ApiHostKey);
                return client;
            }
        }
        
    }
}
