using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Windows.Input;

namespace Lotus_Spor
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Lesson> Lessons { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Lessons = new ObservableCollection<Lesson>();
            LoadLessonsAsync();
        }

        protected override void OnAppearing()
        {

            string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);
            string loggedInUser2 = Preferences.Get("LoggedInUser2", string.Empty);
            string gender = Preferences.Get("Gender", string.Empty);
            int loggedInUserId = int.Parse(Preferences.Get("LoggedInUserId", "0"));
            string userRole = Preferences.Get("UserRole", string.Empty);
            string user = loggedInUser + " " + loggedInUser2;

            base.OnAppearing();
            if (!string.IsNullOrEmpty(loggedInUser) && !string.IsNullOrEmpty(gender))
            {
                string title = gender.ToLower() == "erkek" ? "Bey" : "Hanım";
                WelcomeLabel.Text = $"Hoş Geldiniz, {loggedInUser} {title}!";
            }
            else
            {
                WelcomeLabel.Text = "Hoş Geldiniz!";
            }
        }
        private async void OnClickedLogOut(Object sender, EventArgs e)
        {
            Preferences.Remove("UserRole");
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            await Task.CompletedTask;
        }
        protected override bool OnBackButtonPressed()
        {
            return true; // Geri tuşunu devre dışı bırak
        }
        public async void LoadLessonsAsync()
        {
            // Kullanıcı ID'sini alın
            int loggedInUserId = int.Parse(Preferences.Get("LoggedInUserId", "0"));
            string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);
            string loggedInUser2 = Preferences.Get("LoggedInUser2", string.Empty);
            string user = loggedInUser + " " + loggedInUser2;

            // SQL sorgusu
            string query = @"
            SELECT s.tarih, s.saat, s.durum, s.tur, m.isim, m.soyisim
            FROM seanslar s
            JOIN musteriler m ON s.musteri_id = m.id
            WHERE s.musteri_id = @id
            ORDER BY s.tarih ASC";

            try
            {
                using (var connection = Database.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", loggedInUserId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            Lessons.Clear(); // Listeyi temizle

                            while (await reader.ReadAsync())
                            {
                                string tarih = reader.GetDateTime("tarih").ToString("yyyy-MM-dd");
                                string saat = reader["saat"]?.ToString() ?? "Bilinmiyor";
                                string durum = reader["durum"]?.ToString() ?? "Bilinmiyor";
                                string tur = reader["tur"]?.ToString() ?? "Bilinmiyor";
                                string isimSoyisim = $"{reader["isim"]} {reader["soyisim"]}";

                                Debug.WriteLine($"Veri: {tarih}, {saat}, {durum}, {tur}, {isimSoyisim}");

                                Lessons.Add(new Lesson
                                {
                                    Day = tarih,
                                    Time = saat,
                                    Status = durum,
                                    Type = tur,
                                    Client = isimSoyisim
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", "Veriler yüklenirken bir hata oluştu: " + ex.Message, "Tamam");
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
}