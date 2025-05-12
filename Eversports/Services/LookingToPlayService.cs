using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eversports.Models;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Linq;
using System.Net.Http;
using System.Diagnostics.Metrics;

namespace Eversports.Services
{
    class LookingToPlayService
    {
        private readonly HttpClient _client;
        private string url;

        public LookingToPlayService()
        {
            var handler = new HttpClientHandler()
            {
                // Disable SSL certificate validation
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _client = new HttpClient(handler);
            url = "https://traveler3114.ddns.net/EversportsAPI/";
        }

        public async Task<Dictionary<string, string>?> AddLookingToPlay(LookingToPlay lookingToPlay)
        {

            var data = new
            {
                lookingToPlay = lookingToPlay,
                jwt=await SecureStorage.Default.GetAsync("JWTToken")
            };

            var jsonContent = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("https://traveler3114.ddns.net/EversportsAPI/LookingToPlayFunctions/AddLookingToPlay.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }
        public async Task<Dictionary<string, string>?> DeleteLookingToPlay(int lookingtoplay_id)
        {
            var data = new
            {
                lookingtoplay_id = lookingtoplay_id,
                jwt = await SecureStorage.Default.GetAsync("JWTToken")
            };

            var jsonContent = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://traveler3114.ddns.net/EversportsAPI/LookingToPlayFunctions/DeleteLookingToPlay.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Dictionary<string, string>?>(responseContent);
        }




        public async Task<Response> GetLookingToPlay(string country,string city,List<string> Dates, List<string> FromTimes, List<string> ToTimes, List<string> choosenSports)
        {
            var data = new
            {
                action = "GetLookingToPlay",
                country=country,
                city=city,
                Dates=Dates,
                FromTimes=FromTimes,
                ToTimes=ToTimes,
                choosenSports=choosenSports,
                jwt = await SecureStorage.Default.GetAsync("JWTToken")
            };


            var jsonContent = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://traveler3114.ddns.net/EversportsAPI/LookingToPlayFunctions/GetLookingToPlay.php", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<Response>(responseContent);

            if (deserializedResponse.status == "success")
            {
                deserializedResponse.obj = XDocument.Parse(deserializedResponse.obj.ToString());
            }
            else
            {
                deserializedResponse.obj = deserializedResponse.obj.ToString();
            }
            return deserializedResponse;
        }
        public async Task<Response> GetAllLookingToPlay()
        {
            var data = new
            {
                action = "GetAllLookingToPlay",
                jwt = await SecureStorage.Default.GetAsync("JWTToken")
            };

            var jsonContent = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://traveler3114.ddns.net/EversportsAPI/LookingToPlayFunctions/GetLookingToPlay.php", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<Response>(responseContent);

            if (deserializedResponse.status == "success")
            {
                deserializedResponse.obj = XDocument.Parse(deserializedResponse.obj.ToString());
            }
            else
            {
                deserializedResponse.obj = deserializedResponse.obj.ToString();
            }
            return deserializedResponse;
        }
    }
}
