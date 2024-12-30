using Microsoft.Maui.Platform;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using static Google.Protobuf.Reflection.UninterpretedOption.Types;

namespace Lotus_Spor;

public partial class UyeEkle : ContentPage
{
    List<string> isimListesi = new List<string>();
    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    public ObservableCollection<DoluSeans> DoluSeanslar { get; set; } = new ObservableCollection<DoluSeans>();
    public ObservableCollection<Kisi> GrupDersler { get; set; } = new ObservableCollection<Kisi>();
    private int kullaniciId = int.Parse(Preferences.Get("LoggedInUserId", "0"));
    private string kullaniciAdi = Preferences.Get("LoggedInUser", string.Empty) + " " + Preferences.Get("LoggedInUser2", string.Empty);
    public string grupders;
    public class Kisi
    {
        public string Name { get; set; }
    }
    public class DoluSeans
    {
        public string Gun { get; set; }
        public string BaslangicSaat { get; set; }
        public string HizmetTuru { get; set; }
    }
    public UyeEkle()
	{
		InitializeComponent();
        ResultsCollectionView.ItemsSource = filteredList;
        LoadDoluSeans();
        LoadGrupDersler();
        kisilistele();
    }
    private async void OnPhoneEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Sadece sayıları filtrele
        string numericInput = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        if (numericInput.StartsWith("0"))
        {
            // Uyarı ver
            await DisplayAlert("Uyarı", "Telefon numarası 0 ile başlayamaz.", "Tamam");

            // İlk rakamı kaldır
            numericInput = numericInput.TrimStart('0');
        }

        // Maksimum 10 karakter sınırı için güvence
        if (numericInput.Length > 10)
            numericInput = numericInput.Substring(0, 10);

        // Eski değerle farkı kontrol ederek Text'i güncelle
        if (PhoneEntry.Text != numericInput)
        {
            PhoneEntry.Text = numericInput;
        }
    }
    private async Task<string> GetUniquePhoneNumberAsync(MySqlConnection connection)
    {
        string defaultPhone = "0000000000";

        // Mevcut en büyük varsayılan telefon numarasını bul
        string query = "SELECT telefon FROM musteriler WHERE telefon LIKE '000000000%' ORDER BY telefon DESC LIMIT 1";

        using (MySqlCommand command = new MySqlCommand(query, connection))
        {
            object result = await command.ExecuteScalarAsync();

            if (result != null)
            {
                string lastPhone = result.ToString();
                if (long.TryParse(lastPhone, out long lastPhoneNumber))
                {
                    // Son numarayı bir artır
                    return (lastPhoneNumber + 1).ToString().PadLeft(10, '0');
                }
            }
        }

        // Varsayılan numara kullanılmamışsa, başlangıç değerini döndür
        return defaultPhone;
    }
    private async void OnAddMemberClicked(object sender, EventArgs e)
    {
        string fullName = NameEntry.Text;
        string antrenorName = AntrenorNameEntry.Text;
        string telefon = PhoneEntry.Text;
        string cinsiyet = GenderPicker.SelectedItem?.ToString();
        string seansTur = SeansPicker.SelectedItem?.ToString();
        string notlar = NoteEntry.Text;
        float seansUcret;
        bool ucretValid = float.TryParse(UcretEntry.Text, out seansUcret);

        string[] nameParts = fullName.Split(' ');
        string isim, soyisim;

        if (string.IsNullOrWhiteSpace(telefon))
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                telefon = await GetUniquePhoneNumberAsync(connection);
            }
        }

        using (MySqlConnection connection = Database.GetConnection())
        {
            await connection.OpenAsync();
            string checkQuery = "SELECT COUNT(*) FROM musteriler WHERE telefon = @telefon";
            using (MySqlCommand command = new MySqlCommand(checkQuery, connection))
            {
                command.Parameters.AddWithValue("@telefon", telefon);
                int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                if (count > 0)
                {
                    await DisplayAlert("Hata", "Bu telefon numarası zaten mevcut.", "Tamam");
                    return;
                }
            }
        }

        if (nameParts.Length > 1)
        {
            isim = string.Join(" ", nameParts[..^1]);
            soyisim = nameParts[^1];
        }
        else
        {
            await DisplayAlert("Hata", "Soyadı belirlemek için en az iki kelime girilmelidir.", "Tamam");
            return;
        }

        if (!ucretValid)
        {
            await DisplayAlert("Hata", "Geçerli bir seans ücreti giriniz.", "Tamam");
            return;
        }

        // Seçilen günleri birleştirme
        List<string> secilenGunler = new List<string>();
        if (CheckBoxPazartesi.IsChecked) secilenGunler.Add("Pazartesi");
        if (CheckBoxSali.IsChecked) secilenGunler.Add("Salı");
        if (CheckBoxCarsamba.IsChecked) secilenGunler.Add("Çarşamba");
        if (CheckBoxPersembe.IsChecked) secilenGunler.Add("Perşembe");
        if (CheckBoxCuma.IsChecked) secilenGunler.Add("Cuma");
        if (CheckBoxCumartesi.IsChecked) secilenGunler.Add("Cumartesi");
        if (CheckBoxPazar.IsChecked) secilenGunler.Add("Pazar");
        if (CheckBoxGrup.IsChecked) grupders = "evet";

        string seansGunleri = string.Join(", ", secilenGunler);

        string query = "INSERT INTO musteriler (isim, soyisim, telefon, cinsiyet, hizmet_turu, seans_gunleri, seans_ucreti, notlar, kayit_tarihi, sifre, grup) " +
                       "VALUES (@isim, @soyisim, @telefon, @cinsiyet, @hizmet_turu, @seans_gunleri, @seans_ucreti, @notlar, @kayit_tarihi, @sifre, @grup)";

        string getLastInsertedIdQuery = "SELECT LAST_INSERT_ID()";

        string seansquery = "INSERT INTO seanslar (tur, musteri_id, antrenor, tarih, saat, grup) VALUES (@seans_turu, @musteri_id, @antrenor, @seans_tarihi, @seans_saati, @grup)";

        DateTime selectedDate = DatePickerKayitTarihi.Date;

        Dictionary<string, DayOfWeek> gunlerMap = new Dictionary<string, DayOfWeek>
        {
            { "Pazartesi", DayOfWeek.Monday },
            { "Salı", DayOfWeek.Tuesday },
            { "Çarşamba", DayOfWeek.Wednesday },
            { "Perşembe", DayOfWeek.Thursday },
            { "Cuma", DayOfWeek.Friday },
            { "Cumartesi", DayOfWeek.Saturday },
            { "Pazar", DayOfWeek.Sunday }
        };

        Dictionary<string, TimeSpan> seansSaatleri = new Dictionary<string, TimeSpan>();
        List<DateTime> seansDönemTarihler = new List<DateTime>();
        DateTime today = DateTime.Today;
        DateTime endDate = today.AddMonths(12);

        foreach (string gun in secilenGunler)
        {
            switch (gun)
            {
                case "Pazartesi":
                    seansSaatleri["Pazartesi"] = StartTimePazartesi.Time;
                    break;
                case "Salı":
                    seansSaatleri["Salı"] = StartTimeSali.Time;
                    break;
                case "Çarşamba":
                    seansSaatleri["Çarşamba"] = StartTimeCarsamba.Time;
                    break;
                case "Perşembe":
                    seansSaatleri["Perşembe"] = StartTimePersembe.Time;
                    break;
                case "Cuma":
                    seansSaatleri["Cuma"] = StartTimeCuma.Time;
                    break;
                case "Cumartesi":
                    seansSaatleri["Cumartesi"] = StartTimeCumartesi.Time;
                    break;
                case "Pazar":
                    seansSaatleri["Pazar"] = StartTimePazar.Time;
                    break;
                default:
                    break;
            }

            if (!seansSaatleri.ContainsKey(gun) || seansSaatleri[gun] == default(TimeSpan))
            {
                await DisplayAlert("Hata", $"{gun} için bir saat seçilmelidir.", "Tamam");
                return;
            }

            DayOfWeek hedefGun = gunlerMap[gun];
            DateTime seansTarihi = today;

            // Seçilen günü bulmak için
            while (seansTarihi.DayOfWeek != hedefGun)
            {
                seansTarihi = seansTarihi.AddDays(1);
            }

            // Tarihleri bir aylık süre içinde ekleme
            while (seansTarihi < endDate)
            {
                seansDönemTarihler.Add(seansTarihi);
                seansTarihi = seansTarihi.AddDays(7); // Haftalık artış
            }
        }

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
                    command.Parameters.AddWithValue("@kayit_tarihi", selectedDate.ToString("yyyy-MM-dd")); // Seçilen tarihi ekleyin
                    command.Parameters.AddWithValue("@sifre", telefon);
                    command.Parameters.AddWithValue("@notlar", notlar);
                    command.Parameters.AddWithValue("@grup", grupders ?? null);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Başarılı", "Müşteri bilgileri başarıyla eklendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Veri eklenirken bir sorun oluştu.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabanı Hatası", ex.Message, "Tamam");
        }

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                int musteriId = 0;
                using (MySqlCommand getIdCommand = new MySqlCommand(getLastInsertedIdQuery, connection))
                {
                    musteriId = Convert.ToInt32(await getIdCommand.ExecuteScalarAsync());
                }

                using (MySqlCommand command = new MySqlCommand(seansquery, connection))
                {
                    command.Parameters.AddWithValue("@seans_turu", seansTur);
                    command.Parameters.AddWithValue("@musteri_id", musteriId);
                    command.Parameters.AddWithValue("@antrenor", antrenorName);
                    command.Parameters.AddWithValue("@grup", grupders ?? null);

                    MySqlParameter seansTarihiParam = new MySqlParameter("@seans_tarihi", MySqlDbType.Date);
                    MySqlParameter seansSaatiParam = new MySqlParameter("@seans_saati", MySqlDbType.VarChar);
                    command.Parameters.Add(seansTarihiParam);
                    command.Parameters.Add(seansSaatiParam);

                    foreach (DateTime seansTarihi in seansDönemTarihler)
                    {
                        string gunAdi = seansTarihi.DayOfWeek switch
                        {
                            DayOfWeek.Monday => "Pazartesi",
                            DayOfWeek.Tuesday => "Salı",
                            DayOfWeek.Wednesday => "Çarşamba",
                            DayOfWeek.Thursday => "Perşembe",
                            DayOfWeek.Friday => "Cuma",
                            DayOfWeek.Saturday => "Cumartesi",
                            DayOfWeek.Sunday => "Pazar",
                            _ => throw new InvalidOperationException("Geçersiz gün")
                        };

                        seansTarihiParam.Value = seansTarihi.ToString("yyyy-MM-dd");
                        seansSaatiParam.Value = seansSaatleri[gunAdi].ToString(@"hh\:mm");

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected <= 0)
                        {
                            await DisplayAlert("Hata", "Seans eklenirken bir sorun oluştu.", "Tamam");
                        }
                    }

                    await DisplayAlert("Başarılı", "Seanslar başarıyla eklendi.", "Tamam");
                    await Navigation.PopAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabanı Hatası", ex.Message, "Tamam");
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
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            string selectedName = e.CurrentSelection[0] as string;

            if (!string.IsNullOrEmpty(selectedName))
            {
                AntrenorNameEntry.Text = selectedName;  // Seçilen ismi Entry'ye yaz
                ResultsCollectionView.IsVisible = false;  // Önerileri gizle
                GetKullaniciId(selectedName);
            }
        }
    }
    private async void GetKullaniciId(string fullName)
    {
        try
        {
            var connectionString = Database.GetConnection();
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string[] nameParts = fullName.Split(' ');
                string isim = nameParts[0];
                string soyisim = nameParts.Length > 1 ? nameParts[1] : "";

                // Kullanıcıyı bulmak için sorgu
                string query = "SELECT id FROM yoneticiler WHERE isim = @isim AND soyisim = @soyisim";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@isim", isim);
                    command.Parameters.AddWithValue("@soyisim", soyisim);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        kullaniciId = Convert.ToInt32(result);
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Kullanıcı bulunamadı.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while fetching user ID: " + ex.Message, "OK");
        }
    }
    private async void OnAntrenornameTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            ResultsCollectionView.IsVisible = false;
            filteredList.Clear();
            return;
        }

        // Listeyi filtrele
        var suggestions = isimListesi
            .Where(isimSoyisim => isimSoyisim.ToLower().Contains(searchText))
            .ToList();

        // CollectionView'ı güncelle
        filteredList.Clear();
        foreach (var suggestion in suggestions)
        {
            filteredList.Add(suggestion);
        }

        ResultsCollectionView.IsVisible = filteredList.Count > 0;
    }
    private async void kisilistele()
    {
        try
        {
            var connectionString = Database.GetConnection();
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string query = "SELECT isim, soyisim FROM yoneticiler";

                // SQL komutunu çalıştır
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Verileri listeye ekle
                        while (reader.Read())
                        {
                            string isim = reader["isim"].ToString();
                            string soyisim = reader["soyisim"].ToString();
                            string fullName = $"{isim} {soyisim}";  // İsim ve soyisim birleşiyor
                            isimListesi.Add(fullName);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while fetching data: " + ex, "OK");
        }
    }
    private async void LoadDoluSeans()
    {
        string query = @"
        SELECT 
            seanslar.tarih AS tarih, 
            seanslar.saat AS saat,
            musteriler.isim AS musteri_isim, 
            musteriler.soyisim AS musteri_soyisim,
            musteriler.hizmet_turu AS hizmet_turu
        FROM 
            seanslar
        JOIN 
            musteriler ON seanslar.musteri_id = musteriler.id
        WHERE 
            seanslar.antrenor = @antrenor 
            AND YEARWEEK(seanslar.tarih, 2) = YEARWEEK(CURDATE(), 1)
        ORDER BY 
            seanslar.tarih ASC, 
            seanslar.saat ASC;";

        try
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@antrenor", kullaniciAdi);

                    using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                    {
                        // Gün bazında satır dizinlerini takip ediyoruz
                        var gunBazliSatirIndexleri = new Dictionary<string, int>
                    {
                        { "Pazartesi", 1 },
                        { "Salı", 1 },
                        { "Çarşamba", 1 },
                        { "Perşembe", 1 },
                        { "Cuma", 1 },
                        { "Cumartesi", 1 },
                        { "Pazar", 1 }
                    };

                        while (await reader.ReadAsync())
                        {
                            string gun = DateTime.Parse(reader["tarih"].ToString()).ToString("dddd", new CultureInfo("tr-TR"));
                            string saat = reader["saat"].ToString();
                            string hizmetTuru = reader["hizmet_turu"].ToString();

                            int columnIndex = GetColumnIndex(gun); // Günün sütun indeksini al
                            int rowIndex = gunBazliSatirIndexleri[gun]; // O günün mevcut satır indeksini al

                            // Yeni satır oluşturulmasını sağla
                            LessonsListView.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                            // Saat ve hizmet türünü içeren bir StackLayout oluştur
                            StackLayout seansLayout = new StackLayout
                            {
                                Orientation = StackOrientation.Vertical,
                                HorizontalOptions = LayoutOptions.Center,
                                Children =
                            {
                                new Label
                                {
                                    Text = saat,
                                    HorizontalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Bold
                                },
                                new Label
                                {
                                    Text = hizmetTuru,
                                    HorizontalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Italic
                                }
                            }
                            };

                            // Seansı tabloya ekle
                            Grid.SetColumn(seansLayout, columnIndex);
                            Grid.SetRow(seansLayout, rowIndex);
                            LessonsListView.Children.Add(seansLayout);

                            // İlgili gün için satır indeksini güncelle
                            gunBazliSatirIndexleri[gun]++;

                            DoluSeanslar.Add(new DoluSeans
                            {
                                Gun = DateTime.Parse(reader["tarih"].ToString()).ToString(),
                                BaslangicSaat = saat,
                                HizmetTuru = hizmetTuru
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluştu: {ex.Message}", "Tamam");
        }
    }
    private async void LoadGrupDersler()
    {
        string query = "SELECT isim FROM musteriler WHERE grup='evet'";
        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            GrupDersler.Add(new Kisi
                            {
                                Name = reader["isim"].ToString(),
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluştu: {ex.Message}", "Tamam");
        }
    }
    private int GetColumnIndex(string gun)
    {
        switch (gun)
        {
            case "Pazartesi": return 0;
            case "Salı": return 1;
            case "Çarşamba": return 2;
            case "Perşembe": return 3;
            case "Cuma": return 4;
            case "Cumartesi": return 5;
            case "Pazar": return 6;
            default: return -1;
        }
    }
    private bool isResettingTime = false;
    private async void OnTimeSelected(object sender, EventArgs e)
    {
        if (!CheckBoxGrup.IsChecked)
        {
            if (isResettingTime)
                return;

            var timePicker = sender as TimePicker;

            if (timePicker == null)
                return;

            string selectedDay = string.Empty;
            if (timePicker == StartTimePazartesi) selectedDay = "Pazartesi";
            else if (timePicker == StartTimeSali) selectedDay = "Salı";
            else if (timePicker == StartTimeCarsamba) selectedDay = "Çarşamba";
            else if (timePicker == StartTimePersembe) selectedDay = "Perşembe";
            else if (timePicker == StartTimeCuma) selectedDay = "Cuma";
            else if (timePicker == StartTimeCumartesi) selectedDay = "Cumartesi";
            else if (timePicker == StartTimePazar) selectedDay = "Pazar";

            string selectedTime = timePicker.Time.ToString(@"hh\:mm");

            DateTime today = DateTime.Today;
            DateTime targetDate = today;

            Dictionary<string, DayOfWeek> dayMap = new Dictionary<string, DayOfWeek>
            {
                { "Pazartesi", DayOfWeek.Monday },
                { "Salı", DayOfWeek.Tuesday },
                { "Çarşamba", DayOfWeek.Wednesday },
                { "Perşembe", DayOfWeek.Thursday },
                { "Cuma", DayOfWeek.Friday },
                { "Cumartesi", DayOfWeek.Saturday },
                { "Pazar", DayOfWeek.Sunday }
            };

            if (dayMap.ContainsKey(selectedDay))
            {
                while (targetDate.DayOfWeek != dayMap[selectedDay])
                {
                    targetDate = targetDate.AddDays(1);
                }
            }

            string selectedDateString = targetDate.ToString("yyyy-MM-dd");

            // Çakışma kontrolü
            var isConflicting = DoluSeanslar.Any(doluSeans =>
                DateTime.Parse(doluSeans.Gun).ToString("yyyy-MM-dd") == selectedDateString &&
                TimeSpan.Parse(doluSeans.BaslangicSaat).ToString(@"hh\:mm") == selectedTime);

            if (isConflicting)
            {
                await DisplayAlert("Uyarı", $"{selectedDay} günü {selectedTime} saatinde bir ders zaten mevcut!", "Tamam");
                isResettingTime = true;
                timePicker.Time = TimeSpan.Zero;
                isResettingTime = false;
            }
            else
            {
                await DisplayAlert("Uygun", $"{selectedDay} günü {selectedTime} saatinde ders eklenebilir.", "Tamam");
            }
        }
    }
}