namespace Simionic.PowerTools
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
            {
#if WINDOWS
                var nativeWindow = handler.PlatformView;
                nativeWindow.Activate();
            	IntPtr windowHandle = PInvoke.User32.GetActiveWindow();
                IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

                int width = 1300;
                int height = 1000;

                appWindow.MoveAndResize(new Windows.Graphics.RectInt32(200, 200, width, height));
#endif
            });
        }
    }
}