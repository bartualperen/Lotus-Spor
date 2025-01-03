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
        isimListesi2.Insert(0, "T�m�");
        AntrenorPicker.ItemsSource = isimListesi2;
        AntrenorPicker.SelectedIndex = AntrenorPicker.Items.IndexOf("T�m�");
        ServiceTypePicker.SelectedIndex = ServiceTypePicker.Items.IndexOf("T�m�");
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

                // SQL komutunu �al��t�r
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Verileri listeye ekle
                        while (reader.Read())
                        {
                            string isim = reader["isim"].ToString();
                            string soyisim = reader["soyisim"].ToString();
                            string fullName = $"{isim} {soyisim}";  // �sim ve soyisim birle�iyor
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

                // SQL komutunu �al��t�r
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Verileri listeye ekle
                        while (reader.Read())
                        {
                            string isim = reader["isim"].ToString();
                            string soyisim = reader["soyisim"].ToString();
                            string fullName = $"{isim} {soyisim}";  // �sim ve soyisim birle�iyor
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

        // CollectionView'� g�ncelle
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

        // CollectionView'� g�ncelle
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
                SearchEntry.Text = selectedName;  // Se�ilen ismi Entry'ye yaz
                ResultsCollectionView.IsVisible = false;  // �nerileri gizle
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
                NameEntry.Text = selectedName;  // Se�ilen ismi Entry'ye yaz
                ResultsCollectionView2.IsVisible = false;  // �nerileri gizle
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
                AntrenorNameEntry.Text = selectedName;  // Se�ilen ismi Entry'ye yaz
                ResultsCollectionView1.IsVisible = false;  // �nerileri gizle
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

                // Kullan�c�y� bulmak i�in sorgu
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
                        await DisplayAlert("Hata", "Kullan�c� bulunamad�.", "Tamam");
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
        // Picker'dan se�ilen g�n�n indeksine g�re g�n�n numaras�n� ayarla
        switch (DayPicker.SelectedIndex)
        {
            case 0:
                selectedDayOfWeek = 7; // Pazar
                break;
            case 1:
                selectedDayOfWeek = 1; // Pazartesi
                break;
            case 2:
                selectedDayOfWeek = 2; // Sal�
                break;
            case 3:
                selectedDayOfWeek = 3; // �ar�amba
                break;
            case 4:
                selectedDayOfWeek = 4; // Per�embe
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

        // CollectionView'� g�ncelle
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
            else if (timePicker == StartTimeSali) selectedDay = "Sal�";
            else if (timePicker == StartTimeCarsamba) selectedDay = "�ar�amba";
            else if (timePicker == StartTimePersembe) selectedDay = "Per�embe";
            else if (timePicker == StartTimeCuma) selectedDay = "Cuma";
            else if (timePicker == StartTimeCumartesi) selectedDay = "Cumartesi";
            else if (timePicker == StartTimePazar) selectedDay = "Pazar";

            string selectedTime = timePicker.Time.ToString(@"hh\:mm");

            DateTime today = DateTime.Today;
            DateTime targetDate = today;

            Dictionary<string, DayOfWeek> dayMap = new Dictionary<string, DayOfWeek>
            {
                { "Pazartesi", DayOfWeek.Monday },
                { "Sal�", DayOfWeek.Tuesday },
                { "�ar�amba", DayOfWeek.Wednesday },
                { "Per�embe", DayOfWeek.Thursday },
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

            // �ak��ma kontrol�
            var isConflicting = DoluSeanslar.Any(doluSeans =>
                DateTime.Parse(doluSeans.Gun).ToString("yyyy-MM-dd") == selectedDateString &&
                TimeSpan.Parse(doluSeans.BaslangicSaat).ToString(@"hh\:mm") == selectedTime);

            if (isConflicting)
            {
                await DisplayAlert("Uyar�", $"{selectedDay} g�n� {selectedTime} saatinde bir ders zaten mevcut!", "Tamam");
                isResettingTime = true;
                timePicker.Time = TimeSpan.Zero;
                isResettingTime = false;
            }
            else
            {
                await DisplayAlert("Uygun", $"{selectedDay} g�n� {selectedTime} saatinde ders eklenebilir.", "Tamam");
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
        string fullName = Client.Text; // Se�ilen m��teri adlar�
        string[] clientNameList = fullName.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Sat�rlara g�re ay�r

        try
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@selectedDate", oldDate); // Se�ilen tarih parametresi
                    cmd.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm")); // Se�ilen saat parametresi

                    // Her m��teri i�in sorguyu �al��t�r
                    foreach (var name in clientNameList)
                    {
                        GetKullaniciId(name.Trim());
                        int musteriID = kullaniciId;

                        cmd.Parameters.AddWithValue("@kullaniciID", musteriID); // Her m��teri i�in ID parametresi ekle

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Ba�ar�l�", $"{name.Trim()} i�in ders kayd� ba�ar�yla silindi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name.Trim()} i�in dersler silinemedi. L�tfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreleri s�f�rla, bir sonraki m��teri i�in yeniden ayarlamak i�in
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@selectedDate", oldDate); // Eski tarih parametresini tekrar ekle
                        cmd.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler silinirken bir hata olu�tu: {ex.Message}", "Tamam");
        }

        Sifirla();

    }
    private async void OnKayitSilClicked(object sender, EventArgs e)
    {
        string query = "DELETE FROM seanslar WHERE musteri_id = @kullaniciID AND tarih <= @selectedDate AND tarih > @selectedDate AND saat = @selectedTime";
        string fullName = Client.Text; // Se�ilen m��teri adlar�
        string[] clientNameList = fullName.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Sat�rlara g�re ay�r

        try
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@selectedDate", oldDate); // Se�ilen tarih parametresi
                    cmd.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm")); // Se�ilen saat parametresi

                    // Her m��teri i�in sorguyu �al��t�r
                    foreach (var name in clientNameList)
                    {
                        GetKullaniciId(name.Trim());
                        int musteriID = kullaniciId; // M��teri ID'sini almak i�in fonksiyonu �a��r

                        cmd.Parameters.AddWithValue("@kullaniciID", musteriID); // Her m��teri i�in ID parametresi ekle

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Ba�ar�l�", $"{name.Trim()} i�in ders kayd� ba�ar�yla silindi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name.Trim()} i�in dersler silinemedi. L�tfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreleri s�f�rla, bir sonraki m��teri i�in yeniden ayarlamak i�in
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@selectedDate", oldDate); // Eski tarih parametresini tekrar ekle
                        cmd.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler silinirken bir hata olu�tu: {ex.Message}", "Tamam");
        }

        Sifirla();

    }
    private async void OnCurDateChangeClicked(object sender, EventArgs e)
    {
        string clientNames = Client.Text; // Se�ilen m��teri adlar�
        string[] clientNameList = clientNames.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Adlar� ay�rmak i�in sat�rlara g�re b�lelim

        DateTime currentDate = DateTime.Now.Date; // Bug�n�n tarihi (saat k�sm� hari�)

        DateTime newDate = SeansDate.Date; // Se�ilen yeni tarih
        TimeSpan newTime = SeansTime.Time; // Se�ilen yeni saat

        // SQL sorgusunu olu�tur
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
                    command.Parameters.AddWithValue("@selectedDate", oldDate); // Se�ilen eski tarih parametresi
                    command.Parameters.AddWithValue("@newDate", newDate); // Yeni tarih parametresi
                    command.Parameters.AddWithValue("@newTime", newTime.ToString(@"hh\:mm")); // Yeni saat parametresi

                    // Her m��teri i�in sorguyu �al��t�r
                    foreach (var name in clientNameList)
                    {
                        command.Parameters.AddWithValue("@clientName", name.Trim()); // Her m��teri ad� i�in parametre ekle (Trim ile bo�luklar� temizle)

                        // Sorguyu �al��t�r
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Ba�ar�l�", $"{name} i�in dersin tarihi ve saati ba�ar�yla g�ncellendi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name} i�in ders bulunamad� veya g�ncellenemedi. L�tfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreyi s�f�rla, bir sonraki m��teri i�in yeniden ayarlamak i�in
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
            await DisplayAlert("Hata", $"Veriler y�klenirken bir hata olu�tu: {ex.Message}", "Tamam");
        }

        Sifirla();

    }
    private async void OnAllChangeClicked(object sender, EventArgs e)
    {
        string clientNames = Client.Text; // Se�ilen m��teri adlar�
        string[] clientNameList = clientNames.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Adlar� ay�rmak i�in sat�rlara g�re b�lelim

        DateTime newDate = SeansDate.Date; // Se�ilen yeni tarih
        TimeSpan newTime = SeansTime.Time; // Se�ilen yeni saat

        // SQL sorgusunu olu�tur
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
                    command.Parameters.AddWithValue("@selectedDate", oldDate); // Se�ilen eski tarih
                    command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm")); // Se�ilen eski saat parametresi
                    command.Parameters.AddWithValue("@newTime", newTime.ToString(@"hh\:mm")); // Yeni saat parametresi

                    // Her m��teri i�in sorguyu �al��t�r
                    foreach (var name in clientNameList)
                    {
                        command.Parameters.AddWithValue("@clientName", name.Trim()); // Trim ile gereksiz bo�luklardan kurtuluyoruz

                        // Sorguyu �al��t�r
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Ba�ar�l�", $"�leri tarihli derslerinin tarihi ve saati {name} i�in ba�ar�yla g�ncellendi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name} i�in ileri tarihli ders bulunamad� veya g�ncellenemedi. L�tfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreyi s�f�rla, bir sonraki m��teri i�in yeniden ayarlamak i�in
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
            await DisplayAlert("Hata", $"Veriler y�klenirken bir hata olu�tu: {ex.Message}", "Tamam");
        }
        Sifirla();

    }
    private async void OnDoneClicked(object sender, EventArgs e)
    {
        string clientNames = Client.Text; // Se�ilen m��teri adlar�
        string[] clientNameList = clientNames.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Adlar� ay�rmak i�in sat�rlara g�re b�lelim

        // SQL sorgusunu olu�tur
        string query = @"
        UPDATE seanslar s
        JOIN musteriler m ON s.musteri_id = m.id
        SET s.durum = 'Yap�ld�'
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

                    // Her m��teri i�in sorguyu �al��t�r
                    foreach (var name in clientNameList)
                    {
                        command.Parameters.AddWithValue("@clientName", name);

                        // Sorguyu �al��t�r
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Ba�ar�l�", $"Seans durumu 'Yap�ld�' olarak {name} i�in g�ncellendi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{name} i�in seans g�ncellenemedi. L�tfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreyi s�f�rla, bir sonraki m��teri i�in yeniden ayarlamak i�in
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@selectedDate", oldDate);
                        command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler y�klenirken bir hata olu�tu: {ex.Message}", "Tamam");
        }
        Sifirla();

    }
    private async void OnUnDoneClicked(object sender, EventArgs e)
    {
        string name = Client.Text; // Se�ilen m��teri adlar�n� al
        string[] clientNameList = name.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Sat�rlara g�re ay�r

        // SQL sorgusunu olu�tur
        string query = @"
                        UPDATE seanslar s
                        JOIN musteriler m ON s.musteri_id = m.id
                        SET s.durum = 'Yap�lmad�'
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

                    // Her m��teri i�in sorguyu �al��t�r
                    foreach (var clientName in clientNameList)
                    {
                        command.Parameters.AddWithValue("@clientName", clientName.Trim()); // Her m��teri i�in isim parametresini ekle

                        // Sorguyu �al��t�r
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Ba�ar�l�", $"{clientName.Trim()} i�in seans durumu 'Yap�lmad�' olarak g�ncellendi.", "Tamam");
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"{clientName.Trim()} i�in seans g�ncellenemedi. L�tfen verileri kontrol edin.", "Tamam");
                        }

                        // Parametreleri s�f�rla, bir sonraki m��teri i�in yeniden ayarlamak i�in
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@selectedDate", oldDate); // Eski tarih parametresini tekrar ekle
                        command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler y�klenirken bir hata olu�tu: {ex.Message}", "Tamam");
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

        // Se�ilen g�nleri birle�tirme
        List<string> secilenGunler = new List<string>();
        if (CheckBoxPazartesi.IsChecked) secilenGunler.Add("Pazartesi");
        if (CheckBoxSali.IsChecked) secilenGunler.Add("Sal�");
        if (CheckBoxCarsamba.IsChecked) secilenGunler.Add("�ar�amba");
        if (CheckBoxPersembe.IsChecked) secilenGunler.Add("Per�embe");
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
            { "Sal�", DayOfWeek.Tuesday },
            { "�ar�amba", DayOfWeek.Wednesday },
            { "Per�embe", DayOfWeek.Thursday },
            { "Cuma", DayOfWeek.Friday },
            { "Cumartesi", DayOfWeek.Saturday },
            { "Pazar", DayOfWeek.Sunday }
        };

        Dictionary<string, TimeSpan> seansSaatleri = new Dictionary<string, TimeSpan>();
        List<DateTime> seansD�nemTarihler = new List<DateTime>();
        DateTime today = DateTime.Today;
        DateTime endDate = today.AddMonths(12);

        foreach (string gun in secilenGunler)
        {
            switch (gun)
            {
                case "Pazartesi":
                    seansSaatleri["Pazartesi"] = StartTimePazartesi.Time;
                    break;
                case "Sal�":
                    seansSaatleri["Sal�"] = StartTimeSali.Time;
                    break;
                case "�ar�amba":
                    seansSaatleri["�ar�amba"] = StartTimeCarsamba.Time;
                    break;
                case "Per�embe":
                    seansSaatleri["Per�embe"] = StartTimePersembe.Time;
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
                await DisplayAlert("Hata", $"{gun} i�in bir saat se�ilmelidir.", "Tamam");
                return;
            }

            DayOfWeek hedefGun = gunlerMap[gun];
            DateTime seansTarihi = today;

            // Se�ilen g�n� bulmak i�in
            while (seansTarihi.DayOfWeek != hedefGun)
            {
                seansTarihi = seansTarihi.AddDays(1);
            }

            // Tarihleri bir ayl�k s�re i�inde ekleme
            while (seansTarihi < endDate)
            {
                seansD�nemTarihler.Add(seansTarihi);
                seansTarihi = seansTarihi.AddDays(7); // Haftal�k art��
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

                    foreach (DateTime seansTarihi in seansD�nemTarihler)
                    {
                        string gunAdi = seansTarihi.DayOfWeek switch
                        {
                            DayOfWeek.Monday => "Pazartesi",
                            DayOfWeek.Tuesday => "Sal�",
                            DayOfWeek.Wednesday => "�ar�amba",
                            DayOfWeek.Thursday => "Per�embe",
                            DayOfWeek.Friday => "Cuma",
                            DayOfWeek.Saturday => "Cumartesi",
                            DayOfWeek.Sunday => "Pazar",
                            _ => throw new InvalidOperationException("Ge�ersiz g�n")
                        };

                        seansTarihiParam.Value = seansTarihi.ToString("yyyy-MM-dd");
                        seansSaatiParam.Value = seansSaatleri[gunAdi].ToString(@"hh\:mm");

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected <= 0)
                        {
                            await DisplayAlert("Hata", "Seans eklenirken bir sorun olu�tu.", "Tamam");
                        }
                    }

                    await DisplayAlert("Ba�ar�l�", "Seanslar ba�ar�yla eklendi.", "Tamam");
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
            await DisplayAlert("Veritaban� Hatas�", ex.Message, "Tamam");
        }
        Sifirla();
    }
    private async void OnTekEkleClicked(object sender, EventArgs e)
    {
        string antrenorName = AntrenorNameEntry.Text;
        string seansTur = SeansPicker.SelectedItem?.ToString();

        string seansquery = "INSERT INTO seanslar (tur, musteri_id, antrenor, tarih, saat, grup) VALUES (@seans_turu, @musteri_id, @antrenor, @seans_tarihi, @seans_saati, @grup)";

        // Se�ilen tarih ve saat
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
                        await DisplayAlert("Ba�ar�l�", "Seans ba�ar�yla eklendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Seans eklenirken bir sorun olu�tu.", "Tamam");
                    }
                }

                // Seans ekleme ba�ar�l� ise seanslar� tekrar y�kleyebilirsiniz
                DersEkle.IsVisible = false;

                // Arama kriterlerini al�p dersleri tekrar y�kleyebilirsiniz
                searchName = SearchEntry.Text;
                serviceType = ServiceTypePicker.SelectedItem?.ToString();
                antrenor = AntrenorPicker.SelectedItem?.ToString();
                LoadLessons(searchName, serviceType, antrenor);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritaban� Hatas�", ex.Message, "Tamam");
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
            if (selectedAntrenor != "T�m�")
            {
                query += " AND s.antrenor = @antrenor";
            }
        }

        if (ServiceTypePicker.SelectedIndex != -1)
        {
            string selectedServiceType = ServiceTypePicker.SelectedItem.ToString();
            if (selectedServiceType != "T�m�")
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
                        // G�n ve saat baz�nda verileri saklamak i�in dictionary
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

                        // G�n s�ralamas�
                        var gunSirasi = new List<string> { "Pazartesi", "Sal�", "�ar�amba", "Per�embe", "Cuma", "Cumartesi", "Pazar" };
                        var sortedGunler = gunSirasi.Where(gun => gunler.Contains(gun)).ToList();

                        // Saatleri s�ralama
                        var sortedSaatler = saatler.OrderBy(s => s).ToList();

                        int rowHeight = 70;

                        // 1. �stte saatlerin oldu�u grid'i olu�turuyoruz.
                        HoursHeaderGrid.Children.Clear();
                        HoursHeaderGrid.RowDefinitions.Clear();
                        HoursHeaderGrid.ColumnDefinitions.Clear();
                        HoursHeaderGrid.Children.Clear();

                        // Saat ba�l�klar�n� ekliyoruz
                        HoursHeaderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        var boslabel = new Label
                        {
                            Text = string.Empty
                        };
                        HoursHeaderGrid.Children.Add(boslabel);

                        // Grid yap�land�rmas�
                        LessonsListView.RowDefinitions.Clear();
                        LessonsListView.ColumnDefinitions.Clear();
                        LessonsListView.Children.Clear();

                        // �lk sat�rda g�n ba�l�klar�n� ekle
                        LessonsListView.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        LessonsListView.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                        // G�n ba�l�klar�n� ekle
                        for (int i = 0; i < sortedGunler.Count; i++)
                        {
                            LessonsListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                            var label = new Label
                            {
                                Text = sortedGunler[i],
                                HorizontalOptions = LayoutOptions.Center,
                                FontAttributes = FontAttributes.Bold
                            };
                            Grid.SetRow(label, 0); // Ba�l�k sat�r�
                            Grid.SetColumn(label, i + 1); // G�n s�tunlar�
                            LessonsListView.Children.Add(label);
                        }

                        // Saatleri ve seanslar� ekle
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
                                Grid.SetRow(horizontalLine, rowIndex + 1); // Her sat�r ba�l���n�n alt�na
                                Grid.SetColumn(horizontalLine, 0);
                                Grid.SetColumnSpan(horizontalLine, sortedGunler.Count + 1); // T�m s�tunlarda ge�erli
                                LessonsListView.Children.Add(horizontalLine);
                            }

                            int maxHeight = 0;

                            // G�nlere g�re seanslar� ekle
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
                                    var tappedLabel = (Label)s; // T�klanan Label'� al
                                    string clientInfo = tappedLabel.Text; // T�klanan Label'daki ismi al

                                    if (!string.IsNullOrEmpty(clientInfo))
                                    {
                                        // �lgili Ders Detaylar�n� G�ster
                                        DersDetaylar.IsVisible = true;
                                        btnDersEkle.IsVisible = false;
                                        DersEkle.IsVisible = false;

                                        Client.Text = clientInfo;
                                        string clientNames = Client.Text;
                                        string[] clientNameList = clientNames.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                        var selectedLesson = clientDataList.FirstOrDefault(cd => cd.client == clientNameList[0]); // client bilgisi ile e�le�en dersin verisini al

                                        // T�klanan dersin tarih ve saat bilgisi
                                        SeansDate.Date = selectedLesson.date; // G�n�n tarihi
                                        SeansTime.Time = selectedLesson.time;
                                        oldDate = selectedLesson.date;
                                        oldTime = selectedLesson.time; // Saat bilgisi
                                    }
                                };

                                Color labelColor;
                                foreach (var clientData in clientDataList)
                                {
                                    // Duruma g�re renk se�imi
                                    switch (clientData.status)
                                    {
                                        case "Yap�ld�":
                                            labelColor = (currentTheme == AppTheme.Dark) ? Colors.LightGreen : Colors.DarkGreen; // Koyu modda a��k ye�il, ayd�nl�k modda koyu ye�il
                                            break;
                                        case "Yap�lmad�":
                                            labelColor = (currentTheme == AppTheme.Dark) ? Colors.LightCoral : Colors.DarkRed; // Koyu modda a��k k�rm�z�, ayd�nl�k modda koyu k�rm�z�
                                            break;
                                        case "beklemede":
                                            labelColor = (currentTheme == AppTheme.Dark) ? Colors.Yellow : Colors.OrangeRed; // Koyu modda a��k k�rm�z�, ayd�nl�k modda koyu k�rm�z�
                                            break;
                                        default:
                                            labelColor = Colors.Grey; // Di�er durumlar i�in gri
                                            break;
                                    }

                                    // Her Label'a tapGesture eklendi�i yer
                                    var sessionLabel = new Label
                                    {
                                        Text = clientDisplayText, // Alt alta yaz�lm�� ders isimleri
                                        HorizontalOptions = LayoutOptions.Center,
                                        FontAttributes = FontAttributes.Italic,
                                        TextColor = labelColor,
                                        VerticalTextAlignment = TextAlignment.Center
                                    };

                                    sessionLabel.HeightRequest = rowHeight;
                                    LessonsListView.RowDefinitions[rowIndex + 1].Height = new GridLength(rowHeight, GridUnitType.Absolute);
                                    sessionLabel.GestureRecognizers.Add(tapGestureRecognizer);
                                    Grid.SetRow(sessionLabel, rowIndex + 1); // Saat sat�r�
                                    Grid.SetColumn(sessionLabel, colIndex + 1); // G�n s�tunu
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
            await DisplayAlert("Hata", $"Veriler y�klenirken bir hata olu�tu: {ex.Message}", "Tamam");
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