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
            TotalMembersLabel.Text = $"Toplam �ye Say�s�: {Customers.Count}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"M��teri verileri y�klenirken bir hata olu�tu: {ex.Message}", "Tamam");
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
        //EditPanel.IsVisible = false; // D�zenleme panelini kapat
    }
    private async void OnSaveEditClicked(object sender, EventArgs e)
    {
    //    if (EditPanel.BindingContext is Customer editedCustomer)
    //    {
    //        // D�zenlenmi� m��teri bilgilerini veritaban�na veya API'ye kaydet
    //        bool success = await SaveCustomerToDatabaseAsync(editedCustomer);

    //        if (success)
    //        {
    //            await DisplayAlert("Ba�ar�l�", "M��teri bilgileri ba�ar�yla g�ncellendi.", "Tamam");
    //        }
    //        else
    //        {
    //            await DisplayAlert("Hata", "M��teri bilgileri g�ncellenemedi. L�tfen tekrar deneyin.", "Tamam");
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

    //                // Sorguyu �al��t�r
    //                int rowsAffected = await command.ExecuteNonQueryAsync();

    //                // G�ncellemenin ba�ar�l� olup olmad���n� kontrol et
    //                if (rowsAffected > 0)
    //                {
    //                    Console.WriteLine("M��teri bilgileri ba�ar�yla g�ncellendi.");
    //                }
    //                else
    //                {
    //                    Console.WriteLine("M��teri bilgileri g�ncellenemedi. Belirtilen m��teri bulunamad�.");
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Bir hata olu�tu: {ex.Message}");
    //        throw; // Hata f�rlat�labilir veya daha iyi bir hata i�leme uygulanabilir
    //    }
    //    return Task.FromResult(true);
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Se�im i�lemi ve d�zenleme i�in kod
        if (e.SelectedItem is Customer selectedCustomer)
        {
            // D�zenleme panelini doldurmak i�in se�ilen m��teri bilgilerini aktar
            EditPanel.BindingContext = selectedCustomer; // Paneli ba�la
            EditPanel.IsVisible = true; // Paneli g�r�n�r yap

            // Se�im detaylar�n� g�ster
            await DisplayAlert("Se�ilen M��teri", $"�sim: {selectedCustomer.FullName}\nHizmet T�r�: {selectedCustomer.AdditionalInfo}\nNot: {selectedCustomer.Notlar}\nKay�t Tarihi: {selectedCustomer.KayitTarihi}\nSeans �creti: {selectedCustomer.Ucret}", "Tamam");
        }

        // Se�imi temizle
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