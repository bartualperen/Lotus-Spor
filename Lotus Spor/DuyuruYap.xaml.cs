using MySql.Data.MySqlClient;

namespace Lotus_Spor;

public partial class DuyuruYap : ContentPage
{
	string antrenor = Preferences.Get("loggedInUser", string.Empty) + " " + Preferences.Get("loggedInUser2", string.Empty);
	string antrenorID = Preferences.Get("loggedInUserID", "0");
	public DuyuruYap()
	{
		InitializeComponent();
	}
	private async void OnSaveAnnouncementClicked(object sender, EventArgs e)
	{
		try
		{
            string baslik = AnnouncementTitle.Text;
            string aciklama = AnnouncementDescription.Text;
            string duyuru = AnnouncementBody.Text;

            if (string.IsNullOrWhiteSpace(AnnouncementTitle.Text) ||
                string.IsNullOrWhiteSpace(AnnouncementDescription.Text) ||
                string.IsNullOrWhiteSpace(AnnouncementBody.Text))
            {
                await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurun.", "Tamam");
                return;
            }

            string query = "INSERT INTO duyurular (baslik, aciklama, duyuru, antrenor_id) VALUES (@baslik, @aciklama, @duyuru, @antrenor_id)";

            try
            {
                using (MySqlConnection conn = Database.GetConnection())
                {
                    await conn.OpenAsync();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@baslik", baslik);
                        cmd.Parameters.AddWithValue("@aciklama", aciklama);
                        cmd.Parameters.AddWithValue("@duyuru", duyuru);
                        cmd.Parameters.AddWithValue("@antrenor_id", antrenorID);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            await DisplayAlert("Baþarýlý", "Duyuru baþarýyla eklendi.", "Tamam");
                            await Navigation.PopAsync();
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
        }
		catch (Exception ex)
		{
            await DisplayAlert("Hata", ex.Message, "Tamam");
        }
    }
}