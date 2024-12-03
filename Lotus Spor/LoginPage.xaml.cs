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
    public static string Gender { get; private set; }

    public static bool Login(string isim, string soyisim, string sifre)
    {
        using (var conn = Database.GetConnection())
        {
            conn.Open();

            // �nce y�neticiler tablosunu kontrol et
            string yoneticiQuery = "SELECT 'yonetici' AS rol FROM yoneticiler WHERE isim = @isim AND soyisim = @soyisim AND sifre = @sifre;";
            using (var yoneticiCmd = new MySqlCommand(yoneticiQuery, conn))
            {
                yoneticiCmd.Parameters.AddWithValue("@isim", isim);
                yoneticiCmd.Parameters.AddWithValue("@soyisim", soyisim);
                yoneticiCmd.Parameters.AddWithValue("@sifre", sifre);

                using (var reader = yoneticiCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        LoggedInUser = isim;
                        UserRole = "yonetici";
                        return true;
                    }
                }
            }

            // E�er y�netici de�ilse, m��teriler tablosunu kontrol et
            string musteriQuery = "SELECT 'musteri' AS rol, cinsiyet FROM musteriler WHERE isim = @isim AND soyisim = @soyisim AND sifre = @sifre;";
            using (var musteriCmd = new MySqlCommand(musteriQuery, conn))
            {
                musteriCmd.Parameters.AddWithValue("@isim", isim);
                musteriCmd.Parameters.AddWithValue("@soyisim", soyisim);
                musteriCmd.Parameters.AddWithValue("@sifre", sifre);

                using (var reader = musteriCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        LoggedInUser = isim;
                        Gender = reader["cinsiyet"].ToString();
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

    private void OnPhoneEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Sadece say�lar� filtrele
        string numericInput = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        // Maksimum 10 karakter s�n�r� i�in g�vence
        if (numericInput.Length > 10)
            numericInput = numericInput.Substring(0, 10);

        // Eski de�erle fark� kontrol ederek Text'i g�ncelle
        if (PasswordEntry.Text != numericInput)
        {
            PasswordEntry.Text = numericInput;
        }
    }


    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string isim, soyisim;
        string fullName = UsernameEntry.Text;
        string[] nameParts = fullName.Split(' ');

        if (nameParts.Length > 1)
        {
            isim = string.Join(" ", nameParts[..^1]);
            soyisim = nameParts[^1];
            try
            {
                string sifre = PasswordEntry.Text;

                if (LoginManager.Login(isim, soyisim, sifre))
                {
                    // Kullan�c� rol�n� kaydet
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
                    await DisplayAlert("Hata", "Kullan�c� ad� veya �ifre hatal�!", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
    }
    protected override bool OnBackButtonPressed()
    {
        return true;
    }
    private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        // Kullan�c� taraf�ndan girilen metni al
        string enteredText = UsernameEntry.Text;

        // E�er metin bo� de�ilse, kelimelerin ilk harfini b�y�k yap
        if (!string.IsNullOrEmpty(enteredText))
        {
            // Her kelimenin ilk harfini b�y�k yap
            var words = enteredText.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }

            // D�zenlenmi� metni tekrar Entry'ye yaz
            UsernameEntry.Text = string.Join(" ", words);
        }
    }
}