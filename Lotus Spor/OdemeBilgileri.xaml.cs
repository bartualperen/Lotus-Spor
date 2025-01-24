using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Lotus_Spor;

public partial class OdemeBilgileri : ContentPage
{
    List<string> isimListesi = new List<string>();
    List<string> isimListesi2 = new List<string>();

    private string admin1 = ConnectionString.admin1, admin2 = ConnectionString.admin2;
    string KullaniciAdi = Preferences.Get("LoggedInUser", string.Empty) + " " + Preferences.Get("LoggedInUser2", string.Empty);

    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    public ObservableCollection<Kisi> Customers { get; set; }
    public ObservableCollection<OdemeModel> OdemeListesi { get; set; }
    string searchName, antrenor, donem, customer;
    private int kullaniciId = -1;
    public OdemeBilgileri()
	{
		InitializeComponent();

        OdemeListesi = new ObservableCollection<OdemeModel>();
        Customers = new ObservableCollection<Kisi>();
        ResultsCollectionView.ItemsSource = filteredList;
        kisilistele();
        kisilistele1();
        musterilistele();
        isimListesi2.Insert(0, "Tümü");
        AntrenorPicker.ItemsSource = isimListesi2;
        CustomerListView.ItemsSource = Customers;
        
        if (KullaniciAdi == admin1 || KullaniciAdi == admin2)
        {
            AntrenorPicker.IsVisible = true;
            lblAntrenorPicker.IsVisible = true;
            AntrenorPicker.SelectedIndex = AntrenorPicker.Items.IndexOf("Tümü");
        }
        else if (KullaniciAdi != admin1 || KullaniciAdi != admin2)
        {

            AntrenorPicker.SelectedIndex = AntrenorPicker.Items.IndexOf(KullaniciAdi);
        }
        SaveOdemeBilgileri();

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
        CustomerListView.IsVisible = false;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            ResultsCollectionView.IsVisible = false;
            OdemeListesiView.IsVisible = false;
            CustomerListView.IsVisible = true;
            ListeleButton.IsVisible = false;
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
        ListeleButton.IsVisible = false;
    }
    private async void SaveOdemeBilgileri()
    {
        string insertQuery = @"
               SET sql_mode = (SELECT REPLACE(@@sql_mode, 'ONLY_FULL_GROUP_BY', ''));
               REPLACE INTO odeme_bilgileri (
                   musteri_id,
                   isim,
                   soyisim,
                   kayit_tarihi,
                   seans_ucreti,
                   seans_gunleri,
                   odeme_donemi,
                   yapilan_seans_sayisi,
                   yapilmayan_seans_sayisi,
                   beklemede_seans_sayisi,
                   planlanan_aylik_seans_sayisi,
                   toplam_odeme,
                   antrenor,
                   odeme_durumu
               )
               SELECT 
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
                   (LENGTH(m.seans_gunleri) - LENGTH(REPLACE(m.seans_gunleri, ',', '')) + 1) * 4 AS planlanan_aylik_seans_sayisi,
                   ROUND(
                       (m.seans_ucreti * COUNT(CASE WHEN s.durum = 'yapýldý' THEN s.id END)) / 
                       GREATEST((LENGTH(m.seans_gunleri) - LENGTH(REPLACE(m.seans_gunleri, ',', '')) + 1) * 4, 1),
                       2
                   ) AS toplam_odeme,
                   s.antrenor,
                   CASE
                       WHEN EXISTS (
                           SELECT 1 
                           FROM odeme_bilgileri ob 
                           WHERE ob.musteri_id = m.id 
                             AND ob.odeme_donemi = DATE_FORMAT(DATE_ADD(m.kayit_tarihi, INTERVAL (TIMESTAMPDIFF(MONTH, m.kayit_tarihi, s.tarih)) MONTH), '%Y-%m')
                             AND ob.odeme_durumu != 'Ödenmedi'
                       ) THEN (
                           SELECT ob.odeme_durumu
                           FROM odeme_bilgileri ob
                           WHERE ob.musteri_id = m.id 
                             AND ob.odeme_donemi = DATE_FORMAT(DATE_ADD(m.kayit_tarihi, INTERVAL (TIMESTAMPDIFF(MONTH, m.kayit_tarihi, s.tarih)) MONTH), '%Y-%m')
                       )
                       ELSE 'Ödenmedi'
                   END AS odeme_durumu
               FROM 
                   musteriler AS m
               LEFT JOIN 
                   seanslar AS s
               ON 
                   m.id = s.musteri_id
               WHERE 
                   s.tarih <= NOW()
               GROUP BY 
                   m.id, odeme_donemi, s.antrenor;
        ";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(insertQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error occurred while saving payment data: " + ex.Message, "OK");
        }

    }
    private async void LoadOdemeBilgileri(string searchName = "", string antrenor = "")
    {
        string selectQuery = @"
            SELECT 
                musteri_id,
                isim,
                soyisim,
                kayit_tarihi,
                seans_ucreti,
                odeme_donemi,
                toplam_odeme,
                yapilan_seans_sayisi,
                yapilmayan_seans_sayisi,
                beklemede_seans_sayisi,
                antrenor,
                odeme_durumu
            FROM odeme_bilgileri
            WHERE 
                (@antrenor = 'Tümü' OR antrenor = @antrenor)
                AND CONCAT(isim, ' ', soyisim) LIKE @searchName
            ORDER BY odeme_donemi DESC;
        ";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@antrenor", antrenor);
                    command.Parameters.AddWithValue("@searchName", string.IsNullOrEmpty(searchName) ? "" : $"%{searchName}%");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        OdemeListesi.Clear(); // Mevcut listeyi temizle

                        while (await reader.ReadAsync())
                        {
                            OdemeListesi.Add(new OdemeModel
                            {
                                musteri_id = Convert.ToInt32(reader["musteri_id"]),
                                isim = reader["isim"].ToString(),
                                soyisim = reader["soyisim"].ToString(),
                                antrenor = reader["antrenor"].ToString(),
                                odeme_donemi = reader["odeme_donemi"].ToString(),
                                kayit_tarihi = reader["kayit_tarihi"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["kayit_tarihi"]).ToString("dd-MM-yyyy") : "Bilinmiyor",
                                toplam_odeme = Convert.ToDecimal(reader["toplam_odeme"]),
                                seans_ucreti = Convert.ToDecimal(reader["seans_ucreti"]),
                                yapilan_ders = Convert.ToInt32(reader["yapilan_seans_sayisi"]),
                                yapilmayan_ders = Convert.ToInt32(reader["yapilmayan_seans_sayisi"]),
                                beklenen_ders = Convert.ToInt32(reader["beklemede_seans_sayisi"]),
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
                // OdemeModel'den alýnan verileri kullanarak iþlemleri gerçekleþtirin
                using (MySqlConnection connection = Database.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                    UPDATE odeme_bilgileri 
                    SET odeme_durumu = 'Ödendi' 
                    WHERE musteri_id = @musteri_id AND odeme_donemi = @odeme_donemi";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@musteri_id", odemeModel.musteri_id);
                        command.Parameters.AddWithValue("@odeme_donemi", odemeModel.odeme_donemi);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            // Baþarýlý mesajý
                            await DisplayAlert("Baþarýlý",
                                $"{odemeModel.isim} {odemeModel.soyisim} için ödeme durumu güncellendi.",
                                "Tamam");

                            // UI güncelleme
                            odemeModel.odeme_durumu = "Ödendi";
                            ReloadPage();
                        }
                        else
                        {
                            await DisplayAlert("Hata", "Kayýt bulunamadý.", "Tamam");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata oluþtu: {ex.Message}", "Tamam");
            }
        }
        else
        {
            await DisplayAlert("Hata", "Baðlý bir ödeme modeli bulunamadý.", "Tamam");
        }
    }
    private async void ReloadPage()
    {
        // Mevcut sayfayý kaldýrýp tekrar yüklemek
        var currentPage = this;
        var parent = currentPage.Parent;

        if (parent is NavigationPage navigationPage)
        {
            // Sayfayý kaldýr ve tekrar yükle
            navigationPage.Navigation.RemovePage(currentPage);
            await navigationPage.Navigation.PushAsync(new OdemeBilgileri());
        }
        else
        {
            // Diðer durumlar için sayfayý yenilemek için uygun bir yöntem seçin
        }
    }

}
public class OdemeModel
{
    public int musteri_id { get; set; }
    public string isim { get; set; }
    public string soyisim { get; set; }
    public string antrenor { get; set; }
    public string odeme_donemi { get; set; }
    public string kayit_tarihi { get; set; }
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