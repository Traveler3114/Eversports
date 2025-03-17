using Eversports.Model;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eversports;
//BITNO!!!!!!
//IMENA VARIJABLE U KLASAMA U C# MORAJU BITI JEDNAKA IMENIMA U BAZI PODATAKA VELIKA I MALA SLOVA SU BITNA
public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();
    }

    // Make the event handler async
    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(NameEntry.Text) || string.IsNullOrEmpty(SurnameEntry.Text) || string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text) || string.IsNullOrEmpty(ConfirmPasswordEntry.Text))
        {
            await DisplayAlert("Registration failed", "You didn't enter all the necessary data.", "OK");
        }
        else
        {
            if (PasswordEntry.Text == ConfirmPasswordEntry.Text)
            {
                await RegisterUserAsync();
            }
            else
            {
                await DisplayAlert("Registration failed", "Passwords dont match", "OK");
            }
        }
    }

    //async se koristi kako bi funkcija bila asynchronous sto znaci da se funkcija izvrsava bez blokiranja ostatka aplikacije
    //await se koristi kako bi se pricekalo izvrsenje funkcije bez blokiranja ostatka aplikacije
    //Task represntira operaciju koja je pokrenta u pozadini te ce jednom zavrsiti 
    public async Task RegisterUserAsync()
    {
        // Initialize HttpClient
        var client = new HttpClient();

        UserInfo user = new UserInfo()
        {
            name = NameEntry.Text,
            surname = SurnameEntry.Text,
            password = PasswordEntry.Text,
            email = EmailEntry.Text,
        };

        var registrationData = new
        {
            action = "register", // This tells the backend it's a registration request
            user = user // Include the UserInfo object here
        };

        // konvertiramo objekt user u json
        var jsonContent = JsonSerializer.Serialize(registrationData);
        //Ovako izgleda json file
        //{
        //    "name": "John",
        //    "surname": "Doe",
        //    "email": "john@example.com",
        //    "password": "12345"
        //}



        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        try
        {
            // Send JSON to PHP backend
            var response = await client.PostAsync("http://localhost/auth_app/EversportsAPI.php", content);
            //citamo json response kao string te ga kasnije pretvaramo u C# dictionary
            var responseContent = await response.Content.ReadAsStringAsync();
            ////konvertiramo PHP json u C# dictionary
            var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);


            //// Check if registration was successful
            if (jsonResponse != null && jsonResponse.ContainsKey("status"))
            {
                if (jsonResponse["status"] == "success")
                {
                    await DisplayAlert("Success", jsonResponse["message"], "OK");
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
