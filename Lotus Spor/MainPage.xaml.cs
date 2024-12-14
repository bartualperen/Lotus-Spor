using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Windows.Input;

namespace Lotus_Spor
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Lesson> Lessons { get; set; } = new ObservableCollection<Lesson>();
        public MainPage()
        {
            InitializeComponent();
            LoadLessonsAsync();
            BindingContext = this;
            LessonsListView.ItemsSource = Lessons;
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
        private async void OnClickedCancel(Object sender, EventArgs e)
        {
            
        }
        private async void OnClickedMessages(Object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Duyurular());
        }
        private async void OnClickedMeasurements(Object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MeasurementPage());
        }
        public async void LoadLessonsAsync()
        {
            // Kullanıcı ID'sini alın
            int loggedInUserId = int.Parse(Preferences.Get("LoggedInUserId", "0"));
            string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);
            string loggedInUser2 = Preferences.Get("LoggedInUser2", string.Empty);
            string user = loggedInUser + " " + loggedInUser2;

            string query = @"SELECT s.tarih, s.saat, s.durum, s.tur, s.antrenor, m.isim, m.soyisim
                            FROM seanslar s
                            JOIN musteriler m ON s.musteri_id = m.id
                            WHERE s.musteri_id = @loggedInUserID AND durum = 'beklemede'
                            ORDER BY s.tarih ASC
                            LIMIT 5";


            try
            {
                using (MySqlConnection connection = Database.GetConnection())
                {
                    await connection.OpenAsync();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@loggedInUserID", loggedInUserId);

                        using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                        {
                            Lessons.Clear(); // Listeyi sıfırla

                            while (await reader.ReadAsync())
                            {
                                Lessons.Add(new Lesson
                                {
                                    Day = DateTime.Parse(reader["tarih"].ToString()).ToString("yyyy-MM-dd"),
                                    Status = reader["durum"]?.ToString() ?? "Bilinmiyor",
                                    Type = reader["tur"]?.ToString() ?? "Bilinmiyor",
                                    Time = reader["saat"].ToString(),
                                    Antrenor = reader["antrenor"]?.ToString() ?? "Bilinmiyor"
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
        public class Lesson
        {
            public string Day { get; set; }
            public string Time { get; set; }
            public string Status { get; set; }
            public string Type { get; set; }
            public string Antrenor { get; set; }
            public string DayWithWeekday
            {
                get
                {
                    DateTime dayDate;
                    if (DateTime.TryParse(Day, out dayDate))
                    {
                        // Gün bilgisiyle birlikte tarihi döndürüyoruz
                        return dayDate.ToString("yyyy-MM-dd", new CultureInfo("tr-TR")) + " (" + dayDate.ToString("dddd", new CultureInfo("tr-TR")) + ")";
                    }
                    return Day; // Eğer tarih dönüştürülemezse sadece günü döndür
                }
            }
        }
    }
}