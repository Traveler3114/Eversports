using Eversports.Models;
using Eversports.Services;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eversports.Pages;
//BITNO!!!!!!
//IMENA VARIJABLE U KLASAMA U C# MORAJU BITI JEDNAKA IMENIMA U BAZI PODATAKA VELIKA I MALA SLOVA SU BITNA
public partial class RegistrationPage : ContentPage
{
    private readonly UserService _userService;
    public RegistrationPage()
    {
        InitializeComponent();
        _userService = new UserService();
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
                await RegisterUser();
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
    public async Task RegisterUser()
    {

        UserInfo user = new UserInfo()
        {
            name = NameEntry.Text,
            surname = SurnameEntry.Text,
            password = PasswordEntry.Text,
            email = EmailEntry.Text,
        };

        var response=await _userService.RegisterUser(user);


        //// Check if registration was successful
        if (response != null && response.ContainsKey("status"))
        {
            if (response["status"] == "success")
            {
                await DisplayAlert("Success", response["message"], "OK");
            }
            else
            {
                await DisplayAlert("Error", response["message"], "OK");
            }
        }
    }
}
