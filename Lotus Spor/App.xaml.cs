namespace Lotus_Spor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Kullanıcı rolünü alıyoruz
            string userRole = Preferences.Get("UserRole", string.Empty);

            // AppShell oluşturuluyor
            var appShell = new AppShell();

            // Kullanıcı rolüne göre sayfa yönlendirmesi yapıyoruz
            if (!string.IsNullOrEmpty(userRole))
            {
                if (userRole == "yonetici")
                {
                    // Admin paneli için LessonManagementPage'e yönlendirme yapıyoruz
                    appShell.GoToAsync("//LessonManagementPage");

                    return new Window(appShell);
                }
                else
                {
                    // Normal kullanıcı için MainPage'e yönlendirme yapıyoruz
                    appShell.GoToAsync("//MainPage");

                    return new Window(appShell);
                }
            }
            else
            {
                return new Window(new LoginPage());
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            // Tema ayarlarını yapıyoruz
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
