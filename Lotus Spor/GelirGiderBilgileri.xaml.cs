using MySql.Data.MySqlClient;

namespace Lotus_Spor;

public partial class GelirGiderBilgileri : ContentPage
{
	public GelirGiderBilgileri()
	{
		InitializeComponent();
        DonemleriGetir();

    }
	private async void LoadGelir()
	{

    }
    private async void DonemleriGetir()
    {
        try
        {
            //MusteriListesi.Clear();
            DonemPicker.Items.Clear();
            DonemPicker.Items.Add("Tümü");

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = @"SET lc_time_names = 'tr_TR';
                                SELECT DISTINCT DATE_FORMAT(odeme_donemi, '%Y %M') AS odeme_donemi, YEAR(odeme_donemi) AS yil, MONTH(odeme_donemi) AS ay FROM Odemeler ORDER BY yil DESC, ay DESC;";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DonemPicker.Items.Add(reader.GetString("odeme_donemi"));
                        }
                    }
                }
            }

            DonemPicker.SelectedIndex = 0;
            //OdemeDurumuPicker.SelectedIndex = 0;

            //// Listeyi güncelle
            //MusteriListesi.Clear();
            //foreach (var odeme in await OdemeleriGetir(currentPage))
            //{
            //    MusteriListesi.Add(odeme);
            //}
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler getirilirken bir hata oluþtu: \n {ex.Message}", "Tamam");
        }
    }
}