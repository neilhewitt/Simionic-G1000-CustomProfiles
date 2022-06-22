using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simionic.PowerTools.App
{
    public static class WindowManager
    {
        private static object _window;

        public static (int Width, int Height) DefaultSize { get; private set; } = (1024, 768); // TODO: set via config

        public static int Width { get; private set; }
        public static int Height { get; private set; }

        public static void ResizeToDefaults()
        {
            Resize(DefaultSize.Width, DefaultSize.Height);
        }

        public static void Resize(int width, int height)
        {
            if (_window != null)
            {
#if WINDOWS
                var window = _window as Microsoft.UI.Windowing.AppWindow;
                window.Resize(new Windows.Graphics.SizeInt32(width, height));
                Width = width;
                Height = height;
#elif MACCATALYST
                var scene = _window as UIKit.UIWindowScene;
                if (scene.SizeRestrictions != null)
                {
                    scene.SizeRestrictions.MinimumSize = new CoreGraphics.CGSize(width, height);
                    scene.SizeRestrictions.MaximumSize = scene.SizeRestrictions.MinimumSize;
                    scene.IncreaseSize(null);
                }
                Width = width;
                Height = height;
#endif
            }
        }

        internal static void AssignWindowObject(object windowObject)
        {
            _window = windowObject;
            ResizeToDefaults();
        }
    }
}
