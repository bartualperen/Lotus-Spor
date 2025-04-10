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

        protected override void OnStart()
        {
            base.OnStart();

            // Uygulamanın teması başlangıçta belirlenir ve ilgili renkler otomatik olarak güncellenir.
            if (AppInfo.RequestedTheme == AppTheme.Dark)
            {
                Application.Current.Resources["EntryBackgroundColor"] = Color.FromArgb("#f7f7f7");
                Application.Current.Resources["EntryPlaceholderColor"] = Color.FromArgb("#D3D3D3");
            }
            else
            {
                Application.Current.Resources["EntryBackgroundColor"] = Color.FromArgb("#000000");
                Application.Current.Resources["EntryPlaceholderColor"] = Color.FromArgb("#808080");
            }
        }
    }
}
