using System.Collections.ObjectModel;

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
    }

    public class Lesson
    {
        public string Day { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
    }
}
