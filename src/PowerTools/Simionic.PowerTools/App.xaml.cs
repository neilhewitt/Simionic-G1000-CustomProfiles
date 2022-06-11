using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;

namespace Simionic.PowerTools
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        //        public void PositionWindow(int x, int y, int width, int height)
        //        {
        //#if WINDOWS
        //            var platformView = _mainWindow.PlatformView;
        //            platformView.Activate();

        //            var handle = WinRT.Interop.WindowNative.GetWindowHandle(platformView);
        //            var windowID = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);

        //            Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowID);
        //            appWindow.MoveAndResize(new Windows.Graphics.RectInt32(x, y, width, height));
        //#endif
        //            // TODO - is there a way to do this on Mac Catalyst?
        //        }
        //    }
    }
}