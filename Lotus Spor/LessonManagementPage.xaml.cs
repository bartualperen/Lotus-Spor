using Microsoft.Maui.ApplicationModel;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Lotus_Spor;

public partial class LessonManagementPage : ContentPage
{
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

    public ObservableCollection<Lesson> Lessons { get; set; } = new ObservableCollection<Lesson>();
    private int kullaniciId = -1;
    private int kullaniciId1 = -1;
    public class Kisi
    {
        public string Name { get; set; }
    }
    public LessonManagementPage()
	{
		InitializeComponent();
        ResultsCollectionView.ItemsSource = filteredList;
        ResultsCollectionView2.ItemsSource = filteredList;
        ResultsCollectionView1.ItemsSource = filteredList1;
        kisilistele();
        BindingContext = this;
        LoadLessons();
        kisilistele1();
        isimListesi2.Insert(0, "Tümü");
        AntrenorPicker.ItemsSource = isimListesi2;
        AntrenorPicker.SelectedIndex = AntrenorPicker.Items.IndexOf("Tümü");
        ServiceTypePicker.SelectedIndex = ServiceTypePicker.Items.IndexOf("Tümü");
        HaftaPicker.SelectedIndex = HaftaPicker.Items.IndexOf("Bu Hafta");
        hafta = HaftaPicker.SelectedItem?.ToString();
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

        searchName = SearchEntry.Text;
        serviceType = ServiceTypePicker.SelectedItem?.ToString();
        antrenor = AntrenorPicker.SelectedItem?.ToString();
        hafta = HaftaPicker.SelectedItem?.ToString();

        LoadLessons(searchName, serviceType, antrenor, hafta);
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
                string isim = nameParts[0];
                string soyisim = nameParts.Length > 1 ? nameParts[1] : "";

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
        searchName = SearchEntry.Text;
        serviceType = ServiceTypePicker.SelectedItem?.ToString();
        antrenor = AntrenorPicker.SelectedItem?.ToString();
        hafta = HaftaPicker.SelectedItem?.ToString();

        LoadLessons(searchName, serviceType, antrenor, hafta);
    }
    private async void OnAntrenorChanged(object sender, EventArgs e)
    {
        searchName = SearchEntry.Text;
        serviceType = ServiceTypePicker.SelectedItem?.ToString();
        antrenor = AntrenorPicker.SelectedItem?.ToString();
        hafta = HaftaPicker.SelectedItem?.ToString();

        LoadLessons(searchName, serviceType, antrenor, hafta);
    }
    private async void OnHaftaChanged(object sender, EventArgs e)
    {
        searchName = SearchEntry.Text;
        serviceType = ServiceTypePicker.SelectedItem?.ToString();
        antrenor = AntrenorPicker.SelectedItem?.ToString();
        hafta = HaftaPicker.SelectedItem?.ToString();

        LoadLessons(searchName, serviceType, antrenor, hafta);
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
        string query = "DELETE FROM seanslar WHERE musteri_id = @kullaniciID AND tarih <= @selectedDate AND tarih > @selectedDate AND saat = @selectedTime";
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
          AND s.tarih >= @selectedDate
          AND s.saat = @selectedTime;";

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
        DateTime today = DateTime.Today;
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

                    LoadLessons(searchName, serviceType, antrenor);
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
        TimeSpan selectedTime = TimeSpan.Zero;

        if (CheckBoxPazartesi.IsChecked)
        {
            selectedTime = StartTimePazartesi.Time;
        }
        if (CheckBoxSali.IsChecked)
        {
            selectedTime = StartTimeSali.Time;
        }
        if (CheckBoxCarsamba.IsChecked)
        {
            selectedTime = StartTimeCarsamba.Time;
        }
        if (CheckBoxPersembe.IsChecked)
        {
            selectedTime = StartTimePersembe.Time;
        }
        if (CheckBoxCuma.IsChecked)
        {
            selectedTime = StartTimeCuma.Time;
        }
        if (CheckBoxCumartesi.IsChecked)
        {
            selectedTime = StartTimeCumartesi.Time;
        }
        if (CheckBoxPazar.IsChecked)
        {
            selectedTime = StartTimePazar.Time;
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
                LoadLessons(searchName, serviceType, antrenor);
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
        Type.Text = string.Empty;
        SeansDate.Date = DateTime.Now;
        SeansTime.Time = TimeSpan.Zero;
        LoadLessons(searchName, serviceType, antrenor);
    }
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        DersDetaylar.IsVisible = false;
        DersEkle.IsVisible = false;
        btnDersEkle.IsVisible = true;
        Client.Text = string.Empty;   
        Type.Text = string.Empty;     
        SeansDate.Date = DateTime.Now;
        SeansTime.Time = TimeSpan.Zero;
        LoadLessons(searchName, serviceType, antrenor);
    }
    private async void LoadLessons(string searchName = "", string hizmet_turu = "", string antrenor = "", string hafta = "")
    {
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
        WHERE YEARWEEK(s.tarih, 1) = ";

        if (string.IsNullOrEmpty(hafta))
        {
            hafta = "Bu Hafta";
        }

        if (hafta == "Bu Hafta")
        {
            query += "YEARWEEK(CURDATE(), 1)";
        }

        if (hafta == "Sonraki Hafta")
        {
            query += "YEARWEEK(CURDATE(), 1) + 1";
        }

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
                    if (!string.IsNullOrEmpty(searchName))
                    {
                        command.Parameters.AddWithValue("@searchName", "%" + searchName + "%");
                    }

                    List<Tuple<int, int>> rowHeights = new List<Tuple<int, int>>();

                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        // Gün ve saat bazýnda verileri saklamak için dictionary
                        var lessonsByDayAndTime = new Dictionary<string, Dictionary<string, List<(string client, string status, DateTime date, TimeSpan time)>>>();
                        var saatler = new HashSet<string>();
                        var gunler = new HashSet<string>();

                        while (await reader.ReadAsync())
                        {
                            string day = DateTime.Parse(reader["Day"].ToString()).ToString("dddd", new CultureInfo("tr-TR"));
                            string time = reader["Time"].ToString();
                            string clientInfo = $"{reader["Client"]}";
                            string status = reader["Status"].ToString();

                            DateTime parsedDate = DateTime.Parse(reader["Day"].ToString());
                            TimeSpan parsedTime = TimeSpan.Parse(reader["Time"].ToString());

                            gunler.Add(day);
                            saatler.Add(time);

                            if (!lessonsByDayAndTime.ContainsKey(day))
                                lessonsByDayAndTime[day] = new Dictionary<string, List<(string client, string status, DateTime, TimeSpan)>>();

                            if (!lessonsByDayAndTime[day].ContainsKey(time))
                                lessonsByDayAndTime[day][time] = new List<(string client, string status, DateTime date, TimeSpan time)>();

                            lessonsByDayAndTime[day][time].Add((clientInfo, status, parsedDate, parsedTime));
                        }

                        // Gün sýralamasý
                        var gunSirasi = new List<string> { "Pazartesi", "Salý", "Çarþamba", "Perþembe", "Cuma", "Cumartesi", "Pazar" };
                        var sortedGunler = gunSirasi.Where(gun => gunler.Contains(gun)).ToList();

                        // Saatleri sýralama
                        var sortedSaatler = saatler.OrderBy(s => s).ToList();

                        int rowHeight = 70;

                        // 1. Üstte saatlerin olduðu grid'i oluþturuyoruz.
                        HoursHeaderGrid.Children.Clear();
                        HoursHeaderGrid.RowDefinitions.Clear();
                        HoursHeaderGrid.ColumnDefinitions.Clear();
                        HoursHeaderGrid.Children.Clear();

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
                            LessonsListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                            var label = new Label
                            {
                                Text = sortedGunler[i],
                                HorizontalOptions = LayoutOptions.Center,
                                FontAttributes = FontAttributes.Bold
                            };
                            Grid.SetRow(label, 0); // Baþlýk satýrý
                            Grid.SetColumn(label, i + 1); // Gün sütunlarý
                            LessonsListView.Children.Add(label);
                        }

