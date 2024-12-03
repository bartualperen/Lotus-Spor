using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

namespace Lotus_Spor;

public partial class UyeEkle : ContentPage
{
	public UyeEkle()
	{
		InitializeComponent();
	}
    private async void OnAddMemberClicked(object sender, EventArgs e)
    {
        string fullName = NameEntry.Text;
        string telefon = PhoneEntry.Text;
        string cinsiyet = GenderPicker.SelectedItem?.ToString();
        string seansTur = SeansPicker.SelectedItem?.ToString();
        string notlar = NoteEntry.Text;
        float seansUcret;
        bool ucretValid = float.TryParse(UcretEntry.Text, out seansUcret);
        TimeSpan seansSaat = SeansSaatPicker.Time;

        string[] nameParts = fullName.Split(' ');
        string isim, soyisim;

        if (nameParts.Length > 1)
        {
            isim = string.Join(" ", nameParts[..^1]);
            soyisim = nameParts[^1];
        }
        else
        {
            await DisplayAlert("Hata", "Soyad� belirlemek i�in en az iki kelime girilmelidir.", "Tamam");
            return;
        }

        if (!ucretValid)
        {
            await DisplayAlert("Hata", "Ge�erli bir seans �creti giriniz.", "Tamam");
            return;
        }

        // Se�ilen g�nleri birle�tirme
        List<string> secilenGunler = new List<string>();
        if (CheckBoxPazartesi.IsChecked) secilenGunler.Add("Pazartesi");
        if (CheckBoxSali.IsChecked) secilenGunler.Add("Sal�");
        if (CheckBoxCarsamba.IsChecked) secilenGunler.Add("�ar�amba");
        if (CheckBoxPersembe.IsChecked) secilenGunler.Add("Per�embe");
        if (CheckBoxCuma.IsChecked) secilenGunler.Add("Cuma");
        if (CheckBoxCumartesi.IsChecked) secilenGunler.Add("Cumartesi");
        if (CheckBoxPazar.IsChecked) secilenGunler.Add("Pazar");

        string seansGunleri = string.Join(", ", secilenGunler);

        string connectionString = "Server=193.35.154.209;Port=3306;Database=lotussporDB;Uid=bartu;Pwd=Acx544~%[]4fx;";
        string query = "INSERT INTO musteriler (isim, soyisim, telefon, cinsiyet, hizmet_turu, seans_gunleri, seans_ucreti, seans_saati, notlar, kayit_tarihi, sifre) " +
                       "VALUES (@isim, @soyisim, @telefon, @cinsiyet, @hizmet_turu, @seans_gunleri, @seans_ucreti, @seans_saati, @notlar, CURRENT_DATE, @sifre)";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@isim", isim);
                    command.Parameters.AddWithValue("@soyisim", soyisim);
                    command.Parameters.AddWithValue("@telefon", telefon);
                    command.Parameters.AddWithValue("@cinsiyet", cinsiyet);
                    command.Parameters.AddWithValue("@hizmet_turu", seansTur);
                    command.Parameters.AddWithValue("@seans_gunleri", seansGunleri);
                    command.Parameters.AddWithValue("@seans_ucreti", seansUcret);
                    command.Parameters.AddWithValue("@sifre", telefon);
                    command.Parameters.AddWithValue("@seans_saati", seansSaat.ToString(@"hh\:mm"));
                    command.Parameters.AddWithValue("@notlar", notlar);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Ba�ar�l�", "M��teri bilgileri ba�ar�yla eklendi.", "Tamam");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Veri eklenirken bir sorun olu�tu.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritaban� Hatas�", ex.Message, "Tamam");
        }
    }
    private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        // Kullan�c� taraf�ndan girilen metni al
        string enteredText = NameEntry.Text;

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
            NameEntry.Text = string.Join(" ", words);
        }
    }

}