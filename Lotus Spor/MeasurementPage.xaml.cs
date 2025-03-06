using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace Lotus_Spor;

public partial class MeasurementPage : ContentPage
{
	List<string> isimListesi = new List<string>();
	private ObservableCollection<Customer> Customers { get; set; }
    private ObservableCollection<OlcuModel> Olculer { get; set; }
    string SearchName;
	int KullaniciID = -1;
    string KullaniciAdi;
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
		string query = @"SELECT id, isim, soyisim FROM musteriler WHERE aktiflik = 'Aktif'";

		try
		{
			using (var conn = Database.GetConnection())
			{
				await conn.OpenAsync();

                List<MySqlParameter> parameters = new List<MySqlParameter>();

                if (!string.IsNullOrEmpty(SearchName))
                {
                    query += " AND CONCAT(isim, ' ', soyisim) LIKE @searchName";
                    parameters.Add(new MySqlParameter("@searchName", "%" + SearchName + "%"));
                }

				query += " ORDER BY isim ASC";

                using (var cmd = new MySqlCommand(query, conn))
				{
					Customers.Clear();

                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }

                    using (var reader = await cmd.ExecuteReaderAsync())
					{
						while (await reader.ReadAsync())
						{
                            var customer = new Customer
                            {
                                ID = Convert.ToInt32(reader["id"]),
                                FullName = $"{reader["isim"]} {reader["soyisim"]}"
                            };
                            Customers.Add(customer);
                        }
					}
				}
			}
		}
		catch (Exception ex)
		{
            await DisplayAlert("Hata", $"Müþteri verileri yüklenirken bir hata oluþtu: {ex.Message}\n {query}", "Tamam");
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
        string searchText = e.NewTextValue?.ToLower() ?? string.Empty;

        if (!string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = true;
        }
        else if (string.IsNullOrEmpty(SearchEntry.Text))
        {
            btnClear.IsVisible = false;
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

        SearchName = SearchEntry.Text;
        LoadCustomers();
    }
    private async void OnClearButtonClicked(object sender, EventArgs e)
    {
        SearchEntry.Text = string.Empty;
        SearchName = string.Empty;
        btnClear.IsVisible = false;

        LoadCustomers();
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

            if (!string.IsNullOrEmpty(SearchName))
            {
                SearchEntry.Text = string.Empty;
                SearchName = string.Empty;
                btnClear.IsVisible = !string.IsNullOrEmpty(SearchEntry.Text);
                OnPropertyChanged(nameof(btnClear));
            }

            var selectedCustomer = e.SelectedItem as Customer;
            KullaniciID = selectedCustomer.ID;
            KullaniciAdi = selectedCustomer.FullName;

            Olculer = await OlculeriGetir(KullaniciAdi);
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
        LoadCustomers();
    }
}