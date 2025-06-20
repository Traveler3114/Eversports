using Eversports.Models;
using Eversports.Views;
using System.Xml.Linq;
using Eversports.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Diagnostics.Metrics;
using System;
using System.Text.Json;
using System.Text;

namespace Eversports.Pages;

public partial class AdminPage : ContentPage
{
    private readonly LookingToPlayService _lookingToPlayService;
    private readonly UserService _userService;
    public AdminPage()
	{
        InitializeComponent();
        _lookingToPlayService = new LookingToPlayService();
        _userService = new UserService();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ShowAllLookingToPlay();
        await ShowAllUsers();
    }

    public async Task ShowAllUsers()
    {
        UsersScrollView.Children.Clear();
        try
        {
            var response = await _userService.GetUserData(true);
            if (response.status == "success")
            {
                var UserList = response.obj as List<UserInfo>;
                foreach (var user in UserList)
                {
                    UsersScrollView.Children.Add(new UserView(user.id, user.name, user.surname, user.email, async () => await ShowAllUsers()));
                }
            }
        }
        catch (Exception ex) 
        { 
            await DisplayAlert("OK",ex.Message,"OK");
        }
    }


    public async Task ShowAllLookingToPlay()
    {
        FindToPlayScrollView.Children.Clear();

        // Declare all needed variables here
        int id = 0;
        string country = "";
        string city = "";
        int userId = 0;
        string name = "";
        string surname = "";
        string email = "";
        string date = "";
        string fromTime = "";
        string toTime = "";
        string sportsString = "";

        try
        {
            var response = await _lookingToPlayService.GetAllLookingToPlay();
            if (response.status == "success")
            {
                XDocument doc = response.obj as XDocument;
                foreach (XElement item in doc.Descendants("item"))
                {
                    id = Convert.ToInt32(item.Element("id")!.Value);
                    country = item.Element("country")!.Value;
                    city = item.Element("city")!.Value;
                    userId = Convert.ToInt32(item.Element("user_id")!.Value);
                    Response response1 = await _userService.GetUserData(false);
                    name = (response1.obj as UserInfo).name;
                    surname = (response1.obj as UserInfo).surname;
                    email = (response1.obj as UserInfo).email;

                    foreach (XElement availabledatetimes in item.Descendants("availabledatetimes"))
                    {
                        foreach (XElement availabledatetime in availabledatetimes.Descendants("availabledatetime"))
                        {
                            date = availabledatetime.Element("Date")!.Value;
                            fromTime = availabledatetime.Element("FromTime")!.Value;
                            toTime = availabledatetime.Element("ToTime")!.Value;
                        }
                    }

                    List<string> sports = new List<string>();
                    foreach (XElement choosenSports in item.Descendants("choosenSports"))
                    {
                        foreach (XElement sport in choosenSports.Descendants("sport"))
                        {
                            sports.Add(sport.Element("name")!.Value);
                        }
                    }
                    sportsString = string.Join(", ", sports);
                    FindToPlayScrollView.Children.Add(new FindToPlayView("AdminPage", id, country, city, name, surname, email, date, fromTime, toTime, sportsString, async () => await ShowAllLookingToPlay()));
                }
            }
            else
            {
                await DisplayAlert("Error", response.obj as String, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "AdminPage:" + ex.Message, "OK");
        }
    }

    private async void OnGetPDFClicked(object sender, EventArgs e)
    {

        var url = "http://traveler3114.ddns.net/EversportsAPI/ExportInPDF.php"; // Adjust if needed
        var _client = new HttpClient();


        try
        {
            // Send GET or POST depending on your PHP implementation
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var pdfBytes = await response.Content.ReadAsByteArrayAsync();

            var fileName = "tables.pdf";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string filePath = Path.Combine(desktopPath, fileName);

            File.WriteAllBytes(filePath, pdfBytes);

            await DisplayAlert("Success", $"PDF saved to: {filePath}", "Open");

            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }


}