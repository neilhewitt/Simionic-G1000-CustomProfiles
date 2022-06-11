﻿using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Simionic.PowerTools.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        private bool _mainWindowCreated = false;

        public (int X, int Y, int Width, int Height) FixedSize = (100, 100, 1024, 800);

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiProgram.CreateMauiApp();

            builder.ConfigureLifecycleEvents(events =>
            {
                events.AddWindows(windows =>
                {
                    windows.OnWindowCreated(window =>
                    {
                        if (!_mainWindowCreated) // only runs for first window - child windows can be resizable
                        {
                            IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            WindowId nativeWindowId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                            AppWindow appWindow = AppWindow.GetFromWindowId(nativeWindowId);
                            appWindow.SetPresenter(AppWindowPresenterKind.CompactOverlay);

                            appWindow.MoveAndResize(new RectInt32(FixedSize.X, FixedSize.Y, FixedSize.Width, FixedSize.Height));
                            _mainWindowCreated = true;
                        }
                    });
                });
            });

            return builder.Build();
        }
    }
}