using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddMsalAuthentication<RemoteAuthenticationState, RemoteUserAccount>(options =>
            {
                options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("email");
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                //options.ProviderOptions.LoginMode = "redirect";
            })
                .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, CustomAccountFactory>();
            
            await builder.Build().RunAsync();
        }
    }
 }
