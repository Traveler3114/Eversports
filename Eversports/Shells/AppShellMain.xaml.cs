using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Eversports.Models;
using Microsoft.Maui.Controls;

namespace Eversports.Shells;

public partial class AppShellMain : Shell
{
    public AppShellMain()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CheckRole();
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        SecureStorage.Default.Remove("JWTToken");
        // Call the OnLogout method in the App class
        ((App)Application.Current!)?.SetToAppShellLogin();
    }


    public async Task CheckRole()
    {
        try
        {
            var sendingData = new
            {
                action = "VerifyToken",
                jwt = await SecureStorage.Default.GetAsync("JWTToken"),
            };

            var handler = new HttpClientHandler()
            {
                // Disable SSL certificate validation
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };


            var client = new HttpClient(handler);
            var jsonContent = JsonSerializer.Serialize(sendingData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://traveler3114.ddns.net/EversportsAPI/JWToken.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
            if (deserializedResponse["role"] == "user")
            {
                AdminFlyoutItem.IsVisible = false; // Hide the Admin FlyoutItem
            }
        }
        catch(Exception ex)
        {
            await DisplayAlert("OK", ex.Message, "OK");
        }
    }


    private void SetLanguageToHR(object sender, EventArgs e)
    {
        Localization.SetLanguage("hr");
        //var currentRoute = Shell.Current?.CurrentState?.Location.OriginalString;
        //DisplayAlert("OK", currentRoute, "OK");
    }
    private void SetLanguageToEN(object sender, EventArgs e)
    {
        Localization.SetLanguage("en");
    }
}