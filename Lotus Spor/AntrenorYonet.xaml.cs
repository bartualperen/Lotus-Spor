using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace Lotus_Spor;

public partial class AntrenorYonet : ContentPage
{
    List<string> isimListesi = new List<string>();
    private ObservableCollection<string> filteredList = new ObservableCollection<string>();
    public ObservableCollection<Customer> Customers { get; set; }
    public string selected�sim;
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
            TotalMembersLabel.Text = $"Toplam Antren�r Say�s�: {Customers.Count}";
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
        if (PhoneEntry.Text != numericInput)
        {
            PhoneEntry.Text = numericInput;
        }
    }
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Customer selectedCustomer)
        {
            selected�sim = selectedCustomer.FullName;
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
            await DisplayAlert("Hata", "Soyad� belirlemek i�in en az iki kelime girilmelidir.", "Tamam");
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
                        await DisplayAlert("Ba�ar�l�", "Antren�r bilgileri ba�ar�yla eklendi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", "Veri eklenirken bir sorun olu�tu.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Veritaban� Hatas�", ex.Message, "Tamam");
        }
        AddUserForm.IsVisible = false;  // Formu gizle
        Ekle.IsVisible = true;
        Cikar.IsVisible = true;
        NameEntry.Text = "";  // Textbox'lar� temizle
        PhoneEntry.Text = "";
        LoadCustomers();
    }
    private void OnEkleClicked(object sender, EventArgs e)
    {
        AddUserForm.IsVisible = true;  // Formu g�ster
        Ekle.IsVisible = false;
        Cikar.IsVisible = false;
    }
    private void OnCancelAddClicked(object sender, EventArgs e)
    {
        AddUserForm.IsVisible = false;  // Formu gizle
        Ekle.IsVisible= true;
        Cikar.IsVisible = true;
        NameEntry.Text = "";  // Textbox'lar� temizle
        PhoneEntry.Text = "";
    }
    private async void OnCikarClicked(Object sender, EventArgs e)
    {
        bool isConfirmed = await DisplayAlert(
            "Uyar�",
            $"{selected�sim} kullan�c�s�n� ��karmak istedi�inize emin misiniz?",
            "Evet",
            "Hay�r"
        );

        if (isConfirmed)
        {
            await RemoveUserFromDatabase(selected�sim);
        }
        else
        {
            CustomerListView.SelectedItem = null;
        }
        LoadCustomers();
    }
    private async Task RemoveUserFromDatabase(string selected�sim)
    {
        try
        {
            // Burada veritaban� silme i�lemini ger�ekle�tirebilirsiniz.
            // A�a��da �rnek bir SQL sorgusu g�sterilmektedir.

            string query = "DELETE FROM yoneticiler WHERE CONCAT(isim, ' ', soyisim) = @selectedName";

            using (var connection = Database.GetConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@selectedName", selected�sim);
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Ba�ar�l�", $"{selected�sim} ba�ar�yla silindi.", "Tamam");
                    }
                    else
                    {
                        await DisplayAlert("Hata", $"{selected�sim} bulunamad� veya silinemedi.", "Tamam");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Bir hata olu�tu: {ex.Message}", "Tamam");
        }
    }
    private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        // Kullan�c� taraf�ndan girilen metni al
        string enteredText = NameEntry.Text;

        // E�er metin bo� de�ilse, kelimelerin ilk harfini b�y�k yap
        if (!string.IsNullOrEmpty(enteredText))
        {
            // Her kelimenin ilk harfini b�y�k yap
            var words = enteredText.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }

            // D�zenlenmi� metni tekrar Entry'ye yaz
            NameEntry.Text = string.Join(" ", words);
        }
    }
}