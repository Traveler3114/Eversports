using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Eversports.Models;
using Eversports.Services;

namespace Eversports.Pages;



public partial class ProfilePage : ContentPage
{

    private UserInfo? user;

    private readonly UserService _userService;

    //konstruktor ne moze biti async
    public ProfilePage()
	{
		InitializeComponent();
        _userService = new UserService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //NameEntry.Text=await SecureStorage.Default.GetAsync("JWTToken");
        await GetUserData();
    }

    public async Task GetUserData()
    {

        try
        {
            var response = await _userService.GetUserData(false);
            if (response != null && response.status == "success")
            {
                
                user = (response.obj as UserInfo);
                NameEntry.Text = user!.name;
                SurnameEntry.Text = user.surname;
                EmailEntry.Text = user.email;
            }
            else
            {
                await DisplayAlert("Error", "User not found or error in the response", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "ProfilePage:" + ex.Message, "OK");
        }
    }

    public async Task SetUserData()
    {
        UserInfo changedUser = new UserInfo()
        {
            id = user!.id,
            name = NameEntry.Text,
            surname = SurnameEntry.Text,
            email = EmailEntry.Text,
            password = PasswordEntry.Text,
        };
        try
        {
            var response = await _userService.SetUserData(changedUser);
            if (response != null && response.ContainsKey("status"))
            {
                if (response["status"] == "success")
                {
                    await DisplayAlert("Success", "ProfilePage:" + response["message"], "OK");
                    user = changedUser;
                }
                else
                {
                    await DisplayAlert("Error", "ProfilePage:" + response["message"], "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "ProfilePage:" + ex.Message, "OK");
        }

    }

    public async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(NameEntry.Text)|| string.IsNullOrEmpty(SurnameEntry.Text)|| string.IsNullOrEmpty(EmailEntry.Text)|| string.IsNullOrEmpty(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "Some required fields are empty ", "OK");
        }
        else
        {
            await SecureStorage.Default.SetAsync("UserEmail", EmailEntry.Text);
            await SetUserData();
        }   
    }
}