using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
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
            base.OnAppearing();
            if (!string.IsNullOrEmpty(LoginManager.LoggedInUser) && !string.IsNullOrEmpty(LoginManager.Gender))
            {
                string title = LoginManager.Gender.ToLower() == "erkek" ? "Bey" : "Hanım";
                WelcomeLabel.Text = $"Hoş Geldiniz, {LoginManager.LoggedInUser} {title}!";
            }
            else
            {
                // Kullanıcı henüz giriş yapmamışsa veya veriler eksikse
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
            // Kullanıcı ID'sini alın (örnek olarak statik bir değer kullandık)
            int loggedInUserId = LoginManager.LoggedInUserId;  // Bu değeri giriş yapan kullanıcıya göre ayarlayın

            string query = @"
            SELECT s.tarih, s.saat, s.durum, s.tur, m.isim, m.soyisim
            FROM seanslar s
            JOIN musteriler m ON s.musteri_id = m.id
            WHERE s.musteri_id = @musteri_id
            ORDER BY s.tarih ASC";

            try
            {
                using (var connection = Database.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@musteri_id", loggedInUserId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Tarih değeri string olarak alınıyor. Eğer 'tarih' bir stringse, GetString kullanabilirsiniz.
                                string date = reader.GetString("tarih");  // Eğer 'tarih' sütunu stringse
                                                                          // Eğer 'tarih' bir DateTime ise, şu şekilde de kullanabilirsiniz:
                                                                          // DateTime date = reader.GetDateTime("tarih");  

                                string time = reader.GetString("saat");
                                string status = reader.GetString("durum");
                                string type = reader.GetString("tur");
                                string client = reader.GetString("isim") + " " + reader.GetString("soyisim");

                                Lessons.Add(new Lesson
                                {
                                    Day = date,  // 'date' değişkeni buraya atandı
                                    Time = time,
                                    Status = status,
                                    Type = type,
                                    Client = client
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya uyarı
                Console.WriteLine("Veritabanı hatası: " + ex.Message);
            }
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
