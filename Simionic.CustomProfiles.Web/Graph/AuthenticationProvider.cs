using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Graph;

namespace Simionic.CustomProfiles.Web
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private IAccessTokenProviderAccessor _accessTokenProviderAccessor;

        public AuthenticationProvider(IAccessTokenProviderAccessor accessTokenProviderAccessor)
        {
            _accessTokenProviderAccessor = accessTokenProviderAccessor;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var result = await _accessTokenProviderAccessor.TokenProvider.RequestAccessToken();
            if (result.TryGetToken(out AccessToken token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }
        }
    }
}
