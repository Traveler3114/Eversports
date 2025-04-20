using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            url = "http://traveler3114.ddns.net/EversportsAPI/";
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
            return await SendUserData("setUserData", user);
        }

        public async Task<Response?> GetUserData(string action, int? userId = null)
        {
            var sendingData = new
            {
                action = action,
                jwt = await SecureStorage.Default.GetAsync("JWTToken"),
                user_id = userId
            };

            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<Response>(responseContent);
            deserializedResponse.obj = JsonSerializer.Deserialize<UserInfo>(deserializedResponse.obj.ToString());

            return deserializedResponse;
        }




        private async Task<Dictionary<string, string>?> SendUserData(string action, UserInfo user)
        {

            var sendingData = new
            {
                action=action,
                jwt= await SecureStorage.Default.GetAsync("JWTToken"),
                user =user
            };
            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }

        public async Task<Response?> GetAllUsers()
        {
            var sendingData = new
            {
                action = "GetAllUsers",
            };

            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonSerializer.Deserialize<Response>(responseContent);
            deserializedResponse.obj = JsonSerializer.Deserialize<List<UserInfo>>(deserializedResponse.obj.ToString());


            return deserializedResponse;
        }

        public async Task DeleteUser(int user_id)
        {
            var sendingData = new
            {
                action = "DeleteUser",
                user_id = user_id,
                jwt = await SecureStorage.Default.GetAsync("JWTToken")
            };

            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
        }

        public async Task<Dictionary<string,string>?> VerifyToken()
        {
            var sendingData = new
            {
                action = "verifyToken",
                jwt = await SecureStorage.Default.GetAsync("JWTToken")
            };

            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }
    }
}
