using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Eversports.Models;
using System.Security.Cryptography.X509Certificates;

namespace Eversports.Services
{
    class UserService
    {


        private readonly HttpClient _client;
        private string url;
        public UserService()
        {
            var handler = new HttpClientHandler()
            {
                // Disable SSL certificate validation
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            _client = new HttpClient(handler);
            url = "https://traveler3114.ddns.net/EversportsAPI/";
        }

        public async Task<Dictionary<string, string>?> RegisterUser(UserInfo user)
        {
            var sendingData = new
            {
                user = user
            };
            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://traveler3114.ddns.net/EversportsAPI/UserFunctions/Register.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }

        public async Task<Dictionary<string, string>?> LoginUser(UserInfo user)
        {
            var sendingData = new
            {
                user = user
            };
            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://traveler3114.ddns.net/EversportsAPI/UserFunctions/Login.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }


        public async Task<Dictionary<string, string>?> SetUserData(UserInfo user)
        {
            var sendingData = new
            {
                jwt = await SecureStorage.Default.GetAsync("JWTToken"),
                user = user
            };
            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://traveler3114.ddns.net/EversportsAPI/UserFunctions/SetUserData.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        }

        public async Task<Response?> GetUserData(int? howmany = null)
        {
            var jwt = await SecureStorage.Default.GetAsync("JWTToken");

            // Build the URL with the JWT and user_id (only if userId is not null)
            var url = $"https://traveler3114.ddns.net/EversportsAPI/UserFunctions/GetUserData.php?jwt={jwt}";
            if (howmany.HasValue)
            {
                url += $"&howmany={howmany}";
            }

            // Make the GET request
            var response = await _client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response
            var deserializedResponse = JsonSerializer.Deserialize<Response>(responseContent);
            
            // Deserialize the object part (user info) if available
            if (deserializedResponse != null && deserializedResponse.obj != null)
            {
                if (howmany==1)
                {
                    deserializedResponse.obj = JsonSerializer.Deserialize<UserInfo>(deserializedResponse.obj.ToString());                   
                }
                else
                {
                    deserializedResponse.obj = JsonSerializer.Deserialize<List<UserInfo>>(deserializedResponse.obj.ToString());
                }
            }

            return deserializedResponse;
        }


        public async Task DeleteUser(int user_id)
        {
            var sendingData = new
            {
                user_id = user_id,
                jwt = await SecureStorage.Default.GetAsync("JWTToken")
            };

            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://traveler3114.ddns.net/EversportsAPI/UserFunctions/DeleteUser.php", content);
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
