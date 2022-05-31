using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;

namespace Simionic.PowerTools
{
    public partial class App : Application
    {
        private IWindowHandler _mainWindow;
        private (int x, int y, int width, int height) _initialPosition = (100, 100, 1920, 1080); // just for now

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
            {
                _mainWindow = handler;
                PositionWindow(_initialPosition.x, _initialPosition.y, _initialPosition.width, _initialPosition.height); // Windows only (for now)
            });
        }

        public void PositionWindow(int x, int y, int width, int height)
        {
#if WINDOWS
            var platformView = _mainWindow.PlatformView;
            platformView.Activate();

            var handle = WinRT.Interop.WindowNative.GetWindowHandle(platformView);
            var windowID = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);

            Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowID);
            appWindow.MoveAndResize(new Windows.Graphics.RectInt32(x, y, width, height));
#endif
            // TODO - is there a way to do this on Mac Catalyst?
        }
    }
}