namespace Lotus_Spor
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            if (!Services.UserSession.IsLoggedIn)
            {
                Current.GoToAsync("//LoginPage");
            }
        }
    }
}
