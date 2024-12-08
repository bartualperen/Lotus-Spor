using MySql.Data.MySqlClient;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

namespace Lotus_Spor;

public partial class UyeEkle : ContentPage
{
	public UyeEkle()
	{
		InitializeComponent();
	}
    private void OnPhoneEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Sadece sayýlarý filtrele
        string numericInput = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        // Maksimum 10 karakter sýnýrý için güvence
        if (numericInput.Length > 10)
            numericInput = numericInput.Substring(0, 10);

        // Eski deðerle farký kontrol ederek Text'i güncelle
        if (PhoneEntry.Text != numericInput)
        {
            PhoneEntry.Text = numericInput;
        }
    }
    private async void OnAddMemberClicked(object sender, EventArgs e)
    {
        string fullName = NameEntry.Text;
        string telefon = PhoneEntry.Text.ToString();
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
            await DisplayAlert("Hata", "Soyadý belirlemek için en az iki kelime girilmelidir.", "Tamam");
            return;
        }

        if (!ucretValid)
        {
            await DisplayAlert("Hata", "Geçerli bir seans ücreti giriniz.", "Tamam");
            return;
        }

        // Seçilen günleri birleþtirme
        List<string> secilenGunler = new List<string>();
        if (CheckBoxPazartesi.IsChecked) secilenGunler.Add("Pazartesi");
        if (CheckBoxSali.IsChecked) secilenGunler.Add("Salý");
        if (CheckBoxCarsamba.IsChecked) secilenGunler.Add("Çarþamba");
        if (CheckBoxPersembe.IsChecked) secilenGunler.Add("Perþembe");
        if (CheckBoxCuma.IsChecked) secilenGunler.Add("Cuma");
        if (CheckBoxCumartesi.IsChecked) secilenGunler.Add("Cumartesi");
        if (CheckBoxPazar.IsChecked) secilenGunler.Add("Pazar");

        string seansGunleri = string.Join(", ", secilenGunler);

        string query = "INSERT INTO musteriler (isim, soyisim, telefon, cinsiyet, hizmet_turu, seans_gunleri, seans_ucreti, seans_saati, notlar, kayit_tarihi, sifre) " +
                       "VALUES (@isim, @soyisim, @telefon, @cinsiyet, @hizmet_turu, @seans_gunleri, @seans_ucreti, @seans_saati, @notlar, CURRENT_DATE, @sifre)";

        string getLastInsertedIdQuery = "SELECT LAST_INSERT_ID()";

        string seansquery = "INSERT INTO seanslar (tur, saat, tarih, musteri_id) " +
                       "VALUES (@seans_turu, @seans_saati, @seans_tarihi, @musteri_id)";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
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
                        await DisplayAlert("Baþarýlý", "Müþteri bilgileri baþarýyla eklendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Veri eklenirken bir sorun oluþtu.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabaný Hatasý", ex.Message, "Tamam");
        }

        Dictionary<string, DayOfWeek> gunlerMap = new Dictionary<string, DayOfWeek>
        {
            { "Pazartesi", DayOfWeek.Monday },
            { "Salý", DayOfWeek.Tuesday },
            { "Çarþamba", DayOfWeek.Wednesday },
            { "Perþembe", DayOfWeek.Thursday },
            { "Cuma", DayOfWeek.Friday },
            { "Cumartesi", DayOfWeek.Saturday },
            { "Pazar", DayOfWeek.Sunday }
        };

        List<DateTime> seansDönemTarihler = new List<DateTime>();
        DateTime today = DateTime.Today;
        DateTime endDate = today.AddMonths(1); // Bir aylýk süre

        foreach (string gun in secilenGunler)
        {
            if (!gunlerMap.ContainsKey(gun))
                continue;

            DayOfWeek hedefGun = gunlerMap[gun];
            DateTime seansTarihi = today;

            // Seçilen günü bulmak için
            while (seansTarihi.DayOfWeek != hedefGun)
            {
                seansTarihi = seansTarihi.AddDays(1);
            }

            // Belirlenen tarihleri bir aylýk süre içinde ekleme
            while (seansTarihi < endDate)
            {
                if (seansTarihi > DateTime.MaxValue || seansTarihi < DateTime.MinValue)
                {
                    break; // Tarih sýnýrlarýný aþmamak için döngüyü sonlandýr
                }

                seansDönemTarihler.Add(seansTarihi);
                seansTarihi = seansTarihi.AddDays(7); // Haftalýk artýþ
            }
        }


        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                // Son eklenen müþteri ID'sini al
                int musteriId = 0;
                using (MySqlCommand getIdCommand = new MySqlCommand(getLastInsertedIdQuery, connection))
                {
                    musteriId = Convert.ToInt32(await getIdCommand.ExecuteScalarAsync());
                }

                using (MySqlCommand command = new MySqlCommand(seansquery, connection))
                {
                    command.Parameters.AddWithValue("@seans_turu", seansTur);
                    command.Parameters.AddWithValue("@seans_saati", seansSaat.ToString(@"hh\:mm"));
                    command.Parameters.AddWithValue("@musteri_id", musteriId);

                    // Seans tarihi için parametreyi ekle
                    MySqlParameter seansTarihiParam = new MySqlParameter("@seans_tarihi", MySqlDbType.Date);
                    command.Parameters.Add(seansTarihiParam);

                    foreach (DateTime seansTarihi in seansDönemTarihler)
                    {
                        seansTarihiParam.Value = seansTarihi.ToString("yyyy-MM-dd");
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected <= 0)
                        {
                            await DisplayAlert("Hata", "Seans eklenirken bir sorun oluþtu.", "Tamam");
                        }
                    }

                    await DisplayAlert("Baþarýlý", "Seanslar baþarýyla eklendi.", "Tamam");
                    await Navigation.PopAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabaný Hatasý", ex.Message, "Tamam");
        }
    }
    private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        string enteredText = NameEntry.Text;

        if (!string.IsNullOrEmpty(enteredText))
        {
            var words = enteredText.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }

            NameEntry.Text = string.Join(" ", words);
        }
    }

}