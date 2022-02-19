using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.Web
{
    public class CustomAccountFactory
    : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public CustomAccountFactory(IAccessTokenProviderAccessor accessor,
            IServiceProvider serviceProvider,
            ILogger<CustomAccountFactory> logger)
            : base(accessor)
        {
        }

        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);
            UserInfo.Name = user.Identity.Name;
            if (user.Claims.Any(x => x.Type == "email"))
            {
                UserInfo.Email = user.Claims.SingleOrDefault(x => x.Type == "email").Value;
                UserInfo.OwnerId = await HttpClientFactory.Client.GetStringAsync($"/api/ownerid?email={UserInfo.Email}");
            }
            
            return user;
        }
    }
 }
