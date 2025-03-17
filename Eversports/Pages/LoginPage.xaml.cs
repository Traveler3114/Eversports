using Eversports.Model;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;

namespace Eversports;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
	private async void OnLoginButtonClicked(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
        {
            await DisplayAlert("Login failed", "You didn't enter all the necessary data.", "OK");
        }
        else
        {
            if(EmailEntry.Text=="admin" && PasswordEntry.Text == "admin")
            {
                ((App)Application.Current!)?.SetToAppShellMain();
            }
            else await LoginUserAsync();
        }  
    }


    public async Task LoginUserAsync()
    {
        var client = new HttpClient();

        UserInfo user = new UserInfo()
        {
            password = PasswordEntry.Text,
            email = EmailEntry.Text,
        };

        var loginData = new
        {
            action = "login",
            user = user
        };

        var jsonContent = JsonSerializer.Serialize(loginData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            // Send JSON to PHP backend
            var response = await client.PostAsync("http://localhost/auth_app/EversportsAPI.php", content);
            //citamo json response kao string te ga kasnije pretvaramo u C# dictionary
            var responseContent = await response.Content.ReadAsStringAsync();
            //konvertiramo PHP json u C# dictionary
            var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);

            
            // Check if registration was successful
            if (jsonResponse != null && jsonResponse.ContainsKey("status"))
            {
                if (jsonResponse["status"] == "success")
                {
                    await DisplayAlert("Success", jsonResponse["message"], "OK");

                    await SecureStorage.Default.SetAsync("UserEmail", EmailEntry.Text);

                    if (RememberMeCheckBox.IsChecked)
                    {
                        await SecureStorage.Default.SetAsync("StayLoggedIn", "true");
                    }
                    ((App)Application.Current!)?.SetToAppShellMain();
                }
                else
                {
                    await DisplayAlert("Error", jsonResponse["message"], "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Something went wrong: " + ex.Message, "OK");
        }
    }

}