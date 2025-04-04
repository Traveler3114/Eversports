﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Eversports.Models;

namespace Eversports.Services
{
    class UserService
    {
        private readonly HttpClient _client;
        private string url;
        public UserService()
        {
            _client = new HttpClient();
            url = "http://localhost/EversportsAPI/";
        }

        public async Task<Dictionary<string, string>?> RegisterUser(UserInfo user)
        {
            return await SendUserData("register", user);
        }

        public async Task<Dictionary<string, string>?> LoginUser(UserInfo user)
        {
            return await SendUserData("login", user);
        }

        public async Task<Dictionary<string, string>?> SetUserData(UserInfo user)
        {
            return await SendUserData("setData", user);
        }

        public async Task<Response?> GetUserData(UserInfo user, string action)
        {
            var sendingData = new
            {
                action = action,
                user = user
            };

            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<Response>(responseContent);

            // Directly deserialize obj into UserInfo
            deserializedResponse.obj = JsonSerializer.Deserialize<UserInfo>(deserializedResponse.obj.ToString());

            return deserializedResponse;
        }



        private async Task<Dictionary<string, string>?> SendUserData(string action, UserInfo user)
        {
            var sendingData = new
            {
                action,
                user
            };
            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }
    }
}
