using CommunityToolkit.Maui.Views;
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
    private int currentPage = 1; // Baþlangýç sayfasý
    private int recordsPerPage = 10; // Her sayfada gösterilecek kayýt sayýsý
    private int totalRecords = 0; // Toplam kayýt sayýsý
    private int totalPages = 0; // Toplam sayfa sayýsý
    public bool IsPreviousEnabled => currentPage > 1;
    public bool IsNextEnabled => currentPage < totalPages;

    public OdemeBilgileri()
	{
		InitializeComponent();
        kisilistele();
        ResultsCollectionView.ItemsSource = filteredList;
        MusteriListesi = new ObservableCollection<OdemeModel>();
        CustomerListView.ItemsSource = MusteriListesi;
        VerileriGetir();
        UpdatePageButtons();
        GetTotalRecords();

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
                string query = "SELECT isim, soyisim FROM musteriler WHERE aktiflik = 'Aktif'";

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
            await DisplayAlert("Hata", $"Veriler yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
    }
    private async void VerileriGetir()
    {
        try
        {
            MusteriListesi.Clear();
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
            OdemeDurumuPicker.SelectedIndex = 0;

            // Listeyi güncelle
            MusteriListesi.Clear();
            foreach (var odeme in await OdemeleriGetir(currentPage))
            {
                MusteriListesi.Add(odeme);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Veriler getirilirken bir hata oluþtu: \n {ex.Message}", "Tamam");
        }
    }
    public async Task<ObservableCollection<OdemeModel>> OdemeleriGetir(int page)
    {
        var odemeler = new ObservableCollection<OdemeModel>();
        int offset = (page - 1) * recordsPerPage;

        using (var conn = Database.GetConnection())
        {
            conn.Open();
            string query = @"
            SET lc_time_names = 'tr_TR';
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
                WHERE o.odeme_donemi <= CURDATE() AND m.aktiflik = 'Aktif'
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
                query += " AND DATE_FORMAT(odeme_donemi, '%Y %M') = @odeme_donemi";
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

            query += " ORDER BY odeme_donemi DESC LIMIT @limit OFFSET @offset;";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@limit", recordsPerPage);
                cmd.Parameters.AddWithValue("@offset", offset);

                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }

                using (var reader = cmd.ExecuteReader())
                {
                    odemeler.Clear();
                    while (reader.Read())
                    {
                        var odemeDonemi = reader.GetDateTime("odeme_donemi").ToString("yyyy MMMM");
                        var odemeTarihi = reader.GetDateTime("odeme_donemi").ToString("dd MMMM yyyy");

                        odemeler.Add(new OdemeModel
                        {
                            musteri_id = reader.GetInt32("id"),
                            isim = reader.GetString("isim"),
                            soyisim = reader.GetString("soyisim"),
                            musteri_adi = reader.GetString("isim") + " " + reader.GetString("soyisim"),
                            odeme_donemi = odemeDonemi,
                            toplam_odeme = reader.GetDecimal("toplam_odeme"),
                            odeme_durumu = reader.GetString("odeme_durumu"),
                            odeme_tarihi = odemeTarihi
                        });
                    }
                }
            }
        }
        return odemeler;
    }
    private void GetTotalRecords()
    {
        using (var conn = Database.GetConnection())
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM Odemeler";
            using (var cmd = new MySqlCommand(query, conn))
            {
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar());
                totalPages = (int)Math.Ceiling((double)totalRecords / recordsPerPage);
            }
        }
    }
    private async void OnFilterChanged(object sender, EventArgs e)
    {
        MusteriListesi.Clear();
        foreach (var odeme in await OdemeleriGetir(currentPage))
        {
            MusteriListesi.Add(odeme);
        }
        currentPage = 1;
        UpdatePageButtons();
        GetTotalRecords();
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

        if (!string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = true;
        }
        else if (string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = false;
        }

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
    }
    private async void OnClearButtonClicked(object sender, EventArgs e)
    {
        SearchEntry.Text = string.Empty;
        searchName = string.Empty;
        btnClear.IsVisible = false;

        currentPage = 1;
        UpdatePageButtons();
        GetTotalRecords();
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
                ResultsCollectionView.IsVisible = false;
                ResultsCollectionView.SelectedItem = string.Empty;
                VerileriGetir();
                currentPage = 1;
                UpdatePageButtons();
                GetTotalRecords();
                GetKullaniciId(selectedName);
            }
        }
    }
    private void OnCustomerSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;
        CustomerListView.IsVisible = false;
        SearchEntry.IsVisible = false;
        lblSearchEntry.IsVisible = false;
        DonemPicker.IsVisible = false;
        lblDonemPicker.IsVisible = false;
        OdemeDurumuPicker.IsVisible = false;
        lblOdemeDurumuPicker.IsVisible = false;
        btnOnceki.IsVisible = false;
        SayfaButtons.IsVisible = false;
        btnSonraki.IsVisible = false;
        btnBas.IsVisible = false;
        btnClear.IsVisible = false;
        ResultsCollectionView.IsVisible = false;

        var selectedMusteri = (OdemeModel)e.CurrentSelection[0];

        lblIsim.Text = "Ýsim: " + selectedMusteri.isim + " " + selectedMusteri.soyisim;
        lblDonem.Text = "Dönem: " + selectedMusteri.odeme_donemi;
        lblTutar.Text = "Ödenecek Tutar: " + selectedMusteri.toplam_odeme + " TL";
        lblDurum.Text = "Ödeme Durumu: " + selectedMusteri.odeme_durumu;

        SelectedCustomerPanel.IsVisible = true;
    }
    private async void OnClosePanelClicked(object sender, EventArgs e)
    {
        btnOnceki.IsVisible = true;
        SayfaButtons.IsVisible = true;
        btnSonraki.IsVisible = true;
        btnBas.IsVisible = true;
        SearchEntry.IsVisible = true;
        lblSearchEntry.IsVisible = true;
        DonemPicker.IsVisible = true;
        lblDonemPicker.IsVisible = true;
        OdemeDurumuPicker.IsVisible = true;
        lblOdemeDurumuPicker.IsVisible = true;
        SelectedCustomerPanel.IsVisible = false;
        CustomerListView.SelectedItem = null;
        CustomerListView.IsVisible = true;
        SearchEntry.Text = string.Empty;
        searchName = SearchEntry.Text;

        VerileriGetir();
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
    private void UpdatePageButtons()
    {
        OnPropertyChanged(nameof(IsPreviousEnabled));
        OnPropertyChanged(nameof(IsNextEnabled));
    }
    private async void OnNextPageClicked(object sender, EventArgs e)
    {
        if (currentPage < totalPages)
        {
            currentPage++;
            MusteriListesi.Clear();
            foreach (var odeme in await OdemeleriGetir(currentPage))
            {
                MusteriListesi.Add(odeme);
            }
        }
        UpdatePageButtons();
    }
    private async void OnPreviousPageClicked(object sender, EventArgs e)
    {
        if (currentPage > 1)
        {
            currentPage--;
            MusteriListesi.Clear();
            foreach (var odeme in await OdemeleriGetir(currentPage))
            {
                MusteriListesi.Add(odeme);
            }
        }
        UpdatePageButtons();
    }
    private async void OnReturnToStart(object sender, EventArgs e)
    {
        if (currentPage > 1)
        {
            currentPage = 1;
            MusteriListesi.Clear();
            foreach (var odeme in await OdemeleriGetir(currentPage))
            {
                MusteriListesi.Add(odeme);
            }
        }
        UpdatePageButtons();
    }
}