using CommunityToolkit.Maui.Views;
using Microsoft.Maui.ApplicationModel;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Tls;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.ExceptionServices;

namespace Lotus_Spor;

public partial class LessonManagementPage : ContentPage
{
    private LoadingPopup _loadingPopup;
    List<string> isimListesi = new List<string>();
    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    List<string> isimListesi1 = new List<string>();
    List<string> isimListesi2 = new List<string>();
    private ObservableCollection<string> filteredList1 = new ObservableCollection<string>();
    public ObservableCollection<DoluSeans> DoluSeanslar { get; set; } = new ObservableCollection<DoluSeans>();
    public class DoluSeans
    {
        public string Gun { get; set; }
        public string BaslangicSaat { get; set; }
        public string HizmetTuru { get; set; }
    }
    string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);
    string loggedInUser2 = Preferences.Get("LoggedInUser2", string.Empty);
    string gender = Preferences.Get("Gender", string.Empty);
    int loggedInUserId = int.Parse(Preferences.Get("LoggedInUserId", "0"));
    string userRole = Preferences.Get("UserRole", string.Empty);
    private DateTime oldDate;
    private TimeSpan oldTime;
    string searchName, serviceType, antrenor, hafta;
    private int selectedDayOfWeek;
    public string grupders;
    private bool isResettingTime = false;
    private int firstRunning = 0;
    public ObservableCollection<Lesson> Lessons { get; set; } = new ObservableCollection<Lesson>();
    private int kullaniciId = -1;
    private int kullaniciId1 = -1;
    public LessonManagementPage()
	{
		InitializeComponent();
        HaftaListele();
        ResultsCollectionView.ItemsSource = filteredList;
        ResultsCollectionView2.ItemsSource = filteredList;
        ResultsCollectionView1.ItemsSource = filteredList1;
        kisilistele();
        BindingContext = this;
        kisilistele1();
        isimListesi2.Insert(0, "Tümü");
        AntrenorPicker.ItemsSource = isimListesi2;
        AntrenorPicker.SelectedIndex = AntrenorPicker.Items.IndexOf("Tümü");
        ServiceTypePicker.SelectedIndex = ServiceTypePicker.Items.IndexOf("Tümü");
        HaftaPicker.SelectedIndex = HaftaPicker.Items.IndexOf("Bu Hafta");
        hafta = HaftaPicker.SelectedItem?.ToString();
        LoadLessons();
    }
    private async void HaftaListele()
    {
        //var loadingPopup = new LoadingPopup();
        //this.ShowPopup(loadingPopup);

        try
        {
            var weeks = new List<string>();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            var culture = new CultureInfo("tr-TR");

            DateTime today = DateTime.Now;
            DateTime firstOfMonth = new DateTime(currentYear, currentMonth, 1);
            int dayOfMonth = (today - firstOfMonth).Days;
            int currentWeek = (dayOfMonth / 7) + 1;

            for (int i = 0; i < 12; i++)
            {
                string monthName = new DateTime(currentYear, i + 1, 1).ToString("MMMM", culture);

                for (int j = 1; j <= 5; j++)
                {
                    string weekLabel = $"{monthName} {j}.Hafta";

                    if (currentMonth == (i + 1) && currentWeek == j)
                    {
                        weeks.Add("Bu Hafta");
                    }
                    else if (currentMonth == (i + 1) && currentWeek + 1 == j)
                    {
                        weeks.Add("Sonraki Hafta");
                    }
                    else
                    {
                        weeks.Add(weekLabel);
                    }
                }
            }

            // **Picker'a haftalarý ekleme**
            HaftaPicker.ItemsSource = weeks;
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Bir hata oluþtu: {ex.Message}", "Tamam");
        }
        finally
        {
            //loadingPopup.Close();
        }
    }
    private async void kisilistele()
    {
        try
        {
            var connectionString = Database.GetConnection();
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string query = "SELECT isim, soyisim FROM musteriler";

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
    private async void kisilistele1()
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
                            isimListesi1.Add(fullName);
                            isimListesi2.Add(fullName);
                            isimListesi1.Remove(ConnectionString.admin2);
                            isimListesi2.Remove(ConnectionString.admin2);
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
    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            ResultsCollectionView.IsVisible = false;
            btnClear.IsVisible = false;
            filteredList.Clear();
            return;
        }
        else if(!string.IsNullOrWhiteSpace(searchText))
        {
            btnClear.IsVisible = true;
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

        searchName = SearchEntry.Text;
        serviceType = ServiceTypePicker.SelectedItem?.ToString();
        antrenor = AntrenorPicker.SelectedItem?.ToString();
        hafta = HaftaPicker.SelectedItem?.ToString();

        //LoadLessons(searchName, serviceType, antrenor, hafta);
    }
    private async void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            ResultsCollectionView2.IsVisible = false;
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

        ResultsCollectionView2.IsVisible = filteredList.Count > 0;
    }
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            string selectedName = e.CurrentSelection[0] as string;

            if (!string.IsNullOrEmpty(selectedName))
            {
                SearchEntry.Text = selectedName;  // Seçilen ismi Entry'ye yaz
                ResultsCollectionView.IsVisible = false;  // Önerileri gizle
                GetKullaniciId(selectedName);
                ResultsCollectionView.SelectedItem = string.Empty;

                searchName = SearchEntry.Text;
                serviceType = ServiceTypePicker.SelectedItem?.ToString();
                antrenor = AntrenorPicker.SelectedItem?.ToString();
                hafta = HaftaPicker.SelectedItem?.ToString();

                LoadLessons(searchName, serviceType, antrenor, hafta);
            }
        }
    }
    private async void OnSelectionChanged1(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            string selectedName = e.CurrentSelection[0] as string;

            if (!string.IsNullOrEmpty(selectedName))
            {
                NameEntry.Text = selectedName;  // Seçilen ismi Entry'ye yaz
                ResultsCollectionView2.IsVisible = false;  // Önerileri gizle
                GetKullaniciId(selectedName);
                ResultsCollectionView2.SelectedItem = string.Empty;
            }
        }
    }
    private async void OnSelectionChanged2(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            string selectedName = e.CurrentSelection[0] as string;

            if (!string.IsNullOrEmpty(selectedName))
            {
                AntrenorNameEntry.Text = selectedName;  // Seçilen ismi Entry'ye yaz
                ResultsCollectionView1.IsVisible = false;  // Önerileri gizle
                //GetKullaniciId(selectedName);
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

                // Soyisim: Dizinin son elemaný
                string soyisim = nameParts.Length > 0 ? nameParts[^1] : "";

                // Ýsim: Soyisim hariç kalan kýsým
                string isim = nameParts.Length > 1 ? string.Join(" ", nameParts[..^1]) : "";

                // Kullanýcýyý bulmak için sorgu
                string query = "SELECT id FROM musteriler WHERE isim = @isim AND soyisim = @soyisim";

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
    private void OnDayPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        // Picker'dan seçilen günün indeksine göre günün numarasýný ayarla
        switch (DayPicker.SelectedIndex)
        {
            case 0:
                selectedDayOfWeek = 7; // Pazar
                break;
            case 1:
                selectedDayOfWeek = 1; // Pazartesi
                break;
            case 2:
                selectedDayOfWeek = 2; // Salý
                break;
            case 3:
                selectedDayOfWeek = 3; // Çarþamba
                break;
            case 4:
                selectedDayOfWeek = 4; // Perþembe
                break;
            case 5:
                selectedDayOfWeek = 5; // Cuma
                break;
            case 6:
                selectedDayOfWeek = 6; // Cumartesi
                break;
        }
    }
    private async void OnAntrenornameTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            ResultsCollectionView1.IsVisible = false;
            filteredList1.Clear();
            return;
        }

        // Listeyi filtrele
        var suggestions = isimListesi1
            .Where(isimSoyisim => isimSoyisim.ToLower().Contains(searchText))
            .ToList();

        // CollectionView'ý güncelle
        filteredList1.Clear();
        foreach (var suggestion in suggestions)
        {
            filteredList1.Add(suggestion);
        }

        ResultsCollectionView1.IsVisible = filteredList1.Count > 0;
    }
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
    private async void OnServiceTypeChanged(object sender, EventArgs e)
    {
        FiltreUygula();
    }
    private async void OnAntrenorChanged(object sender, EventArgs e)
    {
        FiltreUygula();
    }
    private async void OnHaftaChanged(object sender, EventArgs e)
    {
        FiltreUygula();
    }
    private async void FiltreUygula()
    {
        if (firstRunning > 2)
        {
            firstRunning++;
            searchName = SearchEntry.Text;
            serviceType = ServiceTypePicker.SelectedItem?.ToString();
            antrenor = AntrenorPicker.SelectedItem?.ToString();
            hafta = HaftaPicker.SelectedItem?.ToString();

            LoadLessons(searchName, serviceType, antrenor, hafta);
        }
        if (firstRunning <= 2)
        {
            firstRunning++;
        }
    }
    private async void OnBiriniSilClicked(object sender, EventArgs e)
    {
        string query = "DELETE FROM seanslar WHERE musteri_id = @kullaniciID AND tarih = @selectedDate AND saat = @selectedTime";
        string fullName = Client.Text; // Seçilen müþteri adlarý
        string[] clientNameList = fullName.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Satýrlara göre ayýr

        try
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@selectedDate", oldDate); // Seçilen tarih parametresi
                    cmd.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm")); // Seçilen saat parametresi

                    // Her müþteri için sorguyu çalýþtýr
                    foreach (var name in clientNameList)
                    {
                        GetKullaniciId(name.Trim());
                        int musteriID = kullaniciId;

                        cmd.Parameters.AddWithValue("@kullaniciID", musteriID); // Her müþteri için ID parametresi ekle

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Baþarýlý", $"{name.Trim()} için ders kaydý baþarýyla silindi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name.Trim()} için dersler silinemedi. Lütfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreleri sýfýrla, bir sonraki müþteri için yeniden ayarlamak için
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@selectedDate", oldDate); // Eski tarih parametresini tekrar ekle
                        cmd.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler silinirken bir hata oluþtu: {ex.Message}", "Tamam");
        }

        Sifirla();

    }
    private async void OnKayitSilClicked(object sender, EventArgs e)
    {
        string query = "DELETE FROM seanslar WHERE musteri_id = @kullaniciID AND tarih >= @selectedDate AND saat = @selectedTime AND DATEDIFF(tarih, @selectedDate) % 7 = 0;";
        string fullName = Client.Text; // Seçilen müþteri adlarý
        string[] clientNameList = fullName.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Satýrlara göre ayýr

        try
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@selectedDate", oldDate); // Seçilen tarih parametresi
                    cmd.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm")); // Seçilen saat parametresi

                    // Her müþteri için sorguyu çalýþtýr
                    foreach (var name in clientNameList)
                    {
                        GetKullaniciId(name.Trim());
                        int musteriID = kullaniciId; // Müþteri ID'sini almak için fonksiyonu çaðýr

                        cmd.Parameters.AddWithValue("@kullaniciID", musteriID); // Her müþteri için ID parametresi ekle

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Baþarýlý", $"{name.Trim()} için ders kaydý baþarýyla silindi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name.Trim()} için dersler silinemedi. Lütfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreleri sýfýrla, bir sonraki müþteri için yeniden ayarlamak için
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@selectedDate", oldDate); // Eski tarih parametresini tekrar ekle
                        cmd.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler silinirken bir hata oluþtu: {ex.Message}", "Tamam");
        }

        Sifirla();

    }
    private async void OnCurDateChangeClicked(object sender, EventArgs e)
    {
        string clientNames = Client.Text; // Seçilen müþteri adlarý
        string[] clientNameList = clientNames.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Adlarý ayýrmak için satýrlara göre bölelim

        DateTime currentDate = DateTime.Now.Date; // Bugünün tarihi (saat kýsmý hariç)

        DateTime newDate = SeansDate.Date; // Seçilen yeni tarih
        TimeSpan newTime = SeansTime.Time; // Seçilen yeni saat

        // SQL sorgusunu oluþtur
        string query = @"
        UPDATE seanslar s
        JOIN musteriler m ON s.musteri_id = m.id
        SET s.tarih = @newDate, s.saat = @newTime
        WHERE CONCAT(m.isim, ' ', m.soyisim) = @clientName
        AND s.tarih = @selectedDate;";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@selectedDate", oldDate); // Seçilen eski tarih parametresi
                    command.Parameters.AddWithValue("@newDate", newDate); // Yeni tarih parametresi
                    command.Parameters.AddWithValue("@newTime", newTime.ToString(@"hh\:mm")); // Yeni saat parametresi

                    // Her müþteri için sorguyu çalýþtýr
                    foreach (var name in clientNameList)
                    {
                        command.Parameters.AddWithValue("@clientName", name.Trim()); // Her müþteri adý için parametre ekle (Trim ile boþluklarý temizle)

                        // Sorguyu çalýþtýr
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Baþarýlý", $"{name} için dersin tarihi ve saati baþarýyla güncellendi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name} için ders bulunamadý veya güncellenemedi. Lütfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreyi sýfýrla, bir sonraki müþteri için yeniden ayarlamak için
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@selectedDate", oldDate); // Tekrar eski tarihi ekle
                        command.Parameters.AddWithValue("@newDate", newDate);
                        command.Parameters.AddWithValue("@newTime", newTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }

        Sifirla();

    }
    private async void OnAllChangeClicked(object sender, EventArgs e)
    {
        string clientNames = Client.Text; // Seçilen müþteri adlarý
        string[] clientNameList = clientNames.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Adlarý ayýrmak için satýrlara göre bölelim

        DateTime newDate = SeansDate.Date; // Seçilen yeni tarih
        TimeSpan newTime = SeansTime.Time; // Seçilen yeni saat

        // SQL sorgusunu oluþtur
        string query = @"
               UPDATE seanslar s
               JOIN musteriler m ON s.musteri_id = m.id
               SET s.tarih = DATE_ADD(@newDate, INTERVAL FLOOR(DATEDIFF(s.tarih, @selectedDate) / 7) WEEK),
                   s.saat = @newTime
               WHERE CONCAT(m.isim, ' ', m.soyisim) = @clientName
                 AND s.tarih >= @selectedDate -- Sadece baþlangýç tarihi ve sonrasý
                 AND s.saat = @selectedTime
                 AND WEEKDAY(s.tarih) = WEEKDAY(@selectedDate);";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@newDate", newDate); // Yeni tarih parametresi
                    command.Parameters.AddWithValue("@selectedDate", oldDate); // Seçilen eski tarih
                    command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm")); // Seçilen eski saat parametresi
                    command.Parameters.AddWithValue("@newTime", newTime.ToString(@"hh\:mm")); // Yeni saat parametresi

                    // Her müþteri için sorguyu çalýþtýr
                    foreach (var name in clientNameList)
                    {
                        command.Parameters.AddWithValue("@clientName", name.Trim()); // Trim ile gereksiz boþluklardan kurtuluyoruz

                        // Sorguyu çalýþtýr
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Baþarýlý", $"Ýleri tarihli derslerinin tarihi ve saati {name} için baþarýyla güncellendi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name} için ileri tarihli ders bulunamadý veya güncellenemedi. Lütfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreyi sýfýrla, bir sonraki müþteri için yeniden ayarlamak için
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@newDate", newDate);
                        command.Parameters.AddWithValue("@selectedDate", oldDate);
                        command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                        command.Parameters.AddWithValue("@newTime", newTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
        Sifirla();
    }
    private async void OnDoneClicked(object sender, EventArgs e)
    {
        string clientNames = Client.Text; // Seçilen müþteri adlarý
        string[] clientNameList = clientNames.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Adlarý ayýrmak için satýrlara göre bölelim

        // SQL sorgusunu oluþtur
        string query = @"
        UPDATE seanslar s
        JOIN musteriler m ON s.musteri_id = m.id
        SET s.durum = 'Yapýldý'
        WHERE CONCAT(m.isim, ' ', m.soyisim) = @clientName
        AND s.tarih = @selectedDate
        AND s.saat = @selectedTime;";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@selectedDate", oldDate);
                    command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));

                    // Her müþteri için sorguyu çalýþtýr
                    foreach (var name in clientNameList)
                    {
                        command.Parameters.AddWithValue("@clientName", name);

                        // Sorguyu çalýþtýr
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Baþarýlý", $"Seans durumu 'Yapýldý' olarak {name} için güncellendi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name} için seans güncellenemedi. Lütfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreyi sýfýrla, bir sonraki müþteri için yeniden ayarlamak için
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@selectedDate", oldDate);
                        command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
        Sifirla();

    }
    private async void OnUnDoneClicked(object sender, EventArgs e)
    {
        string name = Client.Text; // Seçilen müþteri adlarýný al
        string[] clientNameList = name.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Satýrlara göre ayýr

        // SQL sorgusunu oluþtur
        string query = @"
                        UPDATE seanslar s
                        JOIN musteriler m ON s.musteri_id = m.id
                        SET s.durum = 'Yapýlmadý'
                        WHERE CONCAT(m.isim, ' ', m.soyisim) = @clientName
                        AND s.tarih = @selectedDate
                        AND s.saat = @selectedTime;";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@selectedDate", oldDate);
                    command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));

                    // Her müþteri için sorguyu çalýþtýr
                    foreach (var clientName in clientNameList)
                    {
                        command.Parameters.AddWithValue("@clientName", clientName.Trim()); // Her müþteri için isim parametresini ekle

                        // Sorguyu çalýþtýr
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Baþarýlý", $"{clientName.Trim()} için seans durumu 'Yapýlmadý' olarak güncellendi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{clientName.Trim()} için seans güncellenemedi. Lütfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreleri sýfýrla, bir sonraki müþteri için yeniden ayarlamak için
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@selectedDate", oldDate); // Eski tarih parametresini tekrar ekle
                        command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }

        Sifirla();

    }
    private async void OnAddLessonClicked(object sender, EventArgs e)
    {
        DersEkle.IsVisible = true;
        btnDersEkle.IsVisible = false;
    }
    private async void OnIptalClicked(object sender, EventArgs e)
    {
        DersEkle.IsVisible = false;
        btnDersEkle.IsVisible = true;
        Sifirla();
    }
    private async void OnEkleClicked(object sender, EventArgs e)
    {
        string antrenorName = AntrenorNameEntry.Text;
        string seansTur = SeansPicker.SelectedItem?.ToString();

        // Seçilen günleri birleþtirme
        List<string> secilenGunler = new List<string>();
        if (CheckBoxPazartesi.IsChecked) secilenGunler.Add("Pazartesi");
        if (CheckBoxSali.IsChecked) secilenGunler.Add("Salý");
        if (CheckBoxCarsamba.IsChecked) secilenGunler.Add("Çarþamba");
        if (CheckBoxPersembe.IsChecked) secilenGunler.Add("Perþembe");
        if (CheckBoxCuma.IsChecked) secilenGunler.Add("Cuma");
        if (CheckBoxCumartesi.IsChecked) secilenGunler.Add("Cumartesi");
        if (CheckBoxPazar.IsChecked) secilenGunler.Add("Pazar");
        if (CheckBoxGrup.IsChecked) grupders = "evet";

        string seansGunleri = string.Join(", ", secilenGunler);

        string seansquery = "INSERT INTO seanslar (tur, musteri_id, antrenor, tarih, saat, grup) VALUES (@seans_turu, @musteri_id, @antrenor, @seans_tarihi, @seans_saati, @grup)";

        DateTime selectedDate = DatePickerKayitTarihi.Date;

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

        Dictionary<string, TimeSpan> seansSaatleri = new Dictionary<string, TimeSpan>();
        List<DateTime> seansDönemTarihler = new List<DateTime>();
        //DateTime today = DateTime.Today;
        DateTime today;
        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                // Müþteri kayýt tarihini alacak sorgu
                string kayitTarihiQuery = "SELECT kayit_tarihi FROM musteriler WHERE id = @musteri_id LIMIT 1";
                string updateQuery = "UPDATE musteriler SET seans_gunleri = @seans_gunleri WHERE id = @id";

                using (MySqlCommand command = new MySqlCommand(kayitTarihiQuery, connection))
                {
                    command.Parameters.AddWithValue("@musteri_id", kullaniciId); // Kullanýcý ID'si

                    object result = await command.ExecuteScalarAsync();
                    if (result != null && DateTime.TryParse(result.ToString(), out DateTime kayitTarihi))
                    {
                        today = kayitTarihi; // Veritabanýndan alýnan kayýt tarihi
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Seçilen müþteri için kayýt tarihi bulunamadý.", "Tamam");
                        return;
                    }
                }
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", kullaniciId);
                    command.Parameters.AddWithValue("@seans_gunleri", seansGunleri);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected <= 0)
                    {
                        await DisplayAlert("Hata", "Günler güncellenirken bir sorun oluþtu.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabaný Hatasý", ex.Message, "Tamam");
            return;
        }
        DateTime endDate = today.AddMonths(12);

        foreach (string gun in secilenGunler)
        {
            switch (gun)
            {
                case "Pazartesi":
                    seansSaatleri["Pazartesi"] = StartTimePazartesi.Time;
                    break;
                case "Salý":
                    seansSaatleri["Salý"] = StartTimeSali.Time;
                    break;
                case "Çarþamba":
                    seansSaatleri["Çarþamba"] = StartTimeCarsamba.Time;
                    break;
                case "Perþembe":
                    seansSaatleri["Perþembe"] = StartTimePersembe.Time;
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

            // Tarihleri bir aylýk süre içinde ekleme
            while (seansTarihi < endDate)
            {
                seansDönemTarihler.Add(seansTarihi);
                seansTarihi = seansTarihi.AddDays(7); // Haftalýk artýþ
            }
        }
        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                string fullName = NameEntry.Text;

                GetKullaniciId(fullName);

                int musteriId = kullaniciId;

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
                            DayOfWeek.Tuesday => "Salý",
                            DayOfWeek.Wednesday => "Çarþamba",
                            DayOfWeek.Thursday => "Perþembe",
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
                            await DisplayAlert("Hata", "Seans eklenirken bir sorun oluþtu.", "Tamam");
                        }
                    }

                    await DisplayAlert("Baþarýlý", "Seanslar baþarýyla eklendi.", "Tamam");
                    DersEkle.IsVisible = false;

                    searchName = SearchEntry.Text;
                    serviceType = ServiceTypePicker.SelectedItem?.ToString();
                    antrenor = AntrenorPicker.SelectedItem?.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabaný Hatasý", ex.Message, "Tamam");
        }
        Sifirla();
    }
    private async void OnTekEkleClicked(object sender, EventArgs e)
    {
        string antrenorName = AntrenorNameEntry.Text;
        string seansTur = SeansPicker.SelectedItem?.ToString();

        string seansquery = "INSERT INTO seanslar (tur, musteri_id, antrenor, tarih, saat, grup) VALUES (@seans_turu, @musteri_id, @antrenor, @seans_tarihi, @seans_saati, @grup)";

        // Seçilen tarih ve saat
        DateTime selectedDate = DatePickerKayitTarihi.Date;
        TimeSpan selectedTime = TimePickerTek.Time;

        if (AntrenorNameEntry.Text == string.Empty || NameEntry.Text == string.Empty)
        {
            await DisplayAlert("Uyarý", "Lütfen boþ kýsýmlarý doldurunuz.", "Tamam");
        }

        // Gruplar
        string grupders = null;
        if (CheckBoxGrup.IsChecked)
        {
            grupders = "evet";
        }

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                string fullName = NameEntry.Text;
                GetKullaniciId(fullName);
                int musteriId = kullaniciId;

                using (MySqlCommand command = new MySqlCommand(seansquery, connection))
                {
                    command.Parameters.AddWithValue("@seans_turu", seansTur);
                    command.Parameters.AddWithValue("@musteri_id", musteriId);
                    command.Parameters.AddWithValue("@antrenor", antrenorName);
                    command.Parameters.AddWithValue("@grup", grupders ?? null);

                    // Parametreleri ekliyoruz
                    MySqlParameter seansTarihiParam = new MySqlParameter("@seans_tarihi", MySqlDbType.Date);
                    MySqlParameter seansSaatiParam = new MySqlParameter("@seans_saati", MySqlDbType.VarChar);
                    command.Parameters.Add(seansTarihiParam);
                    command.Parameters.Add(seansSaatiParam);

                    // Tek bir tarih ve saat ekleniyor
                    seansTarihiParam.Value = selectedDate.ToString("yyyy-MM-dd");
                    seansSaatiParam.Value = selectedTime.ToString(@"hh\:mm");

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Baþarýlý", "Seans baþarýyla eklendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Seans eklenirken bir sorun oluþtu.", "Tamam");
                    }
                }

                // Seans ekleme baþarýlý ise seanslarý tekrar yükleyebilirsiniz
                DersEkle.IsVisible = false;

                // Arama kriterlerini alýp dersleri tekrar yükleyebilirsiniz
                searchName = SearchEntry.Text;
                serviceType = ServiceTypePicker.SelectedItem?.ToString();
                antrenor = AntrenorPicker.SelectedItem?.ToString();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabaný Hatasý", ex.Message, "Tamam");
        }
        Sifirla();
    }
    private async void Sifirla()
    {
        DersDetaylar.IsVisible = false;
        DersEkle.IsVisible = false;
        btnDersEkle.IsVisible = true;
        Client.Text = string.Empty;
        LessonType.Text = string.Empty;
        SeansDate.Date = DateTime.Now;
        SeansTime.Time = TimeSpan.Zero;
        CheckBoxPazartesi.IsChecked = false;
        CheckBoxSali.IsChecked = false;
        CheckBoxCarsamba.IsChecked = false;
        CheckBoxPersembe.IsChecked = false;
        CheckBoxCuma.IsChecked = false;
        CheckBoxCumartesi.IsChecked = false;
        CheckBoxPazar.IsChecked = false;
        StartTimePazartesi.Time = TimeSpan.Zero;
        StartTimeSali.Time = TimeSpan.Zero;
        StartTimeCarsamba.Time = TimeSpan.Zero;
        StartTimePersembe.Time = TimeSpan.Zero;
        StartTimeCuma.Time = TimeSpan.Zero;
        StartTimeCumartesi.Time = TimeSpan.Zero;
        StartTimePazar.Time = TimeSpan.Zero;
        NameEntry.Text = string.Empty;
        AntrenorNameEntry.Text = string.Empty;
        LoadLessons(searchName, serviceType, antrenor);
    }
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        DersDetaylar.IsVisible = false;
        DersEkle.IsVisible = false;
        btnDersEkle.IsVisible = true;
        Client.Text = string.Empty;   
        //lessontype.Text = string.Empty;
        SeansDate.Date = DateTime.Now;
        SeansTime.Time = TimeSpan.Zero;
        //LoadLessons(searchName, serviceType, antrenor);
    }
    private void OnScrollChanged(object sender, ScrolledEventArgs e)
    {
        
    }
    private int loadLessonsCallCount = 0;
    private async void LoadLessons(string searchName = "", string hizmet_turu = "", string antrenor = "", string Hafta = "")
    {
        loadLessonsCallCount++;
        Console.WriteLine($"LoadLessons çaðrýldý: {loadLessonsCallCount} kez");
        var loadingPopup = new LoadingPopup();
        this.ShowPopup(loadingPopup);

        try
        {
            if (HaftaPicker.SelectedIndex == -1) return;
            Hafta = hafta;

            string query = @"
        SELECT 
            s.tarih AS Day, 
            s.saat AS Time, 
            s.durum AS Status, 
            s.tur AS Type,
            s.grup AS Grup,
            CONCAT(m.isim, ' ', m.soyisim) AS Client 
        FROM 
            seanslar s
        INNER JOIN 
            musteriler m ON s.musteri_id = m.id
        WHERE 
            m.aktiflik = 'Aktif' ";

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            if (string.IsNullOrEmpty(hafta))
            {
                hafta = "Bu Hafta";
            }
            if (hafta == "Bu Hafta")
            {
                startDate = StartOfWeek(DateTime.Now);
                endDate = StartOfWeek(DateTime.Now).AddDays(6);
            }
            if (hafta == "Sonraki Hafta")
            {
                DateTime currentDate = DateTime.Now;
                DateTime startOfWeek = StartOfWeek(currentDate).AddDays(7);
                DateTime endOfWeek = startOfWeek.AddDays(6);
                startDate = startOfWeek;
                endDate = endOfWeek;
            }
            else
            {
                string[] parcalar = hafta.Split(' ');
                if (parcalar.Length == 2)
                {
                    string ay = parcalar[0];  // "Þubat"
                    string haftaNoStr = parcalar[1].Replace(".Hafta", ""); // "2"

                    if (int.TryParse(haftaNoStr, out int haftaNo))
                    {
                        // Ay ismini sayýya dönüþtür
                        int month = DateTime.ParseExact(ay, "MMMM", new CultureInfo("tr-TR")).Month;

                        // Ýlgili ayýn ve haftanýn tarih aralýðýný belirle
                        startDate = GetStartOfWeekInMonth(month, haftaNo);
                        endDate = startDate.AddDays(6);
                    }
                }
            }

            // Tarih aralýðýný sorguya ekle
            query += " AND s.tarih BETWEEN @startDate AND @endDate";

            if (!string.IsNullOrEmpty(searchName))
            {
                query += " AND CONCAT(m.isim, ' ', m.soyisim) LIKE @searchName";
            }
            if (AntrenorPicker.SelectedIndex != -1)
            {
                string selectedAntrenor = AntrenorPicker.SelectedItem.ToString();
                if (selectedAntrenor != "Tümü")
                {
                    query += " AND s.antrenor = @antrenor";
                }
            }
            if (ServiceTypePicker.SelectedIndex != -1)
            {
                string selectedServiceType = ServiceTypePicker.SelectedItem.ToString();
                if (selectedServiceType != "Tümü")
                {
                    query += " AND s.tur = @tur";
                }
            }

            query += " ORDER BY s.tarih ASC, s.saat ASC";

            try
            {
                using (MySqlConnection connection = Database.GetConnection())
                {
                    await connection.OpenAsync();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@tur", hizmet_turu);
                        command.Parameters.AddWithValue("@antrenor", antrenor);

                        // Parametreleri ekle
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);

                        lblTarihBilgi.Text = string.Empty;
                        lblTarihBilgi.Text = "Gösterilen Tarih Aralýðý \n" + startDate.ToString("dd-MM-yyyy") + " - " + endDate.ToString("dd-MM-yyyy");

                        if (!string.IsNullOrEmpty(searchName))
                        {
                            command.Parameters.AddWithValue("@searchName", "%" + searchName + "%");
                        }

                        List<Tuple<int, int>> rowHeights = new List<Tuple<int, int>>();

                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            // Gün ve saat bazýnda verileri saklamak için dictionary
                            var lessonsByDayAndTime = new Dictionary<string, Dictionary<string, List<(string client, string status, DateTime date, TimeSpan time, string type)>>>();
                            var saatler = new HashSet<string>();
                            var gunler = new HashSet<string>();

                            while (await reader.ReadAsync())
                            {
                                string day = DateTime.Parse(reader["Day"].ToString()).ToString("dddd", new CultureInfo("tr-TR"));
                                string time = reader["Time"].ToString();
                                string clientInfo = $"{reader["Client"]}";
                                string status = reader["Status"].ToString();
                                string lessontype = reader["Type"].ToString();

                                DateTime parsedDate = DateTime.Parse(reader["Day"].ToString());
                                TimeSpan parsedTime = TimeSpan.Parse(reader["Time"].ToString());

                                gunler.Add(day);
                                saatler.Add(time);

                                if (!lessonsByDayAndTime.ContainsKey(day))
                                    lessonsByDayAndTime[day] = new Dictionary<string, List<(string client, string status, DateTime, TimeSpan, string type)>>();

                                if (!lessonsByDayAndTime[day].ContainsKey(time))
                                    lessonsByDayAndTime[day][time] = new List<(string client, string status, DateTime date, TimeSpan time, string type)>();

                                lessonsByDayAndTime[day][time].Add((clientInfo, status, parsedDate, parsedTime, lessontype));
                            }

                            // Gün sýralamasý
                            var gunSirasi = new List<string> { "Pazartesi", "Salý", "Çarþamba", "Perþembe", "Cuma", "Cumartesi", "Pazar" };
                            var sortedGunler = gunSirasi.Where(gun => gunler.Contains(gun)).ToList();

                            // Saatleri sýralama
                            var sortedSaatler = saatler.OrderBy(s => s).ToList();

                            int rowHeight = 90;

                            // 1. Üstte saatlerin olduðu grid'i oluþturuyoruz.
                            HoursHeaderGrid.Children.Clear();
                            HoursHeaderGrid.RowDefinitions.Clear();
                            HoursHeaderGrid.ColumnDefinitions.Clear();
                            HoursHeaderGrid.Children.Clear();

                            DaysHeaderGrid.Children.Clear();
                            DaysHeaderGrid.RowDefinitions.Clear();
                            DaysHeaderGrid.ColumnDefinitions.Clear();
                            DaysHeaderGrid.Children.Clear();

                            // Saat baþlýklarýný ekliyoruz
                            HoursHeaderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                            var boslabel = new Label
                            {
                                Text = string.Empty
                            };
                            HoursHeaderGrid.Children.Add(boslabel);

                            // Grid yapýlandýrmasý
                            LessonsListView.RowDefinitions.Clear();
                            LessonsListView.ColumnDefinitions.Clear();
                            LessonsListView.Children.Clear();

                            // Ýlk satýrda gün baþlýklarýný ekle
                            LessonsListView.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                            LessonsListView.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                            // Gün baþlýklarýný ekle
                            for (int i = 0; i < sortedGunler.Count; i++)
                            {
                                DaysHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                                var label = new Label
                                {
                                    Text = sortedGunler[i],
                                    HorizontalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Bold
                                };
                                Grid.SetRow(label, 0); // Baþlýk satýrý
                                Grid.SetColumn(label, i); // Gün sütunlarý
                                DaysHeaderGrid.Children.Add(label);

                                LessonsListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                                var label1 = new Label
                                {
                                    Text = sortedGunler[i],
                                    HorizontalOptions = LayoutOptions.Center,
                                    VerticalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Bold
                                };
                                Grid.SetRow(label1, 0); // Baþlýk satýrý
                                Grid.SetColumn(label1, i + 1); // Gün sütunlarý
                                LessonsListView.Children.Add(label1);
                            }

                            // Saatleri ve seanslarý ekle
                            for (int rowIndex = 0; rowIndex < sortedSaatler.Count; rowIndex++)
                            {
                                LessonsListView.RowDefinitions.Add(new RowDefinition { Height = rowHeight });

                                if (rowIndex < sortedSaatler.Count + 2)
                                {
                                    var horizontalLine = new BoxView
                                    {
                                        Color = Colors.Gray,
                                        HeightRequest = 1,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        VerticalOptions = LayoutOptions.Start
                                    };
                                    Grid.SetRow(horizontalLine, rowIndex + 1); // Her satýr baþlýðýnýn altýna
                                    Grid.SetColumn(horizontalLine, 0);
                                    Grid.SetColumnSpan(horizontalLine, sortedGunler.Count + 1); // Tüm sütunlarda geçerli
                                    LessonsListView.Children.Add(horizontalLine);

                                    var horizontalLine1 = new BoxView
                                    {
                                        Color = Colors.Gray,
                                        HeightRequest = 1,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        VerticalOptions = LayoutOptions.Start
                                    };
                                    Grid.SetRow(horizontalLine1, rowIndex + 1); // Her satýr baþlýðýnýn altýna
                                    Grid.SetColumn(horizontalLine1, 0);
                                    Grid.SetColumnSpan(horizontalLine1, sortedGunler.Count + 1); // Tüm sütunlarda geçerli
                                    HoursHeaderGrid.Children.Add(horizontalLine1);
                                }

                                // Günlere göre seanslarý ekle
                                for (int colIndex = 0; colIndex < sortedGunler.Count; colIndex++)
                                {
                                    string day = sortedGunler[colIndex];
                                    string time = sortedSaatler[rowIndex];

                                    var clientDataList = lessonsByDayAndTime.ContainsKey(day) && lessonsByDayAndTime[day].ContainsKey(time)
                                        ? lessonsByDayAndTime[day][time]
                                        : new List<(string client, string status, DateTime date, TimeSpan time, string type)>();


                                    var currentTheme = Application.Current.RequestedTheme;

                                    var labelContainer = new StackLayout
                                    {
                                        Orientation = StackOrientation.Vertical,
                                        Spacing = 1
                                    };

                                    foreach (var clientData in clientDataList)
                                    {
                                        string clientName = clientData.client;

                                        // Renk seçimi duruma göre yapýlýyor
                                        Color labelColor;
                                        switch (clientData.status)
                                        {
                                            case "Yapýldý":
                                                labelColor = (currentTheme == AppTheme.Dark) ? Colors.LightGreen : Colors.DarkGreen;
                                                break;
                                            case "Yapýlmadý":
                                                labelColor = (currentTheme == AppTheme.Dark) ? Colors.LightCoral : Colors.DarkRed;
                                                break;
                                            case "beklemede":
                                                labelColor = (currentTheme == AppTheme.Dark) ? Colors.Yellow : Colors.OrangeRed;
                                                break;
                                            default:
                                                labelColor = Colors.Grey;
                                                break;
                                        }

                                        // Her müþteri için ayrý bir Label oluþturuluyor
                                        var clientLabel = new Label
                                        {
                                            Text = clientName,
                                            FontSize = 16,
                                            HorizontalOptions = LayoutOptions.Center,
                                            VerticalOptions = LayoutOptions.Center,
                                            TextColor = labelColor
                                        };

                                        var tapGestureRecognizer = new TapGestureRecognizer();
                                        tapGestureRecognizer.Tapped += async (s, e) =>
                                        {
                                            var tappedLabel = (Label)s;
                                            string clientInfo = tappedLabel.Text;
                                            if (!string.IsNullOrEmpty(clientInfo))
                                            {
                                                DersDetaylar.IsVisible = true;
                                                btnDersEkle.IsVisible = false;
                                                DersEkle.IsVisible = false;

                                                Client.Text = clientInfo;

                                                var selectedLesson = clientDataList.FirstOrDefault(cd => cd.client == clientInfo);

                                                LessonType.Text = selectedLesson.type;

                                                SeansDate.Date = selectedLesson.date; // Günün tarihi
                                                SeansTime.Time = selectedLesson.time;
                                                oldDate = selectedLesson.date;
                                                oldTime = selectedLesson.time;

                                                await Task.Delay(100); // UI güncellenmesi için kýsa gecikme
                                                await ScrollViewMain.ScrollToAsync(0, DersDetaylar.Y, true);
                                            }
                                        };


                                        clientLabel.GestureRecognizers.Add(tapGestureRecognizer);

                                        // Label, StackLayout içine ekleniyor
                                        labelContainer.Children.Add(clientLabel);
                                    }

                                    // StackLayout, grid içine ekleniyor
                                    Grid.SetRow(labelContainer, rowIndex + 1);
                                    Grid.SetColumn(labelContainer, colIndex + 1);
                                    LessonsListView.Children.Add(labelContainer);

                                    if (colIndex < sortedGunler.Count)
                                    {
                                        var verticalLine = new BoxView
                                        {
                                            Color = Colors.Gray,
                                            WidthRequest = 1,
                                            VerticalOptions = LayoutOptions.FillAndExpand,
                                            HorizontalOptions = LayoutOptions.Start,
                                        };
                                        Grid.SetRow(verticalLine, rowIndex + 1);
                                        Grid.SetColumn(verticalLine, colIndex + 1);
                                        Grid.SetRowSpan(verticalLine, 0);
                                        LessonsListView.Children.Add(verticalLine);

                                        var verticalLine1 = new BoxView
                                        {
                                            Color = Colors.Gray,
                                            WidthRequest = 1,
                                            VerticalOptions = LayoutOptions.FillAndExpand,
                                            HorizontalOptions = LayoutOptions.Start,
                                        };
                                        Grid.SetRow(verticalLine1, 0);
                                        Grid.SetColumn(verticalLine1, colIndex + 1);
                                        Grid.SetRowSpan(verticalLine1, 0);
                                        LessonsListView.Children.Add(verticalLine1);
                                    }
                                }

                                HoursHeaderGrid.RowDefinitions.Add(new RowDefinition { Height = rowHeight });
                                string saatindex = string.Join("\n", sortedSaatler[rowIndex]);

                                var timeLabel1 = new Label
                                {
                                    Text = saatindex,
                                    HorizontalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Bold,
                                    VerticalTextAlignment = TextAlignment.Center,
                                    VerticalOptions = LayoutOptions.Center
                                };

                                timeLabel1.HeightRequest = rowHeight;
                                HoursHeaderGrid.RowDefinitions[rowIndex + 1].Height = new GridLength(rowHeight, GridUnitType.Absolute);

                                Grid.SetRow(timeLabel1, (rowIndex + 1));
                                Grid.SetColumn(timeLabel1, 0);
                                HoursHeaderGrid.Children.Add(timeLabel1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
            }
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Bir hata oluþtu: {ex.Message}", "Tamam");
            throw;
        }
        finally
        {
            loadingPopup.Close();
        }
    }
    private DateTime StartOfWeek(DateTime date)
    {
        var diff = date.DayOfWeek - DayOfWeek.Monday;
        if (diff < 0)
            diff += 7;

        return date.AddDays(-diff).Date;
    }
    private DateTime GetStartOfWeekInMonth(int month, int weekNumber)
    {
        DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, month, 1);

        var firstDayOfWeek = StartOfWeek(firstDayOfMonth);
        return firstDayOfWeek.AddDays(7 * (weekNumber - 1));
    }
    public int GetWeekOfYear(DateTime date)
    {
        var culture = System.Globalization.CultureInfo.CurrentCulture;
        return culture.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }
    public string GetMonthName(int month)
    {
        var culture = new System.Globalization.CultureInfo("tr-TR");
        return new DateTime(1, month, 1).ToString("MMMM", culture);
    }
    private async void OnClearButtonClicked(object sender, EventArgs e)
    {
        SearchEntry.Text = string.Empty;
        searchName = string.Empty;
        btnClear.IsVisible = false;
        LoadLessons();
    }
}