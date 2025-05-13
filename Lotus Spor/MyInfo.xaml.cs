using CommunityToolkit.Maui.Converters;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace Lotus_Spor;

public partial class MyInfo : ContentPage
{
    string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);
    string loggedInUser2 = Preferences.Get("LoggedInUser2", string.Empty);
    string UserRole = Preferences.Get("UserRole", string.Empty);
    string Gender = Preferences.Get("Gender", string.Empty);
    string LoggedInUserId = Preferences.Get("LoggedInUserId", string.Empty);
    string PhoneNo = Preferences.Get("PhoneNo", string.Empty);
    string Sifre = Preferences.Get("Sifre", string.Empty);
    public MyInfo()
    {
        InitializeComponent();
        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        // Global deðiþkenler: LoggedInUser, LoggedInUser2, UserRole, Gender
        isimEntry.Text = loggedInUser;
        soyisimEntry.Text = loggedInUser2;
        telefonEntry.Text = PhoneNo;
        sifreEntry.Text = Sifre;
        sifreEntry.IsPassword = true;

        if (UserRole == "musteri")
        {
            cinsiyetLabel.IsVisible = true;
            cinsiyetPicker.IsVisible = true;

            if (!string.IsNullOrEmpty(Gender))
                cinsiyetPicker.SelectedItem = Gender;
        }
    }

    private async void OnGuncelleClicked(object sender, EventArgs e)
    {
        var yeniIsim = isimEntry.Text;
        var yeniSoyisim = soyisimEntry.Text;
        var yeniSifre = sifreEntry.Text;
        var yeniTelefon = telefonEntry.Text;
        var yeniCinsiyet = cinsiyetPicker.SelectedItem?.ToString();

        using (var conn = Database.GetConnection())
        {
            conn.Open();

            string query = "";
            if (UserRole == "yonetici")
            {
                query = "UPDATE yoneticiler SET isim = @isim, soyisim = @soyisim, sifre = @sifre, telefon = @telefon WHERE id = @id";
            }
            else
            {
                query = "UPDATE musteriler SET isim = @isim, soyisim = @soyisim, sifre = @sifre, cinsiyet = @cinsiyet, telefon = @telefon WHERE id = @id";
            }

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@isim", yeniIsim);
                cmd.Parameters.AddWithValue("@soyisim", yeniSoyisim);
                cmd.Parameters.AddWithValue("@sifre", yeniSifre);
                cmd.Parameters.AddWithValue("@telefon", yeniTelefon);
                cmd.Parameters.AddWithValue("@id", LoggedInUserId);

                if (UserRole != "yonetici")
                    cmd.Parameters.AddWithValue("@cinsiyet", yeniCinsiyet ?? "");

                cmd.ExecuteNonQuery();
            }
        }

        // Global bilgileri güncelle
        loggedInUser = yeniIsim;
        loggedInUser2 = yeniSoyisim;
        PhoneNo = yeniTelefon;
        if (UserRole == "musteri")
            Gender = yeniCinsiyet;

        await DisplayAlert("Baþarýlý", "Bilgileriniz güncellendi.", "Tamam");
    }
    private bool isPasswordHidden = true;

    public string EyeIcon => isPasswordHidden ? "Resources/Images/eye_closed.png" : "Resources/Images/eye_open.png"; // Göz ikonlarý

    private void TogglePasswordVisibility(object sender, EventArgs e)
    {
        isPasswordHidden = !isPasswordHidden;
        sifreEntry.IsPassword = isPasswordHidden;

        var button = sender as ImageButton;
        button.Source = isPasswordHidden ? "Resources/Images/eye_closed.png" : "Resources/Images/eye_open.png";
    }

}
