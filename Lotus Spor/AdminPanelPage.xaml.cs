namespace Lotus_Spor;

public partial class AdminPanelPage : ContentPage
{
    string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);
    string loggedInUser2 = Preferences.Get("LoggedInUser2", string.Empty);
    string admin1 = ConnectionString.admin1, admin2 = ConnectionString.admin2;
    public AdminPanelPage()
	{
		InitializeComponent();
        string antrenor = loggedInUser + " " + loggedInUser2;
        if (antrenor == admin1 || antrenor == admin2)
        {
            AntrenorYonet.IsVisible = true;
        }
	}
    private async void OnClickedLogOut(Object sender, EventArgs e)
    {
        Preferences.Remove("UserRole");
        await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        await Task.CompletedTask;
    }
    protected override bool OnBackButtonPressed()
    {
        return true; // Geri tuþunu devre dýþý býrak
    }
    private async void OnUyeEkleClicked(Object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new UyeEkle());
    }
    private async void OnOdemeBilgileri(Object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new OdemeBilgileri());
    }
    private async void OnDuyuruYap(Object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new DuyuruYap());
    }
    private async void OnAntrenorYonet(Object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new AntrenorYonet());
    }
    private async void OnOlcuEkleClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new OlcuEkle());
    }
    private async void OnDersYonet(Object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new LessonManagementPage());
    }
    private async void OnOlcuYonet(Object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new MeasurementPage());
    }
    private async void OnUyeListesiClicked(Object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new UyeListesi());
    }
}