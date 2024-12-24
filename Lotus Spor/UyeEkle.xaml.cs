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
    private int kullaniciId = int.Parse(Preferences.Get("LoggedInUserId", "0"));
    private string kullaniciAdi = Preferences.Get("LoggedInUser", string.Empty) + " " + Preferences.Get("LoggedInUser2", string.Empty);
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
        kisilistele();
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
        string antrenorName = AntrenorNameEntry.Text;
        string telefon = PhoneEntry.Text.ToString();
        string cinsiyet = GenderPicker.SelectedItem?.ToString();
        string seansTur = SeansPicker.SelectedItem?.ToString();
        string notlar = NoteEntry.Text;
        float seansUcret;
        bool ucretValid = float.TryParse(UcretEntry.Text, out seansUcret);

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

        string seansquery = "INSERT INTO seanslar (tur, saat, tarih, musteri_id, antrenor) " +
                       "VALUES (@seans_turu, @seans_saati, @seans_tarihi, @musteri_id, @antrenor)";

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
                    command.Parameters.AddWithValue("@musteri_id", musteriId);
                    command.Parameters.AddWithValue("@antrenor", antrenorName);

                    // Seans tarihi için parametreyi ekle
                    MySqlParameter seansTarihiParam = new MySqlParameter("@seans_tarihi", MySqlDbType.Date);
                    command.Parameters.Add(seansTarihiParam);

                    // Seans saati için parametreyi ekle
                    MySqlParameter seansSaatiParam = new MySqlParameter("@seans_saati", MySqlDbType.Time);
                    command.Parameters.Add(seansSaatiParam);

                    Dictionary<string, (CheckBox checkBox, TimePicker timePicker)> gunlerVeSaatler = new Dictionary<string, (CheckBox, TimePicker)>
                    {
                        { "Pazartesi", (CheckBoxPazartesi, StartTimePazartesi) },
                        { "Salý", (CheckBoxSali, StartTimeSali) },
                        { "Çarþamba", (CheckBoxCarsamba, StartTimeCarsamba) },
                        { "Perþembe", (CheckBoxPersembe, StartTimePersembe) },
                        { "Cuma", (CheckBoxCuma, StartTimeCuma) },
                        { "Cumartesi", (CheckBoxCumartesi, StartTimeCumartesi) },
                        { "Pazar", (CheckBoxPazar, StartTimePazar) }
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

                        // Seçilen günü bulmak için baþlangýç tarihini ayarla
                        while (seansTarihi.DayOfWeek != hedefGun)
                        {
                            seansTarihi = seansTarihi.AddDays(1);

                            // Tarih sýnýrýný kontrol et
                            if (seansTarihi > endDate)
                            {
                                break;
                            }
                        }

                        // Belirlenen tarihleri bir aylýk süre içinde ekle
                        while (seansTarihi <= endDate)
                        {
                            if (seansTarihi > DateTime.MaxValue || seansTarihi < DateTime.MinValue)
                            {
                                throw new InvalidOperationException("Seans tarihi desteklenmeyen bir aralýkta."); // Hata at
                            }

                            seansDönemTarihler.Add(seansTarihi);
                            seansTarihi = seansTarihi.AddDays(7); // Haftalýk artýþ
                        }
                    }


                    await DisplayAlert("Baþarýlý", "Seanslar baþarýyla eklendi.", "Tamam");
                    await Navigation.PopAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabaný Hatasý", ex.Message + "\n" + ex.StackTrace, "Tamam");
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

                // Kullanýcýyý bulmak için sorgu
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
                        await DisplayAlert("Hata", "Kullanýcý bulunamadý.", "Tamam");
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

        // CollectionView'ý güncelle
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

                // SQL komutunu çalýþtýr
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Verileri listeye ekle
                        while (reader.Read())
                        {
                            string isim = reader["isim"].ToString();
                            string soyisim = reader["soyisim"].ToString();
                            string fullName = $"{isim} {soyisim}";  // Ýsim ve soyisim birleþiyor
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
            AND YEARWEEK(seanslar.tarih, 1) = YEARWEEK(CURDATE(), 1)
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
                        // Gün bazýnda satýr dizinlerini takip ediyoruz
                        var gunBazliSatirIndexleri = new Dictionary<string, int>
                    {
                        { "Pazartesi", 1 },
                        { "Salý", 1 },
                        { "Çarþamba", 1 },
                        { "Perþembe", 1 },
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
                            int rowIndex = gunBazliSatirIndexleri[gun]; // O günün mevcut satýr indeksini al

                            // Yeni satýr oluþturulmasýný saðla
                            LessonsListView.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                            // Saat ve hizmet türünü içeren bir StackLayout oluþtur
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

                            // Seansý tabloya ekle
                            Grid.SetColumn(seansLayout, columnIndex);
                            Grid.SetRow(seansLayout, rowIndex);
                            LessonsListView.Children.Add(seansLayout);

                            // Ýlgili gün için satýr indeksini güncelle
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
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
    }
    private int GetColumnIndex(string gun)
    {
        switch (gun)
        {
            case "Pazartesi": return 0;
            case "Salý": return 1;
            case "Çarþamba": return 2;
            case "Perþembe": return 3;
            case "Cuma": return 4;
            case "Cumartesi": return 5;
            case "Pazar": return 6;
            default: return -1;
        }
    }
    private bool isResettingTime = false;
    private async void OnTimeSelected(object sender, EventArgs e)
    {
        if (isResettingTime)
            return;

        var timePicker = sender as TimePicker;

        if (timePicker == null)
            return;

        string selectedDay = string.Empty;
        if (timePicker == StartTimePazartesi) selectedDay = "Pazartesi";
        else if (timePicker == StartTimeSali) selectedDay = "Salý";
        else if (timePicker == StartTimeCarsamba) selectedDay = "Çarþamba";
        else if (timePicker == StartTimePersembe) selectedDay = "Perþembe";
        else if (timePicker == StartTimeCuma) selectedDay = "Cuma";
        else if (timePicker == StartTimeCumartesi) selectedDay = "Cumartesi";
        else if (timePicker == StartTimePazar) selectedDay = "Pazar";

        string selectedTime = timePicker.Time.ToString(@"hh\:mm");

        DateTime today = DateTime.Today;
        DateTime targetDate = today;

        Dictionary<string, DayOfWeek> dayMap = new Dictionary<string, DayOfWeek>
    {
        { "Pazartesi", DayOfWeek.Monday },
        { "Salý", DayOfWeek.Tuesday },
        { "Çarþamba", DayOfWeek.Wednesday },
        { "Perþembe", DayOfWeek.Thursday },
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

        // Çakýþma kontrolü
        var isConflicting = DoluSeanslar.Any(doluSeans =>
            DateTime.Parse(doluSeans.Gun).ToString("yyyy-MM-dd") == selectedDateString &&
            TimeSpan.Parse(doluSeans.BaslangicSaat).ToString(@"hh\:mm") == selectedTime);

        if (isConflicting)
        {
            await DisplayAlert("Uyarý", $"{selectedDay} günü {selectedTime} saatinde bir ders zaten mevcut!", "Tamam");
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