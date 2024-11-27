namespace Lotus_Spor;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        if (username == "admin" && password == "1234")
        {
            Services.UserSession.IsLoggedIn = true;
            Services.UserSession.UserName = username;

            Application.Current.MainPage = new AppShell(); // Ana sayfaya geçiþ
        }
        else
        {
            await DisplayAlert("Hata", "Kullanýcý adý veya þifre yanlýþ!", "Tamam");
        }
    }
}