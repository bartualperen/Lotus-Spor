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

        string searchName = SearchEntry.Text;
        LoadLessons(searchName); // Arama ismine g�re seanslar� y�kle
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
    private async void LoadLessons(string searchName = "")
    {
        string antrenor = loggedInUser + " " + loggedInUser2;
        string query = @"
                SELECT 
                    s.tarih AS Day, 
                    s.saat AS Time, 
                    s.durum AS Status, 
                    s.tur AS Type, 
                    CONCAT(m.isim, ' ', m.soyisim) AS Client 
                FROM 
                    seanslar s
                INNER JOIN 
                    musteriler m ON s.musteri_id = m.id
                WHERE 
                    s.antrenor = @loggedInUser";

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
                    command.Parameters.AddWithValue("@loggedInUser", antrenor);
                    if (!string.IsNullOrEmpty(searchName))
                    {
                        command.Parameters.AddWithValue("@searchName", "%" + searchName + "%");
                    }

                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        // G�n ve saat baz�nda verileri saklamak i�in dictionary
                        var lessonsByDayAndTime = new Dictionary<string, Dictionary<string, string>>();
                        var saatler = new HashSet<string>();
                        var gunler = new HashSet<string>();

                        while (await reader.ReadAsync())
                        {
                            string day = DateTime.Parse(reader["Day"].ToString()).ToString("dddd", new CultureInfo("tr-TR"));
                            string time = reader["Time"].ToString();
                            string clientInfo = $"{reader["Client"]} - {reader["Type"] ?? "Bilinmiyor"}";

                            gunler.Add(day);
                            saatler.Add(time);

                            if (!lessonsByDayAndTime.ContainsKey(day))
                                lessonsByDayAndTime[day] = new Dictionary<string, string>();

                            lessonsByDayAndTime[day][time] = clientInfo;
                        }

                        // G�n s�ralamas�
                        var gunSirasi = new List<string> { "Pazartesi", "Sal�", "�ar�amba", "Per�embe", "Cuma", "Cumartesi", "Pazar" };
                        var sortedGunler = gunSirasi.Where(gun => gunler.Contains(gun)).ToList();

                        // Saatleri s�ralama
                        var sortedSaatler = saatler.OrderBy(s => s).ToList();

                        // Grid yap�land�rmas�
                        LessonsListView.RowDefinitions.Clear();
                        LessonsListView.ColumnDefinitions.Clear();
                        LessonsListView.Children.Clear();

                        // �lk sat�rda saat ba�l�klar�n� ekle
                        LessonsListView.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        for (int i = 0; i < sortedSaatler.Count; i++)
                        {
                            LessonsListView.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                            var label = new Label
                            {
                                Text = sortedSaatler[i],
                                HorizontalOptions = LayoutOptions.Center,
                                FontAttributes = FontAttributes.Bold
                            };
                            Grid.SetRow(label, 0); // Ba�l�k sat�r�
                            Grid.SetColumn(label, i + 1); // Saat s�tunlar�
                            LessonsListView.Children.Add(label);
                        }

                        // G�nleri ve seanslar� ekle
                        for (int rowIndex = 0; rowIndex < sortedGunler.Count; rowIndex++)
                        {
                            LessonsListView.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                            // G�n ba�l���n� ekle
                            var dayLabel = new Label
                            {
                                Text = sortedGunler[rowIndex],
                                HorizontalOptions = LayoutOptions.Center,
                                FontAttributes = FontAttributes.Bold
                            };
                            Grid.SetRow(dayLabel, rowIndex + 1); // G�n sat�rlar�
                            Grid.SetColumn(dayLabel, 0); // �lk s�tun
                            LessonsListView.Children.Add(dayLabel);

                            // Saatlere g�re seanslar� ekle
                            for (int colIndex = 0; colIndex < sortedSaatler.Count; colIndex++)
                            {
                                string time = sortedSaatler[colIndex];
                                string day = sortedGunler[rowIndex];

                                string clientInfo = lessonsByDayAndTime.ContainsKey(day) && lessonsByDayAndTime[day].ContainsKey(time)
                                    ? lessonsByDayAndTime[day][time]
                                    : "";

                                var sessionLabel = new Label
                                {
                                    Text = clientInfo,
                                    HorizontalOptions = LayoutOptions.Center,
                                    FontAttributes = FontAttributes.Italic
                                };
                                Grid.SetRow(sessionLabel, rowIndex + 1); // G�n sat�r�
                                Grid.SetColumn(sessionLabel, colIndex + 1); // Saat s�tunu
                                LessonsListView.Children.Add(sessionLabel);
                            }
                        }
                        LessonsListView.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

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
    }
}