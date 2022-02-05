using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Simionic.CustomProfiles.Web.Graph;
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://graph.microsoft.com") }); // use MS Graph to get extended user info
            
            builder.Services.AddMsalAuthentication<RemoteAuthenticationState, RemoteUserAccount>(options =>
            {
                options.ProviderOptions.DefaultAccessTokenScopes.Add("User.Read"); // need this scope to get user info from Graph
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.LoginMode = "redirect";
            })
            .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, UserAccountFactory>();
            
            builder.Services.AddScoped<GraphServiceClientFactory>();

            await builder.Build().RunAsync();
        }
    }
}
