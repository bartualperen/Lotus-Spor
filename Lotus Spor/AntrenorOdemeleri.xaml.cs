using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace Lotus_Spor;

public partial class AntrenorOdemeleri : ContentPage
{
    List<string> isimListesi = new List<string>();
    public ObservableCollection<Kisi> Customers { get; set; }
    public ObservableCollection<OdemeModel> OdemeListesi { get; set; }
    string antrenor, donem;
    public AntrenorOdemeleri()
	{
		InitializeComponent();

        OdemeListesi = new ObservableCollection<OdemeModel>();
        Customers = new ObservableCollection<Kisi>();
        kisilistele1();
        musterilistele();

        CustomerListView.ItemsSource = Customers;

        BindingContext = this;
    }
    private async void kisilistele1()
    {
        try
        {
            var connectionString = Database.GetConnection();
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string query = "SELECT isim, soyisim FROM yoneticiler";

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
                            string fullName = $"{isim} {soyisim}";
                            isimListesi.Add(fullName);
                            isimListesi.Remove(ConnectionString.admin2);
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
    private async void musterilistele()
    {
        try
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open(); // Ba�lant�n�n ba�ar�l� �ekilde a��ld���ndan emin olun

                // Temel sorgu
                string query = "SELECT isim, soyisim FROM yoneticiler WHERE 1=1";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Customers.Clear(); // Mevcut listeyi temizle

                        while (await reader.ReadAsync())
                        {
                            string isim = reader["isim"].ToString();
                            string soyisim = reader["soyisim"].ToString();
                            string fullName = $"{isim} {soyisim}";

                            // Listeye ekleme i�lemi
                            Customers.Add(new Kisi { Name = fullName });
                        }
                    }
                }
            }
            var kisi = Customers.FirstOrDefault(c => c.Name == ConnectionString.admin2);
            if (kisi != null)
            {
                Customers.Remove(kisi);
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while fetching data: " + ex.Message, "OK");
        }
    }
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedCustomer = e.SelectedItem as Kisi;

            antrenor = selectedCustomer.Name;

            OdemeListesiView.IsVisible = true;
            CustomerListView.IsVisible = false;
            ListeleButton.IsVisible = true;

            LoadOdemeBilgileri();
        }
    }
    private async void OnDonemChanged(object sender, EventArgs e)
    {
        donem = DonemPicker.SelectedItem?.ToString();
    }
    private async void OnListeleClicked(object sender, EventArgs e)
    {
        CustomerListView.IsVisible = true;
        OdemeListesiView.IsVisible = false;
        ListeleButton.IsVisible = false;
        CustomerListView.SelectedItem = null;
    }
    private async void LoadOdemeBilgileri()
    {
        string selectQuery = @"SELECT * FROM odeme_bilgileri WHERE antrenor = @antrenor ORDER BY odeme_donemi DESC;";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@antrenor", antrenor);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        OdemeListesi.Clear(); // Mevcut listeyi temizle

                        while (await reader.ReadAsync())
                        {
                            OdemeListesi.Add(new OdemeModel
                            {
                                antrenor = reader["antrenor"].ToString(),
                                odeme_donemi = reader["odeme_donemi"].ToString(),
                                toplam_odeme = Convert.ToDecimal(reader["toplam_odeme"]),
                                seans_ucreti = Convert.ToDecimal(reader["toplam_odeme"]),
                                yapilan_ders = Convert.ToInt32(reader["yapilan_seans_sayisi"]),
                                odeme_durumu = reader["odeme_durumu"].ToString()
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while loading payment information: " + ex.Message, "OK");
        }

    }
    private async void OnOdemeYapildiClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is OdemeModel odemeModel)
        {
            try
            {
                // OdemeModel'den al�nan verileri kullanarak i�lemleri ger�ekle�tirin
                using (MySqlConnection connection = Database.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                    UPDATE odeme_bilgileri 
                    SET odeme_durumu = '�dendi' 
                    WHERE antrenor = @antrenor AND odeme_donemi = @odeme_donemi";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@antrenor", odemeModel.antrenor);
                        command.Parameters.AddWithValue("@odeme_donemi", odemeModel.odeme_donemi);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            // Ba�ar�l� mesaj�
                            await DisplayAlert("Ba�ar�l�",
                                $"{odemeModel.antrenor} i�in �deme durumu g�ncellendi.",
                                "Tamam");

                            // UI g�ncelleme
                            odemeModel.odeme_durumu = "�dendi";
                            ReloadPage();
                        }
                        else
                        {
                            await DisplayAlert("Hata", "Kay�t bulunamad�.", "Tamam");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata olu�tu: {ex.Message}", "Tamam");
            }
        }
        else
        {
            await DisplayAlert("Hata", "Ba�l� bir �deme modeli bulunamad�.", "Tamam");
        }
    }
    private async void ReloadPage()
    {
        // Mevcut sayfay� kald�r�p tekrar y�klemek
        var currentPage = this;
        var parent = currentPage.Parent;

        if (parent is NavigationPage navigationPage)
        {
            // Sayfay� kald�r ve tekrar y�kle
            navigationPage.Navigation.RemovePage(currentPage);
            await navigationPage.Navigation.PushAsync(new AntrenorOdemeleri());
        }
        else
        {
            // Di�er durumlar i�in sayfay� yenilemek i�in uygun bir y�ntem se�in
        }
    }
}