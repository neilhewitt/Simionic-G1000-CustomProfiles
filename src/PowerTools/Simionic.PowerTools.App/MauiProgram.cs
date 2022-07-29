using Microsoft.AspNetCore.Components.WebView.Maui;
using Radzen;
using Simionic.PowerTools.App.Data;

namespace Simionic.PowerTools.App
{
    public static class MauiProgram
    {
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

            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();

            return builder;
        }

        public static MauiApp CreateMauiApp()
        {
            return CreateMauiAppBuilder().Build();
        }
    }
}