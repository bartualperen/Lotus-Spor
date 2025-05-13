namespace Lotus_Spor;

public partial class AppShell : Shell
{
    internal Action<object, object> CurrentPageChanged;
    string admin1 = ConnectionString.admin1, admin2 = ConnectionString.admin2;
    string UserName = Preferences.Get("LoggedInUser", string.Empty) + " " + Preferences.Get("LoggedInUser2", string.Empty);
    string UserRole = Preferences.Get("UserRole", string.Empty);
    public AppShell()
	{
		InitializeComponent();
		if (UserName == admin1)
		{
            MainPage.IsVisible = false;
            GelirGiderBilgileri.IsVisible = false;
            Duyurular.IsVisible = false;
		}
        if (UserRole == "musteri" && UserRole != "yonetici")
        {
            GelirGiderBilgileri.IsVisible = false;
            AntrenorOdemeleri.IsVisible = false;
            LessonManagementPage.IsVisible = false;
            MeasurementPage.IsVisible = false;
            AntrenorYonet.IsVisible = false;
            DuyuruYap.IsVisible = false;
            OdemeBilgileri.IsVisible = false;
            OlcuEkle.IsVisible = false;
            UyeEkle.IsVisible = false;
            UyeListesi.IsVisible = false;
        }
        if(UserRole == "yonetici" && UserName != admin2 && UserName != admin1)
        {
            MainPage.IsVisible = false;
            GelirGiderBilgileri.IsVisible  =false;
            AntrenorOdemeleri.IsVisible = false;
            AntrenorYonet.IsVisible  =false;
            Duyurular.IsVisible = false;
            //OdemeBilgileri.IsVisible = false;
        }
	}
    private async void OnLogOutClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        Application.Current.MainPage = new NavigationPage(new LoginPage());
        await Task.CompletedTask;
    }
}