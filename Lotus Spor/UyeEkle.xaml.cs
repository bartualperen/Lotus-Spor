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
        // Sadece say�lar� filtrele
        string numericInput = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        // Maksimum 10 karakter s�n�r� i�in g�vence
        if (numericInput.Length > 10)
            numericInput = numericInput.Substring(0, 10);

        // Eski de�erle fark� kontrol ederek Text'i g�ncelle
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
                        await DisplayAlert("Ba�ar�l�", "M��teri bilgileri ba�ar�yla eklendi.", "Tamam");
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

        Dictionary<string, DayOfWeek> gunlerMap = new Dictionary<string, DayOfWeek>
        {
            { "Pazartesi", DayOfWeek.Monday },
            { "Sal�", DayOfWeek.Tuesday },
            { "�ar�amba", DayOfWeek.Wednesday },
            { "Per�embe", DayOfWeek.Thursday },
            { "Cuma", DayOfWeek.Friday },
            { "Cumartesi", DayOfWeek.Saturday },
            { "Pazar", DayOfWeek.Sunday }
        };

        List<DateTime> seansD�nemTarihler = new List<DateTime>();
        DateTime today = DateTime.Today;
        DateTime endDate = today.AddMonths(1); // Bir ayl�k s�re

        foreach (string gun in secilenGunler)
        {
            if (!gunlerMap.ContainsKey(gun))
                continue;

            DayOfWeek hedefGun = gunlerMap[gun];
            DateTime seansTarihi = today;

            // Se�ilen g�n� bulmak i�in
            while (seansTarihi.DayOfWeek != hedefGun)
            {
                seansTarihi = seansTarihi.AddDays(1);
            }

            // Belirlenen tarihleri bir ayl�k s�re i�inde ekleme
            while (seansTarihi < endDate)
            {
                if (seansTarihi > DateTime.MaxValue || seansTarihi < DateTime.MinValue)
                {
                    break; // Tarih s�n�rlar�n� a�mamak i�in d�ng�y� sonland�r
                }

                seansD�nemTarihler.Add(seansTarihi);
                seansTarihi = seansTarihi.AddDays(7); // Haftal�k art��
            }
        }


        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                // Son eklenen m��teri ID'sini al
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

                    // Seans tarihi i�in parametreyi ekle
                    MySqlParameter seansTarihiParam = new MySqlParameter("@seans_tarihi", MySqlDbType.Date);
                    command.Parameters.Add(seansTarihiParam);

                    foreach (DateTime seansTarihi in seansD�nemTarihler)
                    {
                        seansTarihiParam.Value = seansTarihi.ToString("yyyy-MM-dd");
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected <= 0)
                        {
                            await DisplayAlert("Hata", "Seans eklenirken bir sorun olu�tu.", "Tamam");
                        }
                    }

                    await DisplayAlert("Ba�ar�l�", "Seanslar ba�ar�yla eklendi.", "Tamam");
                    await Navigation.PopAsync();
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