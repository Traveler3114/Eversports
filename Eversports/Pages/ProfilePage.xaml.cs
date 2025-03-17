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
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        await GetUserData();
    }

    public async Task GetUserData()
	{
        var client = new HttpClient();
        user = new UserInfo()
        {
            email = await SecureStorage.Default.GetAsync("UserEmail") ?? string.Empty
        };


        var Data = new
        {
            action = "getData",
            user = user
        };

        var jsonContent = JsonSerializer.Serialize(Data);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        try
        {
            var response = await client.PostAsync("http://localhost/auth_app/EversportsAPI.php", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            await DisplayAlert("A", "Response content " + responseContent, "OK");
            //konvertiramo PHP json u C# dictionary
            var jsonResponse = JsonSerializer.Deserialize<Response>(responseContent);


            if (jsonResponse != null && jsonResponse.status == "success")
            {
                user = jsonResponse.user;
                await DisplayAlert("Succes", "User found", "OK");
                NameEntry.Text = jsonResponse.user.name;
                SurnameEntry.Text = jsonResponse.user.surname;
                EmailEntry.Text = jsonResponse.user.email;


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
    //public async Task SetUserData()
    //{
    //    var client = new HttpClient();

    //    UserInfo user = new UserInfo()
    //    {
    //        name = NameEntry.Text,
    //        surname = SurnameEntry.Text,
    //        email = EmailEntry.Text,
    //        password= PasswordEntry.Text,
    //    };


    //    var jsonContent= JsonSerializer.Serialize(user);
    //    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
    //    try
    //    {
    //        var response = await client.PostAsync("http://localhost/auth_app/EversportsAPI.php", content);
    //        var responseContent = await response.Content.ReadAsStringAsync();

            
    //        //konvertiramo PHP json u C# dictionary
    //        var jsonResponse = JsonSerializer.Deserialize<Response>(responseContent);

    //        if (jsonResponse != null && jsonResponse.Status == "success")
    //        {
    //            await DisplayAlert("Succes", "User data save succesfully", "OK");
    //        }
    //        else
    //        {
    //            await DisplayAlert("Error", "User not found or error in the response", "OK");
    //        }
    //    }
    //    catch (Exception ex) 
    //    {
    //        await DisplayAlert("Error", "Something went wrong: " + ex.Message, "OK");
    //    }
    //}

    public async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        //await SetUserData();
    }
}