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
        // Sadece sayýlarý filtrele
        string numericInput = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

        // Maksimum 10 karakter sýnýrý için güvence
        if (numericInput.Length > 10)
            numericInput = numericInput.Substring(0, 10);

        // Eski deðerle farký kontrol ederek Text'i güncelle
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

        // Boþ alan kontrolü
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(sifre))
        {
            await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurunuz!", "Tamam");
            return; // Ýþlem burada sonlanýr
        }

        string[] nameParts = fullName.Split(' ');

        if (nameParts.Length > 1)
        {
            isim = string.Join(" ", nameParts[..^1]);
            soyisim = nameParts[^1];

            try
            {
                //if (LoginManager.IsSpecialUser(isim, soyisim)) // Özel kullanýcý kontrolü
                //{
                //    if (!string.IsNullOrEmpty(sifre))
                //    {
                //        // Doðrulama kodu oluþtur
                //        string verificationCode = GenerateVerificationCode();

                //        // SMS gönder (örneðin Twilio API kullanarak)
                //        bool smsSent = await SmsService.SendSmsAsync(sifre,
                //            $"Doðrulama kodunuz: {verificationCode}");

                //        if (!smsSent)
                //        {
                //            await DisplayAlert("Hata", "Doðrulama kodu gönderilemedi!", "Tamam");
                //            return;
                //        }

                //        // Kullanýcýdan doðrulama kodunu al
                //        string enteredCode = await DisplayPromptAsync("Doðrulama",
                //            "Lütfen cep telefonunuza gelen doðrulama kodunu girin:",
                //            "Gönder", "Ýptal");

                //        if (enteredCode != verificationCode)
                //        {
                //            await DisplayAlert("Hata", "Doðrulama kodu hatalý!", "Tamam");
                //            return;
                //        }
                //    }
                //    else
                //    {
                //        await DisplayAlert("Hata", "Telefon numarasý bulunamadý!", "Tamam");
                //        return;
                //    }
                //}
                if (LoginManager.Login(isim, soyisim, sifre))
                {
                    // Kullanýcý rolünü kaydet
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
                    await DisplayAlert("Hata", "Kullanýcý adý veya þifre hatalý!", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", ex.Message, "Tamam");
            }
        }
        else
        {
            await DisplayAlert("Hata", "Lütfen tam adýnýzý giriniz (isim ve soyisim)!", "Tamam");
        }
    }
    protected override bool OnBackButtonPressed()
    {
        return true;
    }
    private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
    {
        // Kullanýcý tarafýndan girilen metni al
        string enteredText = UsernameEntry.Text;

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
            UsernameEntry.Text = string.Join(" ", words);
        }
    }
    private string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString(); // 6 haneli doðrulama kodu
    }
}