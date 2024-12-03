using System.Collections.ObjectModel;
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

            // Dinamik listeyi dolduruyoruz
            Lessons = new ObservableCollection<Lesson>
            {
                new Lesson { Day = "Pazartesi", Time = "10:00 - 11:00", Description = "Reformer Pilates (Düet: Hümeyra Öztürk ile)" },
                new Lesson { Day = "Salı", Time = "14:00 - 15:00", Description = "Yoga (Grup)" },
                new Lesson { Day = "Çarşamba", Time = "16:00 - 17:00", Description = "PT (Bireysel)" }
            };

            // Sayfanın BindingContext'ine verileri bağla
            BindingContext = this;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            string title = LoginManager.Gender.ToLower() == "erkek" ? "Bey" : "Hanım";
            WelcomeLabel.Text = $"Hoş Geldiniz, {LoginManager.LoggedInUser} {title}!";
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
    }

    public class Lesson
    {
        public string Day { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
    }
}
