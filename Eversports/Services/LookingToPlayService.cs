using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eversports.Models;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Linq;
using System.Net.Http;

namespace Eversports.Services
{
    class LookingToPlayService
    {
        private readonly HttpClient _client;
        private string url;

        public LookingToPlayService()
        {
            _client = new HttpClient();
            url = "http://localhost/EversportsAPI/";
        }

        public async Task<Dictionary<string, string>?> AddLookingToPlay(LookingToPlay lookingToPlay)
        {

            var data = new
            {
                action = "AddLookingToPlay",
                lookingToPlay = lookingToPlay
            };

            var jsonContent = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("http://localhost/EversportsAPI/", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }

        public async Task<Response> GetLookingToPlay(string country,string city,List<AvailableDateTime> availableDateTimes,List<string> choosenSports)
        {
            var data = new
            {
                action = "GetLookingToPlay",
                country=country,
                city=city,
                availableDateTimes=availableDateTimes,
                choosenSports=choosenSports
            };


            var jsonContent = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url,content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<Response>(responseContent);

            if (deserializedResponse.status == "succes")
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
