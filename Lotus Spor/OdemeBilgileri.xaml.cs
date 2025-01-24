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
        isimListesi2.Insert(0, "T�m�");
        AntrenorPicker.ItemsSource = isimListesi2;
        CustomerListView.ItemsSource = Customers;
        
        if (KullaniciAdi == admin1 || KullaniciAdi == admin2)
        {
            AntrenorPicker.IsVisible = true;
            lblAntrenorPicker.IsVisible = true;
            AntrenorPicker.SelectedIndex = AntrenorPicker.Items.IndexOf("T�m�");
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
            // Veritaban� ba�lant�s�n� kontrol et
            using (var connection = Database.GetConnection())
            {
                connection.Open(); // Ba�lant�n�n ba�ar�l� �ekilde a��ld���ndan emin olun

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
                    if (selectedAntrenor != "T�m�")
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

                    if (AntrenorPicker.SelectedItem != null && AntrenorPicker.SelectedItem.ToString() != "T�m�")
                    {
                        command.Parameters.AddWithValue("@antrenor", AntrenorPicker.SelectedItem.ToString());
                    }

                    // Sorguyu �al��t�r ve sonucu kontrol et
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

        // CollectionView'� g�ncelle
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

                // Soyisim: Dizinin son eleman�
                string soyisim = nameParts.Length > 0 ? nameParts[^1] : "";

                // �sim: Soyisim hari� kalan k�s�m
                string isim = nameParts.Length > 1 ? string.Join(" ", nameParts[..^1]) : "";

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
                   COUNT(CASE WHEN s.durum = 'yap�ld�' THEN s.id END) AS yapilan_seans_sayisi,
                   COUNT(CASE WHEN s.durum = 'yap�lmad�' THEN s.id END) AS yapilmayan_seans_sayisi,
                   COUNT(CASE WHEN s.durum = 'beklemede' THEN s.id END) AS beklemede_seans_sayisi,
                   (LENGTH(m.seans_gunleri) - LENGTH(REPLACE(m.seans_gunleri, ',', '')) + 1) * 4 AS planlanan_aylik_seans_sayisi,
                   ROUND(
                       (m.seans_ucreti * COUNT(CASE WHEN s.durum = 'yap�ld�' THEN s.id END)) / 
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
                             AND ob.odeme_durumu != '�denmedi'
                       ) THEN (
                           SELECT ob.odeme_durumu
                           FROM odeme_bilgileri ob
                           WHERE ob.musteri_id = m.id 
                             AND ob.odeme_donemi = DATE_FORMAT(DATE_ADD(m.kayit_tarihi, INTERVAL (TIMESTAMPDIFF(MONTH, m.kayit_tarihi, s.tarih)) MONTH), '%Y-%m')
                       )
                       ELSE '�denmedi'
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
                (@antrenor = 'T�m�' OR antrenor = @antrenor)
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
                // OdemeModel'den al�nan verileri kullanarak i�lemleri ger�ekle�tirin
                using (MySqlConnection connection = Database.GetConnection())
                {
                    await connection.OpenAsync();

                    string query = @"
                    UPDATE odeme_bilgileri 
                    SET odeme_durumu = '�dendi' 
                    WHERE musteri_id = @musteri_id AND odeme_donemi = @odeme_donemi";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@musteri_id", odemeModel.musteri_id);
                        command.Parameters.AddWithValue("@odeme_donemi", odemeModel.odeme_donemi);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            // Ba�ar�l� mesaj�
                            await DisplayAlert("Ba�ar�l�",
                                $"{odemeModel.isim} {odemeModel.soyisim} i�in �deme durumu g�ncellendi.",
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
            await navigationPage.Navigation.PushAsync(new OdemeBilgileri());
        }
        else
        {
            // Di�er durumlar i�in sayfay� yenilemek i�in uygun bir y�ntem se�in
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