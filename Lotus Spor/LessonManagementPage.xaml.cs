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
        // SQL sorgusu, arama ismine g�re sorguyu filtreliyoruz
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
                s.antrenor = @loggedInUser"; // Antren�r filtresi

        // E�er bir isim arama yap�l�yorsa sorguya ek filtre ekleniyor
        if (!string.IsNullOrEmpty(searchName))
        {
            query += " AND CONCAT(m.isim, ' ', m.soyisim) LIKE @searchName"; // �sim filtresi
        }

        query += " ORDER BY s.tarih ASC, s.saat ASC"; // Tarihe g�re s�ralama


        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@loggedInUser", antrenor);
                    // E�er isim arama yap�lm��sa parametre ekliyoruz
                    if (!string.IsNullOrEmpty(searchName))
                    {
                        command.Parameters.AddWithValue("@searchName", "%" + searchName + "%");
                    }

                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        Lessons.Clear(); // Listeyi s�f�rla

                        while (await reader.ReadAsync())
                        {
                            Lessons.Add(new Lesson
                            {
                                Day = DateTime.Parse(reader["Day"].ToString()).ToString("yyyy-MM-dd"),
                                Time = reader["Time"].ToString(),
                                Status = reader["Status"]?.ToString() ?? "Bilinmiyor",
                                Type = reader["Type"]?.ToString() ?? "Bilinmiyor",
                                Client = reader["Client"].ToString()
                            });
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
        public string DayWithWeekday
        {
            get
            {
                DateTime dayDate;
                if (DateTime.TryParse(Day, out dayDate))
                {
                    // G�n bilgisiyle birlikte tarihi d�nd�r�yoruz
                    return dayDate.ToString("yyyy-MM-dd", new CultureInfo("tr-TR")) + " (" + dayDate.ToString("dddd", new CultureInfo("tr-TR")) + ")";
                }
                return Day; // E�er tarih d�n��t�r�lemezse sadece g�n� d�nd�r
            }
        }
    }
}