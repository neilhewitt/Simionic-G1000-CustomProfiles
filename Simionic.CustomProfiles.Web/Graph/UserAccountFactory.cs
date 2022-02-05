using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace Simionic.CustomProfiles.Web.Graph
{
    public class UserAccountFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        private readonly IAccessTokenProviderAccessor _accessTokenProviderAccessor;
        private readonly GraphServiceClientFactory _graphClientFactory;
        private readonly ILogger<UserAccountFactory> _logger;

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
            RemoteUserAccount account,
            RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);
            if (user.Identity.IsAuthenticated)
            {
                try
                {
                    await AddGraphInfoToClaims(_accessTokenProviderAccessor, user);
                }
                catch (AccessTokenNotAvailableException exception)
                {
                    _logger.LogError($"Bad token ({exception.Message})");
                }
                catch (ServiceException exception)
                {
                    _logger.LogError($"Error ({exception.Message})");
                    _logger.LogError($"Response: {exception.RawResponseBody}");
                }
            }

            return user;
        }

        private async Task AddGraphInfoToClaims(IAccessTokenProviderAccessor accessor, ClaimsPrincipal claimsPrincipal)
        {
            GraphServiceClient client = _graphClientFactory.GetAuthenticatedClient();
            User user = await client.Me.Request().Select(u => new { u.DisplayName, u.Mail, u.UserPrincipalName }).GetAsync();

            claimsPrincipal.AddGraphClaims(user);
            Stream photo = await client.Me.Photos["48x48"].Content.Request().GetAsync();
            claimsPrincipal.AddUserGraphPhoto(photo);
        }

        public UserAccountFactory(IAccessTokenProviderAccessor accessor, GraphServiceClientFactory clientFactory, ILogger<UserAccountFactory> logger) : base(accessor)
        {
            _accessTokenProviderAccessor = accessor;
            _graphClientFactory = clientFactory;
            _logger = logger;
        }
    }
}