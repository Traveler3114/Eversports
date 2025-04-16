using System.Text.Json;
using System.Text;
using System;
using Eversports.Shells;
using Eversports.Pages;

namespace Eversports
{
    public partial class App : Application
    {
        private bool isTokenValid;
        public App()
        {
            InitializeComponent();
        }


        //FUNKCIJA SE POZIVA
        protected override async void OnStart()
        {
            
            base.OnStart();
            //Windows[0].Page = new ChatPage(1);
            isTokenValid = await CheckTokenValidity();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            if (isTokenValid)
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
        public void SetToChatPage(int lookingtoplay_id)
        {
            Windows[0].Page = new ChatPage(lookingtoplay_id);
        }

        public async Task<bool> CheckTokenValidity()
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
                return false;
            }
            return true;
        }
    }
}
