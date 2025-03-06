using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;

namespace Lotus_Spor;

public partial class OlcuEkle : ContentPage
{
    List<string> isimListesi = new List<string>();
    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    private int kullaniciId = -1;
    public OlcuEkle()
	{
		InitializeComponent();
        ResultsCollectionView.ItemsSource = filteredList;
        kisilistele();
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
    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (kullaniciId == -1)
        {
            await DisplayAlert("Hata", "L�tfen bir kullan�c� se�in.", "Tamam");
            return;
        }

        //if (string.IsNullOrWhiteSpace(KiloEntry.Text) ||
        //    string.IsNullOrWhiteSpace(YagOraniEntry.Text) ||
        //    string.IsNullOrWhiteSpace(SuOraniEntry.Text) ||
        //    string.IsNullOrWhiteSpace(OmuzOlcusuEntry.Text) ||
        //    string.IsNullOrWhiteSpace(BicepsOlcusuEntry.Text) ||
        //    string.IsNullOrWhiteSpace(GogusOlcusuEntry.Text) ||
        //    string.IsNullOrWhiteSpace(BelOlcusuEntry.Text) ||
        //    string.IsNullOrWhiteSpace(KarinOlcusuEntry.Text) ||
        //    string.IsNullOrWhiteSpace(KaclaOlcusuEntry.Text) ||
        //    string.IsNullOrWhiteSpace(BacakOlcusuEntry.Text) ||
        //    string.IsNullOrWhiteSpace(KalfOlcusuEntry.Text))
        //{
        //    await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurun.", "Tamam");
        //    return;
        //}

        // Burada �l��leri i�leyebilirsiniz
        decimal kilo = Convert.ToDecimal(KiloEntry.Text);
        decimal yagOrani = Convert.ToDecimal(YagOraniEntry.Text);
        decimal suOrani = Convert.ToDecimal(SuOraniEntry.Text);
        decimal omuzOlcusu = Convert.ToDecimal(OmuzOlcusuEntry.Text);
        decimal bicepsOlcusu = Convert.ToDecimal(BicepsOlcusuEntry.Text);
        decimal gogusOlcusu = Convert.ToDecimal(GogusOlcusuEntry.Text);
        decimal belOlcusu = Convert.ToDecimal(BelOlcusuEntry.Text);
        decimal karinOlcusu = Convert.ToDecimal(KarinOlcusuEntry.Text);
        decimal kaclaOlcusu = Convert.ToDecimal(KaclaOlcusuEntry.Text);
        decimal bacakOlcusu = Convert.ToDecimal(BacakOlcusuEntry.Text);
        decimal kalfOlcusu = Convert.ToDecimal(KalfOlcusuEntry.Text);

        try
        {
            var connectionString = Database.GetConnection();
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO VucutBilgileri (KullaniciId, Kilo, YagOrani, SuOrani, Omuz, Biceps, Gogus, Bel, Karin, Kalca, Bacak, Kalf) " +
                               "VALUES (@kullaniciId, @kilo, @yagOrani, @suOrani, @omuzOlcusu, @bicepsOlcusu, @gogusOlcusu, @belOlcusu, @karinOlcusu, @kaclaOlcusu, @bacakOlcusu, @kalfOlcusu)";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                    command.Parameters.AddWithValue("@kilo", kilo);
                    command.Parameters.AddWithValue("@yagOrani", yagOrani);
                    command.Parameters.AddWithValue("@suOrani", suOrani);
                    command.Parameters.AddWithValue("@omuzOlcusu", omuzOlcusu);
                    command.Parameters.AddWithValue("@bicepsOlcusu", bicepsOlcusu);
                    command.Parameters.AddWithValue("@gogusOlcusu", gogusOlcusu);
                    command.Parameters.AddWithValue("@belOlcusu", belOlcusu);
                    command.Parameters.AddWithValue("@karinOlcusu", karinOlcusu);
                    command.Parameters.AddWithValue("@kaclaOlcusu", kaclaOlcusu);
                    command.Parameters.AddWithValue("@bacakOlcusu", bacakOlcusu);
                    command.Parameters.AddWithValue("@kalfOlcusu", kalfOlcusu);

                    command.ExecuteNonQuery();
                }
            }
            await DisplayAlert("Ba�ar�l�", "�l��ler ba�ar�yla kaydedildi.", "Tamam");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while saving data: " + ex.Message, "OK");
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
}