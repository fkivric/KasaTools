using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace VolantMusteriDuzel.Class
{
    public class ApiClient
    {
        private static readonly HttpClient client = new HttpClient();
        private static string URL = "api/Iys/SendRecipient";
        public class IYSResponse {
            public string ResponseCode { get; set; }
            public string ResponseDesc { get; set; }
            public string PackageId { get; set; }
        }
        public class IYSRequest
        {
            public string userName { get; set; }
            public string password { get; set; }
            public string permissionType { get; set; }
            public string brandCode { get; set; }
            public string permissionStatus { get; set; }
            public string permissionSource { get; set; }
            public string isCheckBlackList { get; set; }
            public List<iysDatas> iysDatas { get; set; }

        }
        public class iysDatas
        {
            public string recipient { get; set; }
            public string receiveType { get; set; }
            public DateTime consentDate { get; set; }
        }
        public string SMSToken()
        {

            var login = new Dictionary<string, string>
               {
                   {"grant_type", "password"},
                   {"username", "yon.avm"},//TT Mesaj Tarfından Size Verilen Api Kullanıcı Adı
                   {"password", "xz@3RL*12Dvs"},//TT Mesaj Tarfından Size Verilen Api Şifre
               };


            using (HttpClient httpClient = new HttpClient())
            {

                var response = httpClient.PostAsync(Properties.Settings.Default.SmsUrl + "ttmesajToken", new FormUrlEncodedContent(login)).Result;

                if (response.IsSuccessStatusCode)
                {
                    Dictionary<string, string> tokenDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);

                    return tokenDetails.FirstOrDefault().Value;
                }
                else
                {
                    return "";
                }
            }
        }
        public async void SendRequest(string Onay,string Permission,string GSM,DateTime date)
        {
            // API URL'si
            string apiUrl = "https://restapi.ttmesaj.com/api/Iys/SendRecipient";

            // Gönderilecek JSON verisi
            var requestBody = new
            {
                userName = Properties.Settings.Default.SmsUser, // Kullanıcı adı
                password = Properties.Settings.Default.SmsPassword, // Şifre
                permissionType = Permission, // İzin türü
                brandCode = "611821", // Marka kodu
                permissionStatus = Onay, // İzin durumu
                permissionSource = 4, // Kaynak
                isCheckBlackList = "1", // Kara liste kontrolü
                iysDatas = new[]
                {
                    new
                    {
                        recipient = GSM, // Alıcı numarası
                        receiveType = "BIREYSEL", // Alıcı tipi
                        consentDate = date // Onay tarihi
                    }
                }
            };

            // JSON verisini oluştur
            var jsonContent = JsonConvert.SerializeObject(requestBody);

            // İçeriği oluştur
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // API'ye POST isteği gönder
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Properties.Settings.Default.SmsToken);
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarılı ise yanıtı al ve işle
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Başarılı: " + responseBody);
                }
                else
                {
                    // Hata durumunda
                    Console.WriteLine("Hata: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // İstek sırasında oluşan hatayı yakala
                Console.WriteLine("Hata: " + ex.Message);
            }
        }
        public async void Report(string Permission,string PackageId)
        { 
            // API URL'si
            string apiUrl = "https://restapi.ttmesaj.com/api/Iys/Report";

            // Gönderilecek JSON verisi
            var requestBody = new
            {
                userName = Properties.Settings.Default.SmsUser, // Kullanıcı adı
                password = Properties.Settings.Default.SmsPassword, // Şifre
                permissionType = Permission, // İzin türü
                brandCode = "*****", // Marka kodu
                packageId = PackageId, // İzin durumu            
            };

            // JSON verisini oluştur
            var jsonContent = JsonConvert.SerializeObject(requestBody);

            // İçeriği oluştur
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                // API'ye POST isteği gönder
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Başarılı ise yanıtı al ve işle
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Başarılı: " + responseBody);
                }
                else
                {
                    // Hata durumunda
                    Console.WriteLine("Hata: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                // İstek sırasında oluşan hatayı yakala
                Console.WriteLine("Hata: " + ex.Message);
            }
        }
    }
}