                        // Saatleri ve seanslarý ekle
                        for (int rowIndex = 0; rowIndex < sortedSaatler.Count; rowIndex++)
                        {
                            LessonsListView.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

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
                            }

                            int maxHeight = 0;

                            // Günlere göre seanslarý ekle
                            for (int colIndex = 0; colIndex < sortedGunler.Count; colIndex++)
                            {
                                string day = sortedGunler[colIndex];
                                string time = sortedSaatler[rowIndex];

                                var clientDataList = lessonsByDayAndTime.ContainsKey(day) && lessonsByDayAndTime[day].ContainsKey(time)
                                    ? lessonsByDayAndTime[day][time]
                                    : new List<(string client, string status, DateTime date, TimeSpan time)>();

                                string clientDisplayText = string.Join("\n", clientDataList.Select(cd => cd.client));

                                var currentTheme = Application.Current.RequestedTheme;

                                var tapGestureRecognizer = new TapGestureRecognizer();
                                tapGestureRecognizer.Tapped += async (s, e) =>
                                {
                                    var tappedLabel = (Label)s; // Týklanan Label'ý al
                                    string clientInfo = tappedLabel.Text; // Týklanan Label'daki ismi al

                                    if (!string.IsNullOrEmpty(clientInfo))
                                    {
                                        // Ýlgili Ders Detaylarýný Göster
                                        DersDetaylar.IsVisible = true;
                                        btnDersEkle.IsVisible = false;
                                        DersEkle.IsVisible = false;

                                        Client.Text = clientInfo;
                                        string clientNames = Client.Text;
                                        string[] clientNameList = clientNames.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                        var selectedLesson = clientDataList.FirstOrDefault(cd => cd.client == clientNameList[0]); // client bilgisi ile eþleþen dersin verisini al

                                        // Týklanan dersin tarih ve saat bilgisi
                                        SeansDate.Date = selectedLesson.date; // Günün tarihi
                                        SeansTime.Time = selectedLesson.time;
                                        oldDate = selectedLesson.date;
                                        oldTime = selectedLesson.time; // Saat bilgisi
                                    }
                                };

                                Color labelColor;
                                foreach (var clientData in clientDataList)
                                {
                                    // Duruma göre renk seçimi
                                    switch (clientData.status)
                                    {
                                        case "Yapýldý":
                                            labelColor = (currentTheme == AppTheme.Dark) ? Colors.LightGreen : Colors.DarkGreen; // Koyu modda açýk yeþil, aydýnlýk modda koyu yeþil
                                            break;
                                        case "Yapýlmadý":
                                            labelColor = (currentTheme == AppTheme.Dark) ? Colors.LightCoral : Colors.DarkRed; // Koyu modda açýk kýrmýzý, aydýnlýk modda koyu kýrmýzý
                                            break;
                                        case "beklemede":
                                            labelColor = (currentTheme == AppTheme.Dark) ? Colors.Yellow : Colors.OrangeRed; // Koyu modda açýk kýrmýzý, aydýnlýk modda koyu kýrmýzý
                                            break;
                                        default:
                                            labelColor = Colors.Grey; // Diðer durumlar için gri
                                            break;
                                    }

                                    // Her Label'a tapGesture eklendiði yer
                                    var sessionLabel = new Label
                                    {
                                        Text = clientDisplayText, // Alt alta yazýlmýþ ders isimleri
                                        HorizontalOptions = LayoutOptions.Center,
                                        FontAttributes = FontAttributes.Italic,
                                        TextColor = labelColor,
                                        VerticalTextAlignment = TextAlignment.Center
                                    };

                                    sessionLabel.HeightRequest = rowHeight;
                                    LessonsListView.RowDefinitions[rowIndex + 1].Height = new GridLength(rowHeight, GridUnitType.Absolute);
                                    sessionLabel.GestureRecognizers.Add(tapGestureRecognizer);
                                    Grid.SetRow(sessionLabel, rowIndex + 1); // Saat satýrý
                                    Grid.SetColumn(sessionLabel, colIndex + 1); // Gün sütunu
                                    LessonsListView.Children.Add(sessionLabel);
                                    rowHeights.Add(new Tuple<int, int>(rowIndex + 1, (int)sessionLabel.Height));
                                }

                                if (colIndex < sortedGunler.Count + 4)
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
                                }
                            }

                            HoursHeaderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                            string saatindex = string.Join("\n", sortedSaatler[rowIndex]);

                            var timeLabel1 = new Label
                            {
                                Text = saatindex,
                                HorizontalOptions = LayoutOptions.Center,
                                FontAttributes = FontAttributes.Bold,
                                VerticalTextAlignment = TextAlignment.Center
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
    }
    public class Lesson
    {
        public string Day { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Client { get; set; }
        public string Grup {  get; set; }
    }
}