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
        string query = "SELECT id, isim, soyisim, hizmet_turu, seans_ucreti, notlar, telefon, kayit_tarihi FROM musteriler ORDER BY isim ASC";

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
                                ID = Convert.ToInt32(reader["id"]),
                                FullName = $"{reader["isim"]} {reader["soyisim"]}",
                                AdditionalInfo = reader["hizmet_turu"]?.ToString() ?? "Bilinmiyor",
                                Notlar = reader["notlar"]?.ToString() ?? "Bilinmiyor",
                                Telefon = reader["telefon"]?.ToString() ?? "Bilinmiyor",
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
    private async void OnPhoneEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Sadece sayýlarý filtrele
        string numericInput = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        if (numericInput.StartsWith("0"))
        {
            // Uyarý ver
            await DisplayAlert("Uyarý", "Telefon numarasý 0 ile baþlayamaz.", "Tamam");

            // Ýlk rakamý kaldýr
            numericInput = numericInput.TrimStart('0');
        }

        // Maksimum 10 karakter sýnýrý için güvence
        if (numericInput.Length > 10)
            numericInput = numericInput.Substring(0, 10);

        // Eski deðerle farký kontrol ederek Text'i güncelle
        if (telefonentry.Text != numericInput)
        {
            telefonentry.Text = numericInput;
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
        CustomerListView.IsVisible = true;
        EditPanel.IsVisible = false;

        // Liste öðesinin seçilmesini kaldýrýyoruz
        CustomerListView.SelectedItem = null;
    }
    private async void OnSaveEditClicked(object sender, EventArgs e)
    {
        // EditPanel'deki müþteri bilgilerini alýyoruz
        string fullName = isimentry.Text;
        string seansTur = SeansPicker.SelectedItem.ToString();
        string notlar = notentry.Text;
        decimal ucret = decimal.Parse(ucretentry.Text);
        string telefon = telefonentry.Text;

        // Müþteri ismini soyadýna ayýrýyoruz
        string[] nameParts = fullName.Split(' ');
        string isim, soyisim;

        if (nameParts.Length > 1)
        {
            isim = string.Join(" ", nameParts.Take(nameParts.Length - 1));
            soyisim = nameParts.Last();
        }
        else
        {
            await DisplayAlert("Hata", "Soyadý belirlemek için en az iki kelime girilmelidir.", "Tamam");
            return;
        }

        // Seçilen müþteriyi alýyoruz. (ListView'deki seçilen öðe)
        var selectedCustomer = CustomerListView.SelectedItem as Customer; // Burada Customer, müþteri verilerinin olduðu sýnýf adý

        if (selectedCustomer == null)
        {
            await DisplayAlert("Hata", "Bir müþteri seçmeniz gerekmektedir.", "Tamam");
            return;
        }

        int customerId = selectedCustomer.ID; // Müþterinin ID'sini alýyoruz

        // Veritabaný baðlantýsý ve güncelleme sorgusu
        string query = @"
                UPDATE musteriler 
                SET 
                    isim = @isim, 
                    soyisim = @soyisim, 
                    telefon = @telefon, 
                    hizmet_turu = @seans_turu, 
                    notlar = @notlar, 
                    seans_ucreti = @ucret
                WHERE id = @id"; // ID'ye göre güncelleme yapýyoruz

        try
        {
            using (MySqlConnection connection = Database.GetConnection()) // Database.GetConnection() veritabaný baðlantýnýzý alýyor
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Parametreleri ekliyoruz
                    command.Parameters.AddWithValue("@isim", isim);
                    command.Parameters.AddWithValue("@soyisim", soyisim);
                    command.Parameters.AddWithValue("@telefon", telefon);
                    command.Parameters.AddWithValue("@seans_turu", seansTur);
                    command.Parameters.AddWithValue("@notlar", notlar);
                    command.Parameters.AddWithValue("@ucret", ucret);
                    command.Parameters.AddWithValue("@id", customerId); // ID parametresi ekliyoruz

                    // Sorguyu çalýþtýrýyoruz
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Baþarýlý", "Müþteri bilgileri baþarýyla güncellendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Güncellenen müþteri bulunamadý.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabaný Hatasý", ex.Message, "Tamam");
        }

        // Liste öðesinin seçilmesini kaldýrýyoruz
        LoadCustomers();
        CustomerListView.SelectedItem = null;
        EditPanel.IsVisible = false;
        CustomerListView.IsVisible = true;
    }
    private async void OnDeleteCustomerClicked(object sender, EventArgs e)
    {
        if (CustomerListView.SelectedItem is Customer selectedCustomer)
        {
            var confirmation = await DisplayAlert("Silme Onayý",
                                                    $"'{selectedCustomer.FullName}' adlý müþteriyi silmek istediðinize emin misiniz?",
                                                    "Evet", "Hayýr");

            if (confirmation)
            {
                string query = "DELETE FROM musteriler WHERE id = @id";

                try
                {
                    using (var connection = Database.GetConnection())
                    {
                        await connection.OpenAsync();

                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", selectedCustomer.ID);

                            int rowsAffected = await command.ExecuteNonQueryAsync();

                            if (rowsAffected > 0)
                            {
                                await DisplayAlert("Baþarýlý", "Müþteri baþarýyla silindi.", "Tamam");
                                // Müþteri listesi güncellenebilir
                                Customers.Remove(selectedCustomer); // veya listeden çýkarýlabilir
                            }
                            else
                            {
                                await DisplayAlert("Hata", "Müþteri silinirken bir sorun oluþtu.", "Tamam");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Veritabaný Hatasý", $"Silme iþlemi sýrasýnda hata oluþtu: {ex.Message}", "Tamam");
                }
            }
        }
        else
        {
            await DisplayAlert("Hata", "Silinecek müþteri seçilmedi.", "Tamam");
        }
        EditPanel.IsVisible = false;
        CustomerListView.IsVisible = true;
    }
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            // Seçilen öðeyi Customer türüne çeviriyoruz
            var selectedCustomer = e.SelectedItem as Customer;

            // EditPanel'i görünür yapýyoruz
            EditPanel.IsVisible = true;
            CustomerListView.IsVisible = false;

            // EditPanel'deki alanlara týklanan öðenin verilerini baðlýyoruz
            isimentry.Text = selectedCustomer.FullName;
            SeansPicker.SelectedItem = selectedCustomer.AdditionalInfo;
            notentry.Text = selectedCustomer.Notlar;
            ucretentry.Text = selectedCustomer.Ucret.ToString();
            telefonentry.Text = selectedCustomer.Telefon;
        }
    }
    public class Customer
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string AdditionalInfo { get; set; }
        public string Notlar { get; set; }
        public string Telefon { get; set; }
        public string KayitTarihi { get; set; }
        public int Ucret {  get; set; }
    }
}