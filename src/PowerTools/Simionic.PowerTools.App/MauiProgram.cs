using Microsoft.AspNetCore.Components.WebView.Maui;
using Simionic.PowerTools.App.Data;

namespace Simionic.PowerTools.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddMauiBlazorWebView();

            return builder.Build();
        }
    }
}