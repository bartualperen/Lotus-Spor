using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lotus_Spor.Services
{
    public static class VatanSmsService
    {
        private static readonly string _apiId = Secrets.VatanApiId;
        private static readonly string _apiKey = Secrets.VatanApiKey;
        private static readonly string _sender = Secrets.VatanSender;

        private const string Endpoint = "https://api.vatansms.net/api/v1/otp";

        public static async Task<bool> SendOtpAsync(string phone90, string code, CancellationToken ct = default)
        {
            var payload = new
            {
                api_id = _apiId,
                api_key = _apiKey,
                sender = _sender,
                message_type = "turkce",
                phones = new[] { phone90 },  // Gerçekten array olarak gidiyor
                message = $"{code}"
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync(Endpoint, content, ct);

            string body = await response.Content.ReadAsStringAsync(ct);
            return (response.StatusCode == System.Net.HttpStatusCode.OK);
        }
    }
}
