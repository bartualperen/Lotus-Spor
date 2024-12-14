namespace Lotus_Spor;

public partial class AdminPanelPage : ContentPage
{
	public AdminPanelPage()
	{
		InitializeComponent();
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
    private async void OnDuyuruYap(Object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new DuyuruYap());
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
}