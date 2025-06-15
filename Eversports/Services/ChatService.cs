using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Eversports.Models;

namespace Eversports.Services
{

    class ChatService
    {
        private readonly HttpClient _client;
        private string url;
        public ChatService() {
            var handler = new HttpClientHandler()
            {
                // Disable SSL certificate validation
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _client = new HttpClient(handler);
            url = "https://localhost/EversportsAPI/";
        }

        public async Task<Dictionary<string, string>?> SendMessage(int lookingtoplay_id, string message)
        {
            var sendingData = new
            {
                jwt = await SecureStorage.Default.GetAsync("JWTToken"),
                lookingtoplay_id = lookingtoplay_id,
                message = message
            };
            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://localhost/EversportsAPI/Messaging/SendMessage.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }

        public async Task<Response?> GetMessages(int lookingtoplay_id)
        {
            string jwt = await SecureStorage.Default.GetAsync("JWTToken");
            var response = await _client.GetAsync($"https://localhost/EversportsAPI/Messaging/GetMessages.php?lookingtoplayid={lookingtoplay_id}&jwt={jwt}");

            var responseContent = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<Response>(responseContent);

            if (deserializedResponse.status == "success")
            {
                deserializedResponse.obj = XDocument.Parse(deserializedResponse.obj.ToString());
            }
            //else
            //{
            //    deserializedResponse.obj = deserializedResponse.obj.ToString();
            //}
            return deserializedResponse;
        }
    }
}
