using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
namespace Lotus_Spor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            string userRole = Preferences.Get("UserRole", string.Empty);

            if (!string.IsNullOrEmpty(userRole))
            {
                if (userRole == "yonetici")
                    MainPage = new NavigationPage(new AdminPanelPage());
                else
                    MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
