using Microsoft.AspNetCore.Components.WebView.Maui;
using Simionic.PowerTools.Data;

namespace Simionic.PowerTools
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            return CreateMauiAppBuilder().Build();
        }

        public static MauiAppBuilder CreateMauiAppBuilder()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		    builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            return builder;
        }
    }
}