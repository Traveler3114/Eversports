﻿using System;
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
            _client = new HttpClient();
            url = "http://traveler3114.ddns.net/EversportsAPI/";
        }

        public async Task<Dictionary<string, string>?> SendMessage(string action, int lookingtoplay_id, string message)
        {
            var sendingData = new
            {
                action = action,
                jwt = await SecureStorage.Default.GetAsync("JWTToken"),
                lookingtoplay_id = lookingtoplay_id,
                message = message
            };
            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }

        public async Task<Response?> GetAllMessages(string action,int lookingtoplay_id)
        {
            var sendingData = new
            {
                action = action,
                lookingtoplay_id = lookingtoplay_id,
            };

            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);

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
