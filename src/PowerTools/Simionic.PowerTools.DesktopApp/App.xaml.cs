using Simionic.PowerTools.Core;

namespace Simionic.PowerTools.DesktopApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}