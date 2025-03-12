namespace Lotus_Spor;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = this;
    }

    private void OnPhoneEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Sadece say�lar� filtrele
        string numericInput = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        // Maksimum 10 karakter s�n�r� i�in g�vence
        if (numericInput.Length > 10)
            numericInput = numericInput.Substring(0, 10);

        // Eski de�erle fark� kontrol ederek Text'i g�ncelle
        if (PasswordEntry.Text != numericInput)
        {
            PasswordEntry.Text = numericInput;
        }
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string isim, soyisim;
        string fullName = UsernameEntry.Text;
        string sifre = PasswordEntry.Text;

        // Bo� alan kontrol�
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(sifre))
        {
            await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurunuz!", "Tamam");
            return; // ��lem burada sonlan�r
        }

        string[] nameParts = fullName.Split(' ');

        if (nameParts.Length > 1)
        {
            isim = string.Join(" ", nameParts[..^1]);
            soyisim = nameParts[^1];

            try
            {
                //if (LoginManager.IsSpecialUser(isim, soyisim)) // �zel kullan�c� kontrol�
                //{
                //    if (!string.IsNullOrEmpty(sifre))
                //    {
                //        // Do�rulama kodu olu�tur
                //        string verificationCode = GenerateVerificationCode();

                //        // SMS g�nder (�rne�in Twilio API kullanarak)
                //        bool smsSent = await SmsService.SendSmsAsync(sifre,
                //            $"Do�rulama kodunuz: {verificationCode}");

                //        if (!smsSent)
                //        {
                //            await DisplayAlert("Hata", "Do�rulama kodu g�nderilemedi!", "Tamam");
                //            return;
                //        }

                //        // Kullan�c�dan do�rulama kodunu al
                //        string enteredCode = await DisplayPromptAsync("Do�rulama",
                //            "L�tfen cep telefonunuza gelen do�rulama kodunu girin:",
                //            "G�nder", "�ptal");

                //        if (enteredCode != verificationCode)
                //        {
                //            await DisplayAlert("Hata", "Do�rulama kodu hatal�!", "Tamam");
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        await DisplayAlert("Hata", "Telefon numaras� bulunamad�!", "Tamam");
                //        return;
                //    }
                //}
                if (LoginManager.Login(isim, soyisim, sifre))
                {
                    // Kullan�c� rol�n� kaydet
                    Preferences.Set("UserRole", LoginManager.UserRole);
                    Preferences.Set("LoggedInUser", LoginManager.LoggedInUser);
                    Preferences.Set("LoggedInUser2", LoginManager.LoggedInUser2);
                    Preferences.Set("Gender", LoginManager.Gender);
                    Preferences.Set("LoggedInUserId", LoginManager.LoggedInUserId.ToString());

                    if (LoginManager.UserRole == "yonetici")
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new AdminPanelPage());
                    }
                    else if (LoginManager.UserRole == "musteri")
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
                    }
                }
                else
                {
                    await DisplayAlert("Hata", "Kullan�c� ad� veya �ifre hatal�!", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
        else
        {
            await DisplayAlert("Hata", "L�tfen tam ad�n�z� giriniz (isim ve soyisim)!", "Tamam");
        }
    }
    protected override bool OnBackButtonPressed()
    {
        return true;
    }
    private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        // Kullan�c� taraf�ndan girilen metni al
        string enteredText = UsernameEntry.Text;

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
            UsernameEntry.Text = string.Join(" ", words);
        }
    }
    private string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // 6 haneli do�rulama kodu
    }
}