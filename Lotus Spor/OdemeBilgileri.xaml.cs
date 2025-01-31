using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Lotus_Spor;

public partial class OdemeBilgileri : ContentPage
{
    List<string> isimListesi = new List<string>();
    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    string searchName;
    public ObservableCollection<OdemeModel> MusteriListesi { get; set; }
    private int kullaniciId = -1;

    public OdemeBilgileri()
	{
		InitializeComponent();
        kisilistele();
        ResultsCollectionView.ItemsSource = filteredList;
        MusteriListesi = new ObservableCollection<OdemeModel>();
        CustomerListView.ItemsSource = MusteriListesi;
        VerileriGetir();

        BindingContext = this;
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

                // SQL komutunu çalýþtýr
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Verileri listeye ekle
                        while (reader.Read())
                        {
                            string isim = reader["isim"].ToString();
                            string soyisim = reader["soyisim"].ToString();
                            string fullName = $"{isim} {soyisim}";  // Ýsim ve soyisim birleþiyor
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
    private void VerileriGetir()
    {
        MusteriListesi.Clear();
        DonemPicker.Items.Clear();
        DonemPicker.Items.Add("Tümü");

        using (var conn = Database.GetConnection())
        {
            conn.Open();
            string query = "SELECT DISTINCT DATE_FORMAT(odeme_donemi, '%Y-%m') AS odeme_donemi FROM Odemeler ORDER BY odeme_donemi DESC";

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
        OdemeDurumuPicker.SelectedIndex = 0;

        // Listeyi güncelle
        MusteriListesi.Clear();
        foreach (var odeme in OdemeleriGetir())
        {
            MusteriListesi.Add(odeme);
        }

        var odemeler = OdemeleriGetir();

        foreach (var odeme in odemeler)
        {
            MusteriListesi.Add(odeme);
        }
    }
    public ObservableCollection<OdemeModel> OdemeleriGetir()
    {
        var odemeler = new ObservableCollection<OdemeModel>();
        using (var conn = Database.GetConnection())
        {
            conn.Open();
            string query = @"
            WITH RankedPayments AS (
                SELECT 
                    o.id, 
                    o.isim, 
                    o.soyisim, 
                    o.odeme_donemi, 
                    o.toplam_odeme, 
                    o.odeme_durumu,
                    ROW_NUMBER() OVER (PARTITION BY o.musteri_id, o.odeme_donemi ORDER BY o.id DESC) AS row_num
                FROM Odemeler o
                INNER JOIN musteriler m ON o.musteri_id = m.id
                WHERE o.odeme_donemi <= CURDATE()
            )
            SELECT id, isim, soyisim, odeme_donemi, toplam_odeme, odeme_durumu
            FROM RankedPayments
            WHERE row_num = 1
            ";

            // Filtreleme için parametreler
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            // Dönem Picker'ý filtreleme için kullan
            if (DonemPicker.SelectedItem != null && DonemPicker.SelectedItem.ToString() != "Tümü")
            {
                query += " AND DATE_FORMAT(odeme_donemi, '%Y-%m') = @odeme_donemi";
                parameters.Add(new MySqlParameter("@odeme_donemi", DonemPicker.SelectedItem.ToString()));
            }

            // Ödeme Durumu Picker'ý filtreleme için kullan
            if (OdemeDurumuPicker.SelectedItem != null && OdemeDurumuPicker.SelectedItem.ToString() != "Tümü")
            {
                query += " AND odeme_durumu = @odeme_durumu";
                parameters.Add(new MySqlParameter("@odeme_durumu", OdemeDurumuPicker.SelectedItem.ToString()));
            }

            // Arama kutusundaki isme göre filtreleme
            if (!string.IsNullOrEmpty(searchName))
            {
                query += " AND CONCAT(isim, ' ', soyisim) LIKE @searchName";
                parameters.Add(new MySqlParameter("@searchName", "%" + searchName + "%"));
            }

            query += " ORDER BY odeme_donemi ASC;";

            using (var cmd = new MySqlCommand(query, conn))
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    odemeler.Clear();
                    while (reader.Read())
                    {
                        var odemeDonemi = reader.GetDateTime("odeme_donemi").ToString("yyyy-MM");

                        odemeler.Add(new OdemeModel
                        {
                            musteri_id = reader.GetInt32("id"),
                            isim = reader.GetString("isim"),
                            soyisim = reader.GetString("soyisim"),
                            musteri_adi = reader.GetString("isim") + " " + reader.GetString("soyisim"),
                            odeme_donemi = odemeDonemi,
                            toplam_odeme = reader.GetDecimal("toplam_odeme"),
                            odeme_durumu = reader.GetString("odeme_durumu")
                        });
                    }
                }
            }
        }
        return odemeler;
    }
    private void OnFilterChanged(object sender, EventArgs e)
    {
        MusteriListesi.Clear();
        foreach (var odeme in OdemeleriGetir())
        {
            MusteriListesi.Add(odeme);
        }
    }
    private async void OnOdendiClicked(object sender, EventArgs e)
    {
        if (CustomerListView.SelectedItem is OdemeModel seciliMusteri)
        {
            using (MySqlConnection conn = Database.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Odemeler SET odeme_durumu='Ödendi' WHERE id=@id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", seciliMusteri.musteri_id);
                    cmd.ExecuteNonQuery();
                }
            }

            seciliMusteri.odeme_durumu = "Ödendi";
            CustomerListView.ItemsSource = null;
            CustomerListView.ItemsSource = MusteriListesi;

            lblDurum.Text = "Ödeme Durumu: Ödendi";
            await DisplayAlert("Baþarýlý", "Ödeme durumu güncellendi.", "Tamam");
            OnClosePanelClicked(null, null);
        }
    }
    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;
        //CustomerListView.IsVisible = false;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            CustomerListView.IsVisible = true;
            filteredList.Clear();
            return;
        }

        // Listeyi filtrele
        var suggestions = isimListesi
            .Where(isimSoyisim => isimSoyisim.ToLower().Contains(searchText))
            .ToList();

        // CollectionView'ý güncelle
        filteredList.Clear();
        foreach (var suggestion in suggestions)
        {
            filteredList.Add(suggestion);
        }
        ResultsCollectionView.IsVisible = filteredList.Count > 0;
        searchName = SearchEntry.Text;
        VerileriGetir();
    }
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            string selectedName = e.CurrentSelection[0] as string;

            if (!string.IsNullOrEmpty(selectedName))
            {
                SearchEntry.Text = selectedName;
                searchName = selectedName;
                //CustomerListView.IsVisible = false;
                VerileriGetir();
                GetKullaniciId(selectedName);
            }
        }
    }
    private void OnCustomerSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        CustomerListView.IsVisible = false;

        var selectedMusteri = (OdemeModel)e.CurrentSelection[0];

        lblIsim.Text = "Ýsim: " + selectedMusteri.isim + " " + selectedMusteri.soyisim;
        lblDonem.Text = "Dönem: " + selectedMusteri.odeme_donemi;
        lblTutar.Text = "Ödenecek Tutar: " + selectedMusteri.toplam_odeme + " TL";
        lblDurum.Text = "Ödeme Durumu: " + selectedMusteri.odeme_durumu;

        SelectedCustomerPanel.IsVisible = true;
    }
    private void OnClosePanelClicked(object sender, EventArgs e)
    {
        SelectedCustomerPanel.IsVisible = false;
        CustomerListView.SelectedItem = null;
        CustomerListView.IsVisible = true;
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

                // Soyisim: Dizinin son elemaný
                string soyisim = nameParts.Length > 0 ? nameParts[^1] : "";

                // Ýsim: Soyisim hariç kalan kýsým
                string isim = nameParts.Length > 1 ? string.Join(" ", nameParts[..^1]) : "";

                // Kullanýcýyý bulmak için sorgu
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
                        await DisplayAlert("Hata", "Kullanýcý bulunamadý.", "Tamam");
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