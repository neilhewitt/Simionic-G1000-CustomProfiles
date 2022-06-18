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
    }
}