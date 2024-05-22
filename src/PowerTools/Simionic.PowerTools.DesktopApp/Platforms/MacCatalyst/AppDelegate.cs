using Foundation;
using Microsoft.Maui.LifecycleEvents;
using UIKit;

namespace Simionic.PowerTools.DesktopApp
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiProgram.CreateMauiAppBuilder();

            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddiOS(iosApp =>
                {
                    iosApp.OnActivated(uiApplication =>
                    {
                        var scene = uiApplication.ConnectedScenes.ToArray()[0];
                        if (scene is not null && scene is UIWindowScene)
                        {
                            var windowScene = (UIWindowScene)scene;
                            WindowManager.AssignWindowObjectAndSetDefaultSize(windowScene);
                        }
                    });
                });
            });

            return builder.Build();
        }
    }
}