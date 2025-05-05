using MySql.Data.MySqlClient;

namespace Lotus_Spor
{
    class Models
    {
    }
    public class OdemeModel
    {
        public string musteri_adi { get; set; }
        public int musteri_id { get; set; }
        public string isim { get; set; }
        public string soyisim { get; set; }
        public string antrenor { get; set; }
        public string odeme_donemi { get; set; }
        public string odeme_tarihi { get; set; }
        public decimal toplam_odeme { get; set; }
        public decimal seans_ucreti { get; set; }
        public string odeme_durumu { get; set; }
        public int yapilan_ders { get; set; }
        public int yapilmayan_ders { get; set; }
        public int beklenen_ders { get; set; }
    }
    public class Kisi
    {
        public string Name { get; set; }
    }
    public class Customer
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string AdditionalInfo { get; set; }
        public string Notlar { get; set; }
        public string Telefon { get; set; }
        public string KayitTarihi { get; set; }
        public int Ucret { get; set; }
        public string Aktiflik { get; set; }
        public int DersSayisi { get; set; }
    }
    public class OlcuModel
    {
        public string Isim { get; set; }
        public DateTime Tarih { get; set; } 
        public decimal Kilo { get; set; }
        public decimal YagOrani { get; set; }
        public decimal SuOrani { get; set; }
        public decimal KasOrani { get; set; }
        public decimal BMI { get; set; }
        public decimal Omuz { get; set; }
        public decimal Biceps { get; set; }
        public decimal Gogus { get; set; }
        public decimal Bel { get; set; }
        public decimal Karin { get; set; }
        public decimal Kalca { get; set; }
        public decimal Bacak { get; set; }
        public decimal Kalf { get; set; }
    }
    public class Lesson
    {
        public string Day { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Client { get; set; }
        public string Grup { get; set; }
    }
    public class DoluSeans
    {
        public string Gun { get; set; }
        public string BaslangicSaat { get; set; }
        public string HizmetTuru { get; set; }
    }
    class Database
    {
        private static string connectionString = ConnectionString.connectionString;
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
    public class LoginManager
    {
        public static string LoggedInUser { get; private set; }
        public static string LoggedInUser2 { get; private set; }
        public static string UserRole { get; private set; }
        public static string Gender { get; private set; }
        public static int LoggedInUserId { get; private set; }
        public static bool IsSpecialUser(string isim, string soyisim)
        {
            string query = "SELECT COUNT(*) FROM yoneticiler WHERE isim = @FirstName AND soyisim = @LastName";

            using (var connection = Database.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", isim);
                    command.Parameters.AddWithValue("@LastName", soyisim);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        public static bool Login(string isim, string soyisim, string sifre)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                // Önce yöneticiler tablosunu kontrol et
                string yoneticiQuery = "SELECT 'yonetici' AS rol, id FROM yoneticiler WHERE isim = @isim AND soyisim = @soyisim AND sifre = @sifre;";
                using (var yoneticiCmd = new MySqlCommand(yoneticiQuery, conn))
                {
                    yoneticiCmd.Parameters.AddWithValue("@isim", isim);
                    yoneticiCmd.Parameters.AddWithValue("@soyisim", soyisim);
                    yoneticiCmd.Parameters.AddWithValue("@sifre", sifre);

                    using (var reader = yoneticiCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            LoggedInUser = isim;
                            LoggedInUser2 = soyisim;
                            LoggedInUserId = Convert.ToInt32(reader["id"]);
                            UserRole = "yonetici";
                            return true;
                        }
                    }
                }

                // Eğer yönetici değilse, müşteriler tablosunu kontrol et
                string musteriQuery = "SELECT 'musteri' AS rol, cinsiyet, id FROM musteriler WHERE isim = @isim AND soyisim = @soyisim AND sifre = @sifre;";
                using (var musteriCmd = new MySqlCommand(musteriQuery, conn))
                {
                    musteriCmd.Parameters.AddWithValue("@isim", isim);
                    musteriCmd.Parameters.AddWithValue("@soyisim", soyisim);
                    musteriCmd.Parameters.AddWithValue("@sifre", sifre);

                    using (var reader = musteriCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            LoggedInUser = isim;
                            LoggedInUser2 = soyisim;
                            Gender = reader["cinsiyet"].ToString();
                            UserRole = "musteri";
                            LoggedInUserId = Convert.ToInt32(reader["id"]);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static void Logout()
        {
            LoggedInUser = null;
            UserRole = null;
            LoggedInUserId = 0;
        }
        // < YENİ > OTP belleği (basit, 3 dk. süreli)
        private static readonly Dictionary<string, (string Code, DateTime Expire)> _otpCache
            = new();

        private static readonly object _lock = new();     // eş-zamansız güncelleme için

        /// <summary>Kimlik + şifre doğruysa SMS OTP gönderir ve true döndürür.</summary>
        public static async Task<bool> LoginWithOtpAsync(string isim, string soyisim,
                                                        string sifre, string phone90)
        {
            // 1) Önce normal doğrulama
            if (!Login(isim, soyisim, sifre))
                return false;

            // 2) 6 haneli kod üret
            var rnd = new Random();
            string code = rnd.Next(100000, 999999).ToString();

            // 3) SMS gönder
            bool ok = await Services.VatanSmsService.SendOtpAsync(phone90, $"Uygulamaya giriş yapmak için kodunuz: {code}");
            if (!ok) return false;

            // 4) Belleğe koy (3 dk geçerli)
            lock (_lock)
                _otpCache[phone90] = (code, DateTime.UtcNow.AddMinutes(3));

            return true;
        }

        /// <summary>Kullanıcının girdiği kodu doğrular.</summary>
        public static bool VerifyOtp(string phone90, string code)
        {
            lock (_lock)
            {
                if (!_otpCache.TryGetValue(phone90, out var entry))
                    return false;                          // kod hiç gönderilmemiş

                if (DateTime.UtcNow > entry.Expire)        // süresi doldu
                {
                    _otpCache.Remove(phone90);
                    return false;
                }

                bool match = entry.Code == code;
                if (match) _otpCache.Remove(phone90);      // tek kullanımlık
                return match;
            }
        }
    }
}
