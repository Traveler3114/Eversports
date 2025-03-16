using Eversports.Model;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Eversports;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();
    }

    // Make the event handler async
    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        UserInfo user = new UserInfo()
        {
            Name = NameEntry.Text,
            Surname = SurnameEntry.Text,
            Password = PasswordEntry.Text,
            Email = EmailEntry.Text,
            Address = AddressEntry.Text,
            Phone = PhoneNumberEntry.Text
        };

        // Call RegisterUserAsync asynchronously
        if(PasswordEntry.Text==ConfirmPasswordEntry.Text)
        {
            await RegisterUserAsync(user);
        }
        else
        {
            await DisplayAlert("Registration failed", "Passwords dont match", "Not ok");
        }
    }

    public async Task RegisterUserAsync(UserInfo user)
    {
        // Initialize HttpClient
        var client = new HttpClient();

        // Prepare form data
        var postData = new Dictionary<string, string>
        {
            { "name", user.Name },
            { "surname", user.Surname },
            { "email", user.Email },
            { "password", user.Password },
        };

        // Send the POST request to PHP backend
        var content = new FormUrlEncodedContent(postData);

        try
        {
            // Replace with your actual PHP script URL
            var response = await client.PostAsync("http://localhost/auth_app/EversportsAPI.php", content);

            if (response.IsSuccessStatusCode)
            {
                // Read the response (success or failure message)
                var responseContent = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Registration succesful", responseContent, "Ok");
            }
            else
            {
                // If the response is not successful, show an error
                await DisplayAlert("Registration unsuccesful","Registration failed: " + response.StatusCode, "Not ok");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Registration not succesful", "Error: " + ex.Message, "Not ok");
        }
    }
}
