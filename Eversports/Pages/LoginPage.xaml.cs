using Eversports.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;
using Eversports.Services;
using Eversports.Resources;

namespace Eversports.Pages;


//! se koristi kako bi se supresirao warning da je moguci operator null, ! kaze da definitivno nije
//? se koristi kao if i provjerava da li operator null tocnije da li je operator validan;
public partial class LoginPage : ContentPage
{

    private readonly UserService _userService;
    public LoginPage()
	{
		InitializeComponent();
        _userService = new UserService();
    }


    private async void OnLoginButtonClicked(object sender, EventArgs e)
	{
        if (string.IsNullOrEmpty(EmailEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
        {
            await DisplayAlert("Error", Strings.DataNotEntered, "OK");
        }
        else
        {
#if DEBUG
            if (EmailEntry.Text == "admin" && PasswordEntry.Text == "admin")
            {
                
                return;
            }
#endif
            await LoginUser();
        }  
    }


    public async Task LoginUser()
    {
        
        UserInfo user = new UserInfo()
        {
            password = PasswordEntry.Text,
            email = EmailEntry.Text,
        };
        try
        {
            var response = await _userService.LoginUser(user);

            if (response != null && response.ContainsKey("status"))
            {
                if (response["status"] == "success")
                {
                    string jwt = response["token"];
                    await SecureStorage.Default.SetAsync("JWTToken", jwt);


                    ((App?)Application.Current!).SetToAppShellMain();
                }
                else
                {
                    await DisplayAlert("Error", response["message"], "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}