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

        // Veritaban�ndan verileri y�kle
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

                            // M��teriyi Aktif veya Pasif listesine ekleyelim
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

            TotalMembersLabel.Text = $"Toplam �ye Say�s�: {AktifCustomers.Count + PasifCustomers.Count}";
            TotalActiveMembersLabel.Text = $"Toplam Aktif �ye Say�s�: {AktifCustomers.Count}";
            TotalPasiveMembersLabel.Text = $"Toplam Pasif �ye Say�s�: { PasifCustomers.Count}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"M��teri verileri y�klenirken bir hata olu�tu: {ex.Message}", "Tamam");
        }
    }

    private async void OnPhoneEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Sadece say�lar� filtrele
        string numericInput = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        if (numericInput.StartsWith("0"))
        {
            // Uyar� ver
            await DisplayAlert("Uyar�", "Telefon numaras� 0 ile ba�layamaz.", "Tamam");

            // �lk rakam� kald�r
            numericInput = numericInput.TrimStart('0');
        }

        // Maksimum 10 karakter s�n�r� i�in g�vence
        if (numericInput.Length > 10)
            numericInput = numericInput.Substring(0, 10);

        // Eski de�erle fark� kontrol ederek Text'i g�ncelle
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

        // Liste ��esinin se�ilmesini kald�r�yoruz
        AktifCustomerListView.SelectedItem = null;
        PasifCustomerListView.SelectedItem = null;
    }
    private async void OnSaveEditClicked(object sender, EventArgs e)
    {
        // EditPanel'deki m��teri bilgilerini al�yoruz
        string fullName = isimentry.Text;
        string seansTur = SeansPicker.SelectedItem.ToString();
        string aktiflik = AktiflikPicker.SelectedItem.ToString();
        string notlar = notentry.Text;
        decimal ucret = decimal.Parse(ucretentry.Text);
        string telefon = telefonentry.Text;

        // M��teri ismini soyad�na ay�r�yoruz
        string[] nameParts = fullName.Split(' ');
        string isim, soyisim;

        if (nameParts.Length > 1)
        {
            isim = string.Join(" ", nameParts.Take(nameParts.Length - 1));
            soyisim = nameParts.Last();
        }
        else
        {
            await DisplayAlert("Hata", "Soyad� belirlemek i�in en az iki kelime girilmelidir.", "Tamam");
            return;
        }

        // Se�ilen m��teriyi al�yoruz (Aktif veya Pasif m��teri listelerinden)
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
            await DisplayAlert("Hata", "Bir m��teri se�meniz gerekmektedir.", "Tamam");
            return;
        }

        int customerId = selectedCustomer.ID; // M��terinin ID'sini al�yoruz

        // Veritaban� ba�lant�s� ve g�ncelleme sorgusu
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

                    // Sorguyu �al��t�r�yoruz
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Ba�ar�l�", "M��teri bilgileri ba�ar�yla g�ncellendi.", "Tamam");

                        // Eski listeden kald�r ve uygun listeye ekle
                        if (selectedCustomer.Aktiflik == "Aktif")
                        {
                            AktifCustomers.Remove(selectedCustomer);
                        }
                        else if (selectedCustomer.Aktiflik == "Pasif")
                        {
                            PasifCustomers.Remove(selectedCustomer);
                        }

                        // G�ncellenmi� m��teri nesnesini olu�tur
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

                        // Yeni aktiflik durumuna g�re ekle
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
                        await DisplayAlert("Hata", "G�ncellenen m��teri bulunamad�.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritaban� Hatas�", ex.Message, "Tamam");
        }

        // Liste ��esinin se�ilmesini kald�r�yoruz
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
            var confirmation = await DisplayAlert("Silme Onay�",
                $"'{selectedCustomer.FullName}' adl� m��teriyi silmek istedi�inize emin misiniz?",
                "Evet", "Hay�r");

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
                                await DisplayAlert("Ba�ar�l�", "M��teri ba�ar�yla silindi.", "Tamam");

                                // Aktif veya Pasif listeden kald�r
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
                                await DisplayAlert("Hata", "M��teri silinirken bir sorun olu�tu.", "Tamam");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Veritaban� Hatas�", $"Silme i�lemi s�ras�nda hata olu�tu: {ex.Message}", "Tamam");
                }
            }
        }
        else
        {
            await DisplayAlert("Hata", "Silinecek m��teri se�ilmedi.", "Tamam");
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
            // Se�ilen ��eyi Customer t�r�ne �eviriyoruz
            var selectedCustomer = e.SelectedItem as Customer;

            // EditPanel'i g�r�n�r yap�yoruz
            EditPanel.IsVisible = true;
            AktifCustomerListView.IsVisible = false;
            PasifCustomerListView.IsVisible = false;

            SearchEntry.IsVisible = false;
            btnClear.IsVisible = false;
            lblSearchEntry.IsVisible = false;

            // EditPanel'deki alanlara t�klanan ��enin verilerini ba�l�yoruz
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