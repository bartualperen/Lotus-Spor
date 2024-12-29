using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace Lotus_Spor;

public partial class UyeListesi : ContentPage
{
    List<string> isimListesi = new List<string>();
    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    public ObservableCollection<Customer> Customers { get; set; }

    public class Kisi
    {
        public string Name { get; set; }
    }
    public UyeListesi()
	{
		InitializeComponent();
        Customers = new ObservableCollection<Customer>();
        CustomerListView.ItemsSource = Customers;

        // Load data from database
        LoadCustomers();
    }
    private async void LoadCustomers()
    {
        string query = "SELECT isim, soyisim, hizmet_turu, seans_ucreti, notlar, kayit_tarihi FROM musteriler";

        try
        {
            using (var connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        Customers.Clear();

                        while (await reader.ReadAsync())
                        {
                            Customers.Add(new Customer
                            {
                                FullName = $"{reader["isim"]} {reader["soyisim"]}",
                                AdditionalInfo = reader["hizmet_turu"]?.ToString() ?? "Bilinmiyor",
                                Notlar = reader["notlar"]?.ToString() ?? "Bilinmiyor",
                                KayitTarihi = reader["kayit_tarihi"] != DBNull.Value
                                ? Convert.ToDateTime(reader["kayit_tarihi"]).ToString("dd-MM-yyyy") : "Bilinmiyor",
                                Ucret = Convert.ToInt32((reader["seans_ucreti"]))
                            });
                        }
                    }
                }
            }
            TotalMembersLabel.Text = $"Toplam Üye Sayýsý: {Customers.Count}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Müþteri verileri yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
    }
    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchQuery = e.NewTextValue?.ToLower() ?? string.Empty;
        CustomerListView.ItemsSource = string.IsNullOrEmpty(searchQuery)
            ? Customers
            : new ObservableCollection<Customer>(
                Customers.Where(c => c.FullName.ToLower().Contains(searchQuery)));
    }
    private void OnCancelEditClicked(object sender, EventArgs e)
    {
        //EditPanel.IsVisible = false; // Düzenleme panelini kapat
    }
    private async void OnSaveEditClicked(object sender, EventArgs e)
    {
    //    if (EditPanel.BindingContext is Customer editedCustomer)
    //    {
    //        // Düzenlenmiþ müþteri bilgilerini veritabanýna veya API'ye kaydet
    //        bool success = await SaveCustomerToDatabaseAsync(editedCustomer);

    //        if (success)
    //        {
    //            await DisplayAlert("Baþarýlý", "Müþteri bilgileri baþarýyla güncellendi.", "Tamam");
    //        }
    //        else
    //        {
    //            await DisplayAlert("Hata", "Müþteri bilgileri güncellenemedi. Lütfen tekrar deneyin.", "Tamam");
    //        }

    //        // Paneli kapat
    //        EditPanel.IsVisible = false;
    //    }
    }
    //private async Task<bool> SaveCustomerToDatabaseAsync(Customer customer)
    //{
    //    string query = @"
    //        UPDATE musteriler 
    //        SET 
    //            isim = @isim, 
    //            soyisim = @soyisim, 
    //            telefon = @telefon, 
    //            hizmet_turu = @hizmet_turu, 
    //            seans_ucreti = @seans_ucreti, 
    //            notlar = @notlar, 
    //            grup = @grup 
    //        WHERE CONCAT(isim, ' ', soyisim) = @clientName";

    //    try
    //    {
    //        using (MySqlConnection connection = Database.GetConnection())
    //        {
    //            await connection.OpenAsync();

    //            using (MySqlCommand command = new MySqlCommand(query, connection))
    //            {
    //                string fullName = isimentry.Text;
    //                string[] nameParts = fullName.Split(' ');
    //                string isim = nameParts[0];
    //                string soyisim = nameParts.Length > 1 ? nameParts[1] : "";
    //                // Parametreleri ekle
    //                command.Parameters.AddWithValue("@isim", isim);
    //                command.Parameters.AddWithValue("@soyisim", soyisim);
    //                command.Parameters.AddWithValue("@telefon", telefonentry.Text.ToString());
    //                command.Parameters.AddWithValue("@hizmet_turu", SeansPicker.SelectedItem?.ToString());
    //                command.Parameters.AddWithValue("@seans_ucreti", updatedCustomer.SeansUcreti);
    //                command.Parameters.AddWithValue("@notlar", updatedCustomer.Notlar);
    //                command.Parameters.AddWithValue("@grup", updatedCustomer.Grup);
    //                command.Parameters.AddWithValue("@clientName", originalClientName);

    //                // Sorguyu çalýþtýr
    //                int rowsAffected = await command.ExecuteNonQueryAsync();

    //                // Güncellemenin baþarýlý olup olmadýðýný kontrol et
    //                if (rowsAffected > 0)
    //                {
    //                    Console.WriteLine("Müþteri bilgileri baþarýyla güncellendi.");
    //                }
    //                else
    //                {
    //                    Console.WriteLine("Müþteri bilgileri güncellenemedi. Belirtilen müþteri bulunamadý.");
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Bir hata oluþtu: {ex.Message}");
    //        throw; // Hata fýrlatýlabilir veya daha iyi bir hata iþleme uygulanabilir
    //    }
    //    return Task.FromResult(true);
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Seçim iþlemi ve düzenleme için kod
        if (e.SelectedItem is Customer selectedCustomer)
        {
            // Düzenleme panelini doldurmak için seçilen müþteri bilgilerini aktar
            EditPanel.BindingContext = selectedCustomer; // Paneli baðla
            EditPanel.IsVisible = true; // Paneli görünür yap

            // Seçim detaylarýný göster
            await DisplayAlert("Seçilen Müþteri", $"Ýsim: {selectedCustomer.FullName}\nHizmet Türü: {selectedCustomer.AdditionalInfo}\nNot: {selectedCustomer.Notlar}\nKayýt Tarihi: {selectedCustomer.KayitTarihi}\nSeans Ücreti: {selectedCustomer.Ucret}", "Tamam");
        }

        // Seçimi temizle
        CustomerListView.SelectedItem = null;
    }
    public class Customer
    {
        public string FullName { get; set; }
        public string AdditionalInfo { get; set; }
        public string Notlar { get; set; }
        public string KayitTarihi { get; set; }
        public int Ucret {  get; set; }
    }
}