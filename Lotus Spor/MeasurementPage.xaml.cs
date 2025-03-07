using CommunityToolkit.Maui.Views;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
namespace Lotus_Spor;

public partial class MeasurementPage : ContentPage
{
	List<string> isimListesi = new List<string>();
	private ObservableCollection<Customer> Customers { get; set; }
    private ObservableCollection<OlcuModel> Olculer { get; set; }
	int KullaniciID = -1;
    string KullaniciAdi, nullText = string.Empty;
    private List<Customer> AllCustomers = new List<Customer>();
    public MeasurementPage()
	{
		InitializeComponent();

		Customers = new ObservableCollection<Customer>();
		CustomerListView.ItemsSource = Customers;
        SelectedCustomerListView.ItemsSource = Olculer;
		LoadCustomers();

		BindingContext = this;
	}
	private async void LoadCustomers()
	{
        var loadingPopup = new LoadingPopup();
        this.ShowPopup(loadingPopup);

        try
        {
            string query = "SELECT id, isim, soyisim FROM musteriler WHERE aktiflik = 'Aktif' ORDER BY isim ASC";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    await conn.OpenAsync();
                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        AllCustomers.Clear();
                        while (await reader.ReadAsync())
                        {
                            var customer = new Customer
                            {
                                ID = Convert.ToInt32(reader["id"]),
                                FullName = $"{reader["isim"]} {reader["soyisim"]}"
                            };
                            AllCustomers.Add(customer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Müþteri verileri yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
            }

            await Task.Delay(1000);
            Customers.Clear();
            foreach (var customer in AllCustomers)
            {
                Customers.Add(customer);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Müþteri verileri yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
        }
        finally
        {
            loadingPopup.Close();
        }

    }
    private void SearchCustomers(string searchName)
    {
        Customers.Clear();

        var filteredCustomers = string.IsNullOrWhiteSpace(searchName)
            ? AllCustomers
            : AllCustomers.Where(c => c.FullName.Contains(searchName, StringComparison.OrdinalIgnoreCase));

        foreach (var customer in filteredCustomers)
        {
            Customers.Add(customer);
        }
    }
    public async Task<ObservableCollection<OlcuModel>> OlculeriGetir(string MusteriIsmi)
    {
        var olculer = new ObservableCollection<OlcuModel>();
        string OlcuQuery = "SELECT * FROM VucutBilgileri WHERE KullaniciId = @id ORDER BY Tarih DESC";

        using (var conn = Database.GetConnection())
        {
            await conn.OpenAsync();
            using (var cmd = new MySqlCommand(OlcuQuery, conn))
            {
                cmd.Parameters.AddWithValue("@id", KullaniciID);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    olculer.Clear();
                    while (await reader.ReadAsync())
                    {
                        olculer.Add(new OlcuModel
                        {
                            Isim = MusteriIsmi,
                            Tarih = Convert.ToDateTime(reader["Tarih"]),
                            Kilo = reader.GetDecimal("Kilo"),
                            YagOrani = reader.GetDecimal("YagOrani"),
                            SuOrani = reader.GetDecimal("SuOrani"),
                            KasOrani = reader.GetDecimal("KasOrani"),
                            BMI = reader.GetDecimal("BMI"),
                            Omuz = reader.GetDecimal("Omuz"),
                            Biceps = reader.GetDecimal("Biceps"),
                            Gogus = reader.GetDecimal("Gogus"),
                            Bel = reader.GetDecimal("Bel"),
                            Karin = reader.GetDecimal("Karin"),
                            Kalca = reader.GetDecimal("Kalca"),
                            Bacak = reader.GetDecimal("Bacak"),
                            Kalf = reader.GetDecimal("Kalf")
                        });
                    }
                }
            }
        }

        if (olculer.Count == 0)
        {
            await DisplayAlert("Uyarý", $"Bu müþteri için hiç ölçü bilgisi girilmemiþ.", "Tamam");
            btnUyeListele.IsVisible = false;
            CustomerListView.IsVisible = true;
            lblSearchEntry.IsVisible = true;
            //LoadCustomers();
            SearchEntry.IsVisible = true;
        }

        return olculer;
    }
    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string SearchName;
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        if (!string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = true;
            SearchName = SearchEntry.Text;
            Console.WriteLine(searchText);

            SearchCustomers(SearchName);
        }
        else if (string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = false;
            SearchName = string.Empty;
            Console.WriteLine(searchText);

            SearchCustomers(SearchName);
        }

        if (string.IsNullOrWhiteSpace(searchText))
        {
            CustomerListView.IsVisible = true;
            return;
        }

        // Listeyi filtrele
        var suggestions = isimListesi
            .Where(isimSoyisim => isimSoyisim.ToLower().Contains(searchText))
            .ToList();
    }
    private async void OnClearButtonClicked(object sender, EventArgs e)
    {
        SearchEntry.Text = string.Empty;
        btnClear.IsVisible = false;

        SearchCustomers(nullText);
    }
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        CustomerListView.IsVisible = false;

        if (e.SelectedItem != null)
        {
            CustomerListView.IsVisible = false;
            CustomerListView.SelectedItem = null;
            
            lblSearchEntry.IsVisible = false;
            SearchEntry.IsVisible = false;
            btnUyeListele.IsVisible = true;

            if (!string.IsNullOrEmpty(SearchEntry.Text))
            {
                SearchEntry.Text = string.Empty;
                btnClear.IsVisible = !string.IsNullOrEmpty(SearchEntry.Text);
                OnPropertyChanged(nameof(btnClear));
            }

            var selectedCustomer = e.SelectedItem as Customer;
            KullaniciID = selectedCustomer.ID;
            KullaniciAdi = selectedCustomer.FullName;

            var loadingPopup = new LoadingPopup();
            this.ShowPopup(loadingPopup);
            try
            {
                Olculer = await OlculeriGetir(KullaniciAdi);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Müþteri verileri yüklenirken bir hata oluþtu: {ex.Message}", "Tamam");
            }
            finally
            {
                loadingPopup.Close();
            }
            //Olculer = await OlculeriGetir(KullaniciAdi);
            SelectedCustomerListView.ItemsSource = Olculer;
        }
    }
    private void OnListeyeDonClicked(object sender, EventArgs e)
    {
        btnUyeListele.IsVisible = false;
        CustomerListView.IsVisible = true;
        lblSearchEntry.IsVisible = true;
        SearchEntry.IsVisible = true;
        Olculer.Clear();
    }
}