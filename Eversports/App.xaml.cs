using System.Text.Json;
using System.Text;
using System;
using Eversports.Shells;

namespace Eversports
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CheckTokenValidity();
        }



        protected override Window CreateWindow(IActivationState? activationState)
        {
            var isUserLoggedIn = SecureStorage.Default.GetAsync("StayLoggedIn").Result;

            if (isUserLoggedIn == "true")
            {
                return new Window(new AppShellMain()); // Main app shell
            }
            else
            {
                return new Window(new AppShellLogin()); // Login shell
            }
        }



        public void SetToAppShellMain()
        {
            // Set the login state to Secure Storage
            Windows[0].Page = new AppShellMain();
        }

        public void SetToAppShellLogin()
        {
            // Remove the login state from Secure Storage
            Windows[0].Page = new AppShellLogin();
        }

        public async Task CheckTokenValidity()
        {

            var sendingData = new
            {
                action = "verifyToken",
                jwt = await SecureStorage.Default.GetAsync("JWTToken"),
            };
            var client = new HttpClient();
            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost/EversportsAPI/",content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var deserializedResponse=JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
            if (deserializedResponse["status"] == "error")
            {
                SecureStorage.Default.Remove("StayLoggedIn");
            }
        }
    }
}
