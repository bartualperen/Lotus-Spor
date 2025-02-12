using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace Lotus_Spor;

public partial class AntrenorYonet : ContentPage
{
    List<string> isimListesi = new List<string>();
    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    public ObservableCollection<Customer> Customers { get; set; }
    public string selectedÝsim;
    public AntrenorYonet()
    {
        InitializeComponent();
        Customers = new ObservableCollection<Customer>();
        CustomerListView.ItemsSource = Customers;

        // Load data from database
        LoadCustomers();
    }
    private async void LoadCustomers()
    {
        string query = "SELECT isim, soyisim FROM yoneticiler";

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
                                FullName = $"{reader["isim"]} {reader["soyisim"]}"
                            });
                        }
                    }
                }
            }
            var kisi = Customers.FirstOrDefault(c => c.FullName == ConnectionString.admin2);
            if (kisi != null)
            {
                Customers.Remove(kisi);
            }
            TotalMembersLabel.Text = $"Toplam Antrenör Sayýsý: {Customers.Count}";
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
        if (PhoneEntry.Text != numericInput)
        {
            PhoneEntry.Text = numericInput;
        }
    }
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Customer selectedCustomer)
        {
            selectedÝsim = selectedCustomer.FullName;
        }
    }
    private async void OnSaveUserClicked(Object sender, EventArgs e)
    {
        string fullName = NameEntry.Text;
        string telefon = PhoneEntry.Text.ToString();

        string[] nameParts = fullName.Split(' ');
        string isim, soyisim, kullaniciAdi;

        if (nameParts.Length > 1)
        {
            isim = string.Join(" ", nameParts[..^1]);
            soyisim = nameParts[^1];
            kullaniciAdi = isim.ToLower();
        }
        else
        {
            await DisplayAlert("Hata", "Soyadý belirlemek için en az iki kelime girilmelidir.", "Tamam");
            return;
        }

        string query = "INSERT INTO yoneticiler (isim, soyisim, kullanici_adi, sifre) VALUES (@isim, @soyisim, @kullaniciadi, @sifre)";

        try
        {
            using (MySqlConnection connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@isim", isim);
                    command.Parameters.AddWithValue("@soyisim", soyisim);
                    command.Parameters.AddWithValue("@sifre", telefon);
                    command.Parameters.AddWithValue("@kullaniciadi", kullaniciAdi);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Baþarýlý", "Antrenör bilgileri baþarýyla eklendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Veri eklenirken bir sorun oluþtu.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritabaný Hatasý", ex.Message, "Tamam");
        }
        AddUserForm.IsVisible = false;  // Formu gizle
        Ekle.IsVisible = true;
        Cikar.IsVisible = true;
        NameEntry.Text = "";  // Textbox'larý temizle
        PhoneEntry.Text = "";
        LoadCustomers();
    }
    private void OnEkleClicked(object sender, EventArgs e)
    {
        AddUserForm.IsVisible = true;  // Formu göster
        Ekle.IsVisible = false;
        Cikar.IsVisible = false;
    }
    private void OnCancelAddClicked(object sender, EventArgs e)
    {
        AddUserForm.IsVisible = false;  // Formu gizle
        Ekle.IsVisible= true;
        Cikar.IsVisible = true;
        NameEntry.Text = "";  // Textbox'larý temizle
        PhoneEntry.Text = "";
    }
    private async void OnCikarClicked(Object sender, EventArgs e)
    {
        bool isConfirmed = await DisplayAlert(
            "Uyarý",
            $"{selectedÝsim} kullanýcýsýný çýkarmak istediðinize emin misiniz?",
            "Evet",
            "Hayýr"
        );

        if (isConfirmed)
        {
            await RemoveUserFromDatabase(selectedÝsim);
        }
        else
        {
            CustomerListView.SelectedItem = null;
        }
        LoadCustomers();
    }
    private async Task RemoveUserFromDatabase(string selectedÝsim)
    {
        try
        {
            // Burada veritabaný silme iþlemini gerçekleþtirebilirsiniz.
            // Aþaðýda örnek bir SQL sorgusu gösterilmektedir.

            string query = "DELETE FROM yoneticiler WHERE CONCAT(isim, ' ', soyisim) = @selectedName";

            using (var connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@selectedName", selectedÝsim);
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Baþarýlý", $"{selectedÝsim} baþarýyla silindi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", $"{selectedÝsim} bulunamadý veya silinemedi.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Bir hata oluþtu: {ex.Message}", "Tamam");
        }
    }
    private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        // Kullanýcý tarafýndan girilen metni al
        string enteredText = NameEntry.Text;

        // Eðer metin boþ deðilse, kelimelerin ilk harfini büyük yap
        if (!string.IsNullOrEmpty(enteredText))
        {
            // Her kelimenin ilk harfini büyük yap
            var words = enteredText.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }

            // Düzenlenmiþ metni tekrar Entry'ye yaz
            NameEntry.Text = string.Join(" ", words);
        }
    }
}