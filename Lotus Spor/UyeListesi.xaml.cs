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
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Customer selectedCustomer)
        {
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