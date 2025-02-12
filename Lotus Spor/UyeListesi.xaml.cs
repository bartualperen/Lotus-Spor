using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace Lotus_Spor;


public partial class UyeListesi : ContentPage
{
    List<string> isimListesi = new List<string>();
    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    public ObservableCollection<Customer> AktifCustomers { get; set; }
    public ObservableCollection<Customer> PasifCustomers { get; set; }

    public UyeListesi()
    {
        InitializeComponent();
        AktifCustomers = new ObservableCollection<Customer>();
        PasifCustomers = new ObservableCollection<Customer>();

        AktifCustomerListView.ItemsSource = AktifCustomers;
        PasifCustomerListView.ItemsSource = PasifCustomers;

        // Veritabanýndan verileri yükle
        LoadCustomers();
    }

    private async void LoadCustomers()
    {
        string query = "SELECT id, isim, soyisim, hizmet_turu, seans_ucreti, notlar, telefon, kayit_tarihi, aktiflik FROM musteriler ORDER BY isim ASC";

        try
        {
            using (var connection = Database.GetConnection())
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        AktifCustomers.Clear();
                        PasifCustomers.Clear();

                        while (await reader.ReadAsync())
                        {
                            var customer = new Customer
                            {
                                ID = Convert.ToInt32(reader["id"]),
                                FullName = $"{reader["isim"]} {reader["soyisim"]}",
                                AdditionalInfo = reader["hizmet_turu"]?.ToString() ?? "Bilinmiyor",
                                Notlar = reader["notlar"]?.ToString() ?? "Bilinmiyor",
                                Telefon = reader["telefon"]?.ToString() ?? "Bilinmiyor",
                                KayitTarihi = reader["kayit_tarihi"] != DBNull.Value
                                    ? Convert.ToDateTime(reader["kayit_tarihi"]).ToString("dd-MM-yyyy")
                                    : "Bilinmiyor",
                                Ucret = Convert.ToInt32(reader["seans_ucreti"]),
                                Aktiflik = reader["aktiflik"].ToString()
                            };

                            // Müþteriyi Aktif veya Pasif listesine ekleyelim
                            if (customer.Aktiflik == "Aktif")
                            {
                                AktifCustomers.Add(customer);
                            }
                            else if (customer.Aktiflik == "Pasif")
                            {
                                PasifCustomers.Add(customer);
                            }
                        }
                    }
                }
            }

            TotalMembersLabel.Text = $"Toplam Üye Sayýsý: {AktifCustomers.Count + PasifCustomers.Count}";
            TotalActiveMembersLabel.Text = $"Toplam Aktif Üye Sayýsý: {AktifCustomers.Count}";
            TotalPasiveMembersLabel.Text = $"Toplam Pasif Üye Sayýsý: { PasifCustomers.Count}";
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
        if (!string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = true;
        }
        else if (string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = false;
        }
        string searchQuery = e.NewTextValue?.ToLower() ?? string.Empty;
        AktifCustomerListView.ItemsSource = string.IsNullOrEmpty(searchQuery)
            ? AktifCustomers
            : new ObservableCollection<Customer>(
                AktifCustomers.Where(c => c.FullName.ToLower().Contains(searchQuery)));
        PasifCustomerListView.ItemsSource = string.IsNullOrEmpty(searchQuery)
            ? PasifCustomers
            : new ObservableCollection<Customer>(
                PasifCustomers.Where(c => c.FullName.ToLower().Contains(searchQuery)));
    }
    private void OnCancelEditClicked(object sender, EventArgs e)
    {
        AktifCustomerListView.IsVisible = true;
        PasifCustomerListView.IsVisible = true;
        EditPanel.IsVisible = false;
        SearchEntry.IsVisible = true;
        lblSearchEntry.IsVisible = true;
        if (!string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = true;
        }
        else if (string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = false;
        }

        // Liste öðesinin seçilmesini kaldýrýyoruz
        AktifCustomerListView.SelectedItem = null;
        PasifCustomerListView.SelectedItem = null;
    }
    private async void OnSaveEditClicked(object sender, EventArgs e)
    {
        // EditPanel'deki müþteri bilgilerini alýyoruz
        string fullName = isimentry.Text;
        string seansTur = SeansPicker.SelectedItem.ToString();
        string aktiflik = AktiflikPicker.SelectedItem.ToString();
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

        // Seçilen müþteriyi alýyoruz (Aktif veya Pasif müþteri listelerinden)
        Customer selectedCustomer = null;

        if (AktifCustomerListView.SelectedItem is Customer aktifCustomer)
        {
            selectedCustomer = aktifCustomer;
        }
        else if (PasifCustomerListView.SelectedItem is Customer pasifCustomer)
        {
            selectedCustomer = pasifCustomer;
        }

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
                seans_ucreti = @ucret,
                aktiflik = @aktiflik
            WHERE id = @id";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
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
                    command.Parameters.AddWithValue("@aktiflik", aktiflik);
                    command.Parameters.AddWithValue("@id", customerId);

                    // Sorguyu çalýþtýrýyoruz
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Baþarýlý", "Müþteri bilgileri baþarýyla güncellendi.", "Tamam");

                        // Eski listeden kaldýr ve uygun listeye ekle
                        if (selectedCustomer.Aktiflik == "Aktif")
                        {
                            AktifCustomers.Remove(selectedCustomer);
                        }
                        else if (selectedCustomer.Aktiflik == "Pasif")
                        {
                            PasifCustomers.Remove(selectedCustomer);
                        }

                        // Güncellenmiþ müþteri nesnesini oluþtur
                        var updatedCustomer = new Customer
                        {
                            ID = customerId,
                            FullName = $"{isim} {soyisim}",
                            AdditionalInfo = seansTur,
                            Notlar = notlar,
                            Telefon = telefon,
                            KayitTarihi = selectedCustomer.KayitTarihi,
                            Ucret = (int)ucret,
                            Aktiflik = aktiflik
                        };

                        // Yeni aktiflik durumuna göre ekle
                        if (aktiflik == "Aktif")
                        {
                            AktifCustomers.Add(updatedCustomer);
                        }
                        else
                        {
                            PasifCustomers.Add(updatedCustomer);
                        }
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
        AktifCustomerListView.SelectedItem = null;
        PasifCustomerListView.SelectedItem = null;
        EditPanel.IsVisible = false;
        AktifCustomerListView.IsVisible = true;
        PasifCustomerListView.IsVisible = true;
        SearchEntry.IsVisible = true;
        lblSearchEntry.IsVisible = true;

        btnClear.IsVisible = !string.IsNullOrEmpty(SearchEntry.Text);
    }
    private async void OnDeleteCustomerClicked(object sender, EventArgs e)
    {
        Customer selectedCustomer = null;

        if (AktifCustomerListView.SelectedItem is Customer aktifCustomer)
        {
            selectedCustomer = aktifCustomer;
        }
        else if (PasifCustomerListView.SelectedItem is Customer pasifCustomer)
        {
            selectedCustomer = pasifCustomer;
        }

        if (selectedCustomer != null)
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

                                // Aktif veya Pasif listeden kaldýr
                                if (selectedCustomer.Aktiflik == "Aktif")
                                {
                                    AktifCustomers.Remove(selectedCustomer);
                                }
                                else if (selectedCustomer.Aktiflik == "Pasif")
                                {
                                    PasifCustomers.Remove(selectedCustomer);
                                }
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
        AktifCustomerListView.IsVisible = true;
        PasifCustomerListView.IsVisible = true;
        SearchEntry.IsVisible = true;
        lblSearchEntry.IsVisible = true;

        btnClear.IsVisible = !string.IsNullOrEmpty(SearchEntry.Text);
    }
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            // Seçilen öðeyi Customer türüne çeviriyoruz
            var selectedCustomer = e.SelectedItem as Customer;

            // EditPanel'i görünür yapýyoruz
            EditPanel.IsVisible = true;
            AktifCustomerListView.IsVisible = false;
            PasifCustomerListView.IsVisible = false;

            SearchEntry.IsVisible = false;
            btnClear.IsVisible = false;
            lblSearchEntry.IsVisible = false;

            // EditPanel'deki alanlara týklanan öðenin verilerini baðlýyoruz
            isimentry.Text = selectedCustomer.FullName;
            SeansPicker.SelectedItem = selectedCustomer.AdditionalInfo;
            notentry.Text = selectedCustomer.Notlar;
            ucretentry.Text = selectedCustomer.Ucret.ToString();
            telefonentry.Text = selectedCustomer.Telefon;
            AktiflikPicker.SelectedItem = selectedCustomer.Aktiflik;
        }
    }
    private async void OnClearButtonClicked(object sender, EventArgs e)
    {
        SearchEntry.Text = string.Empty;
        btnClear.IsVisible = false;
        LoadCustomers();
    }
}