using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace Simionic.CustomProfiles.Web.Graph
{
    public class GraphServiceClientFactory
    {
        private readonly IAccessTokenProviderAccessor _accessTokenProviderAccessor;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GraphServiceClientFactory> _logger;
        private GraphServiceClient _graphClient;

        public GraphServiceClient GetAuthenticatedClient()
        {
            if (_graphClient == null)
            {
                _graphClient = new GraphServiceClient(_httpClient);
                _graphClient.AuthenticationProvider = new AuthenticationProvider(_accessTokenProviderAccessor);
            }

            return _graphClient;
        }

        public GraphServiceClientFactory(IAccessTokenProviderAccessor accessor, HttpClient httpClient, ILogger<GraphServiceClientFactory> logger)
        {
            _accessTokenProviderAccessor = accessor;
            _httpClient = httpClient;
            _logger = logger;
        }
    }
}
