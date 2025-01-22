using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace Lotus_Spor;

public partial class OdemeBilgileri : ContentPage
{
    List<string> isimListesi = new List<string>();
    List<string> isimListesi2 = new List<string>();

    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    public ObservableCollection<Kisi> Customers { get; set; }
    public ObservableCollection<OdemeModel> OdemeListesi { get; set; }
    public Command KaydetCommand { get; set; }
    string searchName, antrenor, donem, customer;
    private int kullaniciId = -1;
    public OdemeBilgileri()
	{
		InitializeComponent();

        OdemeListesi = new ObservableCollection<OdemeModel>();
        Customers = new ObservableCollection<Kisi>();
        KaydetCommand = new Command(KaydetOdemeBilgileri);
        ResultsCollectionView.ItemsSource = filteredList;
        kisilistele();
        kisilistele1();
        musterilistele();
        isimListesi2.Insert(0, "Tümü");
        AntrenorPicker.ItemsSource = isimListesi2;
        CustomerListView.ItemsSource = Customers;
        AntrenorPicker.SelectedIndex = AntrenorPicker.Items.IndexOf("Tümü");

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
    private async void kisilistele1()
    {
        try
        {
            var connectionString = Database.GetConnection();
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                string query = "SELECT isim, soyisim FROM yoneticiler";

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
                            string fullName = $"{isim} {soyisim}";
                            isimListesi2.Add(fullName);
                            isimListesi2.Remove(ConnectionString.admin2);
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
            // Veritabaný baðlantýsýný kontrol et
            using (var connection = Database.GetConnection())
            {
                connection.Open(); // Baðlantýnýn baþarýlý þekilde açýldýðýndan emin olun

                // Temel sorgu
                string query = "SELECT isim, soyisim FROM musteriler WHERE 1=1";

                // Dinamik sorgu eklemeleri
                if (!string.IsNullOrEmpty(searchName))
                {
                    query += " AND CONCAT(isim, ' ', soyisim) LIKE @searchName";
                }

                if (AntrenorPicker.SelectedIndex != -1)
                {
                    string selectedAntrenor = AntrenorPicker.SelectedItem.ToString();
                    if (selectedAntrenor != "Tümü")
                    {
                        query += " AND EXISTS (SELECT 1 FROM seanslar s WHERE s.musteri_id = musteriler.id AND s.antrenor = @antrenor)";
                    }
                }

                using (var command = new MySqlCommand(query, connection))
                {
                    // Parametreleri kontrol et ve ekle
                    if (!string.IsNullOrEmpty(searchName))
                    {
                        command.Parameters.AddWithValue("@searchName", "%" + searchName + "%");
                    }
                    if (AntrenorPicker.SelectedItem != null && AntrenorPicker.SelectedItem.ToString() != "Tümü")
                    {
                        command.Parameters.AddWithValue("@antrenor", AntrenorPicker.SelectedItem.ToString());
                    }

                    // Sorguyu çalýþtýr ve sonucu kontrol et
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Customers.Clear(); // Mevcut listeyi temizle

                        while (await reader.ReadAsync())
                        {
                            string isim = reader["isim"].ToString();
                            string soyisim = reader["soyisim"].ToString();
                            string fullName = $"{isim} {soyisim}";

                            // Listeye ekleme iþlemi
                            Customers.Add(new Kisi { Name = fullName });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while fetching data: " + ex.Message, "OK");
        }

    }
    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            ResultsCollectionView.IsVisible = false;
            OdemeListesiView.IsVisible = false;
            CustomerListView.IsVisible = true;
            ListeleButton.IsVisible = false;
            KaydetButton.IsVisible = false;
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
        antrenor = AntrenorPicker.SelectedItem?.ToString();

        searchName = SearchEntry.Text;
    }
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            string selectedName = e.CurrentSelection[0] as string;

            if (!string.IsNullOrEmpty(selectedName))
            {
                SearchEntry.Text = selectedName;
                ResultsCollectionView.IsVisible = false;
                OdemeListesiView.IsVisible = true;
                KaydetButton.IsVisible = true;
                CustomerListView.IsVisible = false;
                ListeleButton.IsVisible = true;
                GetKullaniciId(selectedName);
                LoadOdemeBilgileri(searchName, antrenor);
            }
        }
    }
    private async void OnAntrenorChanged(object sender, EventArgs e)
    {
        searchName = SearchEntry.Text;
        antrenor = AntrenorPicker.SelectedItem?.ToString();
        donem = DonemPicker.SelectedItem?.ToString();

        musterilistele();
    }
    private async void OnDonemChanged(object sender, EventArgs e)
    {
        searchName = SearchEntry.Text;
        antrenor = AntrenorPicker.SelectedItem?.ToString();
        donem = DonemPicker.SelectedItem?.ToString();
    }
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedCustomer = e.SelectedItem as Kisi;

            customer = selectedCustomer.Name;

            OdemeListesiView.IsVisible = true;
            CustomerListView.IsVisible = false;
            KaydetButton.IsVisible = true;
            ListeleButton.IsVisible = true;

            LoadOdemeBilgileri(customer, antrenor);
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
    private async void OnListeleClicked(object sender, EventArgs e)
    {
        CustomerListView.IsVisible = true;
        OdemeListesiView.IsVisible = false;
        KaydetButton.IsVisible = false;
        ListeleButton.IsVisible = false;
    }
    private async void LoadOdemeBilgileri(string searchName = "", string antrenor = "")
    {
        string query = @"SELECT 
                m.id AS musteri_id,
                m.isim,
                m.soyisim,
                m.kayit_tarihi,
                m.seans_ucreti,
                m.seans_gunleri,
                DATE_FORMAT(DATE_ADD(m.kayit_tarihi, INTERVAL (TIMESTAMPDIFF(MONTH, m.kayit_tarihi, s.tarih)) MONTH), '%Y-%m') AS odeme_donemi,
                COUNT(CASE WHEN s.durum = 'yapýldý' THEN s.id END) AS yapilan_seans_sayisi,
                COUNT(CASE WHEN s.durum = 'yapýlmadý' THEN s.id END) AS yapilmayan_seans_sayisi,
                COUNT(CASE WHEN s.durum = 'beklemede' THEN s.id END) AS beklemede_seans_sayisi,
                (LENGTH(m.seans_gunleri) - LENGTH(REPLACE(m.seans_gunleri, ',', '')) + 1) * 4 AS planlanan_aylik_seans_sayisi, -- Haftalýk günlerin sayýsý x 4 (aylýk)
                ROUND(
                    (m.seans_ucreti * COUNT(CASE WHEN s.durum = 'yapýldý' THEN s.id END)) /
                    GREATEST((LENGTH(m.seans_gunleri) - LENGTH(REPLACE(m.seans_gunleri, ',', '')) + 1) * 4, 1),
                    2
                ) AS toplam_odeme,
                s.antrenor
            FROM 
                musteriler AS m
            LEFT JOIN 
                seanslar AS s
            ON 
                m.id = s.musteri_id
            WHERE 
                s.tarih <= NOW()
                AND (@antrenor = 'Tümü' OR s.antrenor = @antrenor)
                AND CONCAT(m.isim, ' ', m.soyisim) LIKE @searchName
            GROUP BY 
                m.id, odeme_donemi, s.antrenor
            ORDER BY 
                m.id, odeme_donemi;";


        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    // Antrenör ve isim parametrelerini ekle
                    command.Parameters.AddWithValue("@antrenor", antrenor);
                    command.Parameters.AddWithValue("@searchName", string.IsNullOrEmpty(searchName) ? "" : $"%{searchName}%");

                    using (var reader = command.ExecuteReader())
                    {
                        OdemeListesi.Clear(); // Mevcut listeyi temizle

                        while (reader.Read())
                        {
                            OdemeListesi.Add(new OdemeModel
                            {
                                musteri_id = Convert.ToInt32(reader["musteri_id"]),
                                isim = reader["isim"].ToString(),
                                soyisim = reader["soyisim"].ToString(),
                                odeme_donemi = reader["odeme_donemi"].ToString(),
                                toplam_odeme = Convert.ToDecimal(reader["toplam_odeme"]),
                                seans_ucreti = Convert.ToDecimal(reader["seans_ucreti"]),
                                yapilan_ders = Convert.ToInt32(reader["yapilan_seans_sayisi"]),
                                yapilmayan_ders = Convert.ToInt32(reader["yapilmayan_seans_sayisi"]),
                                beklenen_ders = Convert.ToInt32(reader["beklemede_seans_sayisi"]),
                                odeme_durumu = "beklemede" // Ödeme durumu örnek olarak sabitlenmiþtir
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "An error occurred while loading payment information: " + ex.Message, "OK");
        }
    }
    private void KaydetOdemeBilgileri()
    {
        DisplayAlert("Baþarýlý", "Ödeme bilgileri kaydedildi!", "Tamam");
    }
}
public class OdemeModel
{
    public int musteri_id { get; set; }
    public string isim { get; set; }
    public string soyisim { get; set; }
    public string odeme_donemi { get; set; }
    public decimal toplam_odeme { get; set; }
    public decimal seans_ucreti { get; set; }
    public string odeme_durumu { get; set; }
    public int yapilan_ders { get; set; }
    public int yapilmayan_ders { get; set; }
    public int beklenen_ders { get; set; }
}
public class Kisi
{
    public string Name { get; set; }
}