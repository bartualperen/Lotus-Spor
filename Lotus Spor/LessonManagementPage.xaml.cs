using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Lotus_Spor;

public partial class LessonManagementPage : ContentPage
{
    List<string> isimListesi = new List<string>();
    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);
    string loggedInUser2 = Preferences.Get("LoggedInUser2", string.Empty);
    string gender = Preferences.Get("Gender", string.Empty);
    int loggedInUserId = int.Parse(Preferences.Get("LoggedInUserId", "0"));
    string userRole = Preferences.Get("UserRole", string.Empty);
    private DateTime oldDate;
    private TimeSpan oldTime;
    private int selectedDayOfWeek;

    public ObservableCollection<Lesson> Lessons { get; set; } = new ObservableCollection<Lesson>();
    private int kullaniciId = -1;
    public class Kisi
    {
        public string Name { get; set; }
    }
    public LessonManagementPage()
	{
		InitializeComponent();
        ResultsCollectionView.ItemsSource = filteredList;
        kisilistele();
        BindingContext = this;
        LoadLessons();
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

        string searchName = SearchEntry.Text;
        string serviceType = ServiceTypePicker.SelectedItem?.ToString();  // Seçilen hizmet türü

        LoadLessons(searchName, serviceType);
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
    private async void OnServiceTypeChanged(object sender, EventArgs e)
    {
        string searchName = SearchEntry.Text;
        string serviceType = ServiceTypePicker.SelectedItem?.ToString();  // Seçilen hizmet türü

        LoadLessons(searchName, serviceType);
    }
    private async void OnCurDateChangeClicked(object sender, EventArgs e)
    {
        string name = Client.Text; // Seçilen müþteri adýný al
        DateTime currentDate = DateTime.Now.Date; // Bugünün tarihi (saat kýsmý hariç)

        DateTime newDate = SeansDate.Date; // Seçilen yeni tarih
        TimeSpan newTime = SeansTime.Time; // Seçilen yeni saat

        // SQL sorgusunu oluþtur
        string query = @"
                UPDATE seanslar s
                JOIN musteriler m ON s.musteri_id = m.id
                SET s.tarih = @newDate, s.saat = @newTime
                WHERE CONCAT(m.isim, ' ', m.soyisim) = @clientName
                AND s.tarih = @selectedDate";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@clientName", name);
                    command.Parameters.AddWithValue("@selectedDate", oldDate);
                    //command.Parameters.AddWithValue("@dayOfWeek", selectedDayOfWeek);
                    command.Parameters.AddWithValue("@newDate", newDate);
                    command.Parameters.AddWithValue("@newTime", newTime.ToString(@"hh\:mm"));

                    // Sorguyu çalýþtýr
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Baþarýlý", "Ýleri tarihli cuma günleri derslerinin tarihi ve saati baþarýyla güncellendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Ýleri tarihli cuma günleri için ders bulunamadý veya güncellenemedi. Lütfen verileri kontrol edin.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
    }
    private async void OnAllChangeClicked(object sender, EventArgs e)
    {
        string name = Client.Text; // Seçilen müþteri adýný al

        DateTime newDate = SeansDate.Date; // Seçilen yeni tarih
        TimeSpan newTime = SeansTime.Time; // Seçilen yeni saat

        // SQL sorgusunu oluþtur
        string query = @"
                UPDATE seanslar s
                JOIN musteriler m ON s.musteri_id = m.id
                SET s.tarih = @newDate, s.saat = @newTime
                WHERE CONCAT(m.isim, ' ', m.soyisim) = @clientName
                AND s.tarih >= @selectedDate
                AND s.saat = @selectedTime;
                ";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Parametreleri ekle
                    command.Parameters.AddWithValue("@clientName", name); // Müþteri adý parametresi
                    command.Parameters.AddWithValue("@newDate", newDate); // Yeni tarih parametresi
                    command.Parameters.AddWithValue("@selectedDate", oldDate);
                    command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));
                    command.Parameters.AddWithValue("@newTime", newTime.ToString(@"hh\:mm")); // Yeni saat parametresi

                    // Sorguyu çalýþtýr
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Baþarýlý", "Ýleri tarihli derslerinin tarihi ve saati baþarýyla güncellendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Ýleri tarihli ders bulunamadý veya güncellenemedi. Lütfen verileri kontrol edin.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex}", "Tamam");
        }
    }
    private async void OnDoneClicked(object sender, EventArgs e)
    {
        string name = Client.Text; // Seçilen müþteri adýný al

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
                    command.Parameters.AddWithValue("@clientName", name);
                    command.Parameters.AddWithValue("@selectedDate", oldDate);
                    command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));

                    // Sorguyu çalýþtýr
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Baþarýlý", "Seans durumu 'Yapýldý' olarak güncellendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Seans güncellenemedi. Lütfen verileri kontrol edin.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
        DersDetaylar.IsVisible = false;
        Client.Text = string.Empty;
        Type.Text = string.Empty;
        SeansDate.Date = DateTime.Now;
        SeansTime.Time = TimeSpan.Zero;
        LoadLessons();
    }
    private async void OnUnDoneClicked(object sender, EventArgs e)
    {
        string name = Client.Text; // Seçilen müþteri adýný al

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
                    command.Parameters.AddWithValue("@clientName", name);
                    command.Parameters.AddWithValue("@selectedDate", oldDate);
                    command.Parameters.AddWithValue("@selectedTime", oldTime.ToString(@"hh\:mm"));

                    // Sorguyu çalýþtýr
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Baþarýlý", "Seans durumu 'Yapýlmadý' olarak güncellendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Seans güncellenemedi. Lütfen verileri kontrol edin.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
        DersDetaylar.IsVisible = false;
        Client.Text = string.Empty;
        Type.Text = string.Empty;
        SeansDate.Date = DateTime.Now;
        SeansTime.Time = TimeSpan.Zero;
        LoadLessons();
    }
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        DersDetaylar.IsVisible = false;
        Client.Text = string.Empty;
        Type.Text = string.Empty;
        SeansDate.Date = DateTime.Now;
        SeansTime.Time = TimeSpan.Zero;
        LoadLessons();
    }
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        DersDetaylar.IsVisible = false;
        Client.Text = string.Empty;   
        Type.Text = string.Empty;     
        SeansDate.Date = DateTime.Now;
        SeansTime.Time = TimeSpan.Zero;
        LoadLessons();
    }
    private async void LoadLessons(string searchName = "", string hizmet_turu = "")
    {
        string antrenor = loggedInUser + " " + loggedInUser2;
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
            s.tur = @tur";

        if (!string.IsNullOrEmpty(searchName))
        {
            query += " AND CONCAT(m.isim, ' ', m.soyisim) LIKE @searchName";
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
                    if (!string.IsNullOrEmpty(searchName))
                    {
                        command.Parameters.AddWithValue("@searchName", "%" + searchName + "%");
                    }

                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        // Gün ve saat bazýnda verileri saklamak için dictionary
                        var lessonsByDayAndTime = new Dictionary<string, Dictionary<string, (DateTime, TimeSpan, string)>>();
                        var saatler = new HashSet<string>();
                        var gunler = new HashSet<string>();

                        while (await reader.ReadAsync())
                        {
                            string day = DateTime.Parse(reader["Day"].ToString()).ToString("dddd", new CultureInfo("tr-TR"));
                            string time = reader["Time"].ToString();
                            string clientInfo = $"{reader["Client"]}";

                            DateTime parsedDate = DateTime.Parse(reader["Day"].ToString());
                            TimeSpan parsedTime = TimeSpan.Parse(reader["Time"].ToString());

                            gunler.Add(day);
                            saatler.Add(time);

                            if (!lessonsByDayAndTime.ContainsKey(day))
                                lessonsByDayAndTime[day] = new Dictionary<string, (DateTime, TimeSpan, string)>();

                            lessonsByDayAndTime[day][time] = (parsedDate, parsedTime, clientInfo);
                        }

                        // Gün sýralamasý
                        var gunSirasi = new List<string> { "Pazartesi", "Salý", "Çarþamba", "Perþembe", "Cuma", "Cumartesi", "Pazar" };
                        var sortedGunler = gunSirasi.Where(gun => gunler.Contains(gun)).ToList();

                        // Saatleri sýralama
                        var sortedSaatler = saatler.OrderBy(s => s).ToList();

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

                            // Saat baþlýðýný ekle
                            var timeLabel = new Label
                            {
                                Text = sortedSaatler[rowIndex],
                                HorizontalOptions = LayoutOptions.Center,
                                FontAttributes = FontAttributes.Bold
                            };
                            Grid.SetRow(timeLabel, rowIndex + 1); // Saat satýrlarý
                            Grid.SetColumn(timeLabel, 0); // Ýlk sütun
                            LessonsListView.Children.Add(timeLabel);

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

                            // Günlere göre seanslarý ekle
                            for (int colIndex = 0; colIndex < sortedGunler.Count; colIndex++)
                            {
                                string day = sortedGunler[colIndex];
                                string time = sortedSaatler[rowIndex];

                                var clientData = lessonsByDayAndTime.ContainsKey(day) && lessonsByDayAndTime[day].ContainsKey(time)
                                    ? lessonsByDayAndTime[day][time]
                                    : (default(DateTime), default(TimeSpan), string.Empty);

                                var sessionLabel = new Label
                                {
                                    Text = clientData.Item3,
                                    HorizontalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Italic
                                };

                                var tapGestureRecognizer = new TapGestureRecognizer();
                                tapGestureRecognizer.Tapped += (s, e) =>
                                {
                                    if (!string.IsNullOrEmpty(clientData.Item3))
                                    {
                                        DersDetaylar.IsVisible = true;
                                        Client.Text = clientData.Item3;
                                        SeansDate.Date = clientData.Item1;
                                        oldDate = clientData.Item1;
                                        oldTime = clientData.Item2;
                                        SeansTime.Time = clientData.Item2;
                                    }
                                };
                                sessionLabel.GestureRecognizers.Add(tapGestureRecognizer);

                                Grid.SetRow(sessionLabel, rowIndex + 1); // Saat satýrý
                                Grid.SetColumn(sessionLabel, colIndex + 1); // Gün sütunu
                                LessonsListView.Children.Add(sessionLabel);

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
                                    Grid.SetColumn(verticalLine, colIndex + 1); // Her sütun arasýna
                                    Grid.SetRowSpan(verticalLine, 0); // Her hücreye bir çizgi
                                    LessonsListView.Children.Add(verticalLine);
                                }
                            }
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