using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Lotus_Spor;

class Database
{
    private static string connectionString = "Server=193.35.154.209;Port=3306;Database=lotussporDB;Uid=bartu;Pwd=Acx544~%[]4fx;";
    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}

public class LoginManager
{
    public static string LoggedInUser { get; private set; }
    public static string UserRole { get; private set; }

    public static bool Login(string kullaniciAdi, string sifre)
    {
        using (var conn = Database.GetConnection())
        {
            conn.Open();

            // Önce yöneticiler tablosunu kontrol et
            string yoneticiQuery = "SELECT 'yonetici' AS rol FROM yoneticiler WHERE isim = @kullaniciAdi AND sifre = @sifre;";
            using (var yoneticiCmd = new MySqlCommand(yoneticiQuery, conn))
            {
                yoneticiCmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                yoneticiCmd.Parameters.AddWithValue("@sifre", sifre);

                using (var reader = yoneticiCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        LoggedInUser = kullaniciAdi;
                        UserRole = "yonetici";
                        return true;
                    }
                }
            }

            // Eðer yönetici deðilse, müþteriler tablosunu kontrol et
            string musteriQuery = "SELECT 'musteri' AS rol FROM musteriler WHERE isim = @kullaniciAdi AND sifre = @sifre;";
            using (var musteriCmd = new MySqlCommand(musteriQuery, conn))
            {
                musteriCmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                musteriCmd.Parameters.AddWithValue("@sifre", sifre);

                using (var reader = musteriCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        LoggedInUser = kullaniciAdi;
                        UserRole = "musteri";
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static void Logout()
    {
        LoggedInUser = null;
        UserRole = null;
    }
}


public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = this;
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            string kullaniciAdi = UsernameEntry.Text;
            string sifre = PasswordEntry.Text;

            if (LoginManager.Login(kullaniciAdi, sifre))
            {
                // Kullanýcý rolünü kaydet
                Preferences.Set("UserRole", LoginManager.UserRole);

                if (LoginManager.UserRole == "yonetici")
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new AdminPanelPage());
                }
                else if (LoginManager.UserRole == "musteri")
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                }
            }
            else
            {
                await DisplayAlert("Hata", "Kullanýcý adý veya þifre hatalý!", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", ex.Message, "Tamam");
        }
    }
    protected override bool OnBackButtonPressed()
    {
        return true; // Geri tuþunu devre dýþý býrak
    }
}