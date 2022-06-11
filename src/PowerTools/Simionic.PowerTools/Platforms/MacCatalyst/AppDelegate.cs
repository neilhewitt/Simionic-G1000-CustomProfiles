using Foundation;
using Microsoft.Maui.LifecycleEvents;
using UIKit;

namespace Simionic.PowerTools
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        public (int X, int Y, int Width, int Height) FixedSize = (100, 100, 1024, 800);

        protected override MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiProgram.CreateMauiApp();

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
                            if (windowScene.SizeRestrictions != null)
                            {
                                windowScene.SizeRestrictions.MinimumSize = new CoreGraphics.CGSize(FixedSize.Width, FixedSize.Height);
                                windowScene.SizeRestrictions.MaximumSize = windowScene.SizeRestrictions.MinimumSize;
                            }
                        }
                    });
                });
            });

            return builder.Build();
        }
    }
}