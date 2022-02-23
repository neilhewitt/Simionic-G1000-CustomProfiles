using System;
using System.Net.Http;

namespace Simionic.CustomProfiles.Web
{
    public static class HttpClientFactory
    {
        //public const string BaseAddress = "";

        public static HttpClient Client => new HttpClient();// { BaseAddress = new Uri(BaseAddress) };
    }
}
