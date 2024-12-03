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
}