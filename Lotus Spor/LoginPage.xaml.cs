using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Lotus_Spor;

class Database
{
    private static string connectionString = ConnectionString.connectionString;
    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}

public class LoginManager
{
    public static bool IsSpecialUser(string isim, string soyisim)
    {
        string query = "SELECT COUNT(*) FROM yoneticiler WHERE isim = @FirstName AND soyisim = @LastName";

        using (var connection = Database.GetConnection())
        {
            connection.Open();
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", isim);
                command.Parameters.AddWithValue("@LastName", soyisim);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }
    }
    public static string LoggedInUser { get; private set; }
    public static string LoggedInUser2 { get; private set; }
    public static string UserRole { get; private set; }
    public static string Gender { get; private set; }
    public static int LoggedInUserId { get; private set; }
    public static bool Login(string isim, string soyisim, string sifre)
    {
        using (var conn = Database.GetConnection())
        {
            conn.Open();

            // �nce y�neticiler tablosunu kontrol et
            string yoneticiQuery = "SELECT 'yonetici' AS rol, id FROM yoneticiler WHERE isim = @isim AND soyisim = @soyisim AND sifre = @sifre;";
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
                        LoggedInUser2 = soyisim;
                        LoggedInUserId = Convert.ToInt32(reader["id"]);
                        UserRole = "yonetici";
                        return true;
                    }
                }
            }

            // E�er y�netici de�ilse, m��teriler tablosunu kontrol et
            string musteriQuery = "SELECT 'musteri' AS rol, cinsiyet, id FROM musteriler WHERE isim = @isim AND soyisim = @soyisim AND sifre = @sifre;";
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
                        LoggedInUser2 = soyisim;
                        Gender = reader["cinsiyet"].ToString();
                        UserRole = "musteri";
                        LoggedInUserId = Convert.ToInt32(reader["id"]);
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
        LoggedInUserId = 0;
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
        string sifre = PasswordEntry.Text;

        // Bo� alan kontrol�
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(sifre))
        {
            await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurunuz!", "Tamam");
            return; // ��lem burada sonlan�r
        }

        string[] nameParts = fullName.Split(' ');

        if (nameParts.Length > 1)
        {
            isim = string.Join(" ", nameParts[..^1]);
            soyisim = nameParts[^1];

            try
            {
                //if (LoginManager.IsSpecialUser(isim, soyisim)) // �zel kullan�c� kontrol�
                //{
                //    if (!string.IsNullOrEmpty(sifre))
                //    {
                //        // Do�rulama kodu olu�tur
                //        string verificationCode = GenerateVerificationCode();

                //        // SMS g�nder (�rne�in Twilio API kullanarak)
                //        bool smsSent = await SmsService.SendSmsAsync(sifre,
                //            $"Do�rulama kodunuz: {verificationCode}");

                //        if (!smsSent)
                //        {
                //            await DisplayAlert("Hata", "Do�rulama kodu g�nderilemedi!", "Tamam");
                //            return;
                //        }

                //        // Kullan�c�dan do�rulama kodunu al
                //        string enteredCode = await DisplayPromptAsync("Do�rulama",
                //            "L�tfen cep telefonunuza gelen do�rulama kodunu girin:",
                //            "G�nder", "�ptal");

                //        if (enteredCode != verificationCode)
                //        {
                //            await DisplayAlert("Hata", "Do�rulama kodu hatal�!", "Tamam");
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        await DisplayAlert("Hata", "Telefon numaras� bulunamad�!", "Tamam");
                //        return;
                //    }
                //}
                if (LoginManager.Login(isim, soyisim, sifre))
                {
                    // Kullan�c� rol�n� kaydet
                    Preferences.Set("UserRole", LoginManager.UserRole);
                    Preferences.Set("LoggedInUser", LoginManager.LoggedInUser);
                    Preferences.Set("LoggedInUser2", LoginManager.LoggedInUser2);
                    Preferences.Set("Gender", LoginManager.Gender);
                    Preferences.Set("LoggedInUserId", LoginManager.LoggedInUserId.ToString());

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
        else
        {
            await DisplayAlert("Hata", "L�tfen tam ad�n�z� giriniz (isim ve soyisim)!", "Tamam");
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
    private string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // 6 haneli do�rulama kodu
    }
}