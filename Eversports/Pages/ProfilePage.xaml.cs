using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Eversports.Model;

namespace Eversports;



public partial class ProfilePage : ContentPage
{

    private UserInfo? user;

    //konstruktor ne moze biti async
	public ProfilePage()
	{
		InitializeComponent();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetUserData();
    }

    public async Task GetUserData()
	{
        var client = new HttpClient();
        user = new UserInfo()
        {
            email = await SecureStorage.Default.GetAsync("UserEmail") ?? string.Empty
        };



        SendingData sendingData = new SendingData()
        {
            action = "getData",
            user = user,
        };

        var jsonContent = JsonSerializer.Serialize(sendingData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync("http://localhost/auth_app/EversportsAPI.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            //konvertiramo PHP json u C# dictionary
            var jsonResponse = JsonSerializer.Deserialize<ReceivingData>(responseContent);


            if (jsonResponse != null && jsonResponse.status == "success")
            {
                user = jsonResponse.user;
                NameEntry.Text = user.name;
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
            await DisplayAlert("Error", "Something went wrong: " + ex.Message, "OK");
        }
    }



    //
    //
    //
    //
    //
    //
    //napravi mjenjanje korisnicik podataka!!!!
    public async Task SetUserData()
    {
        var client = new HttpClient();

        UserInfo changedUser = new UserInfo()
        {
            id=user!.id,
            name = NameEntry.Text,
            surname = SurnameEntry.Text,
            email = EmailEntry.Text,
            password = PasswordEntry.Text,
        };


        SendingData sendingData = new SendingData()
        {
            action = "setData",
            user = user,
        };

        var jsonContent = JsonSerializer.Serialize(sendingData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync("http://localhost/auth_app/EversportsAPI.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();


            //konvertiramo PHP json u C# dictionary
            var jsonResponse = JsonSerializer.Deserialize<Dictionary<string,string>>(responseContent);
            if (jsonResponse != null && jsonResponse.ContainsKey("status"))
            {
                if (jsonResponse["status"] == "success")
                {
                    await DisplayAlert("Success", jsonResponse["message"], "OK");
                    user = changedUser;
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

    public async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(NameEntry.Text)|| string.IsNullOrEmpty(SurnameEntry.Text)|| string.IsNullOrEmpty(EmailEntry.Text)|| string.IsNullOrEmpty(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "Some required fields are empty ", "OK");
        }
        else
        {
            await SetUserData();
        }   
    }
}