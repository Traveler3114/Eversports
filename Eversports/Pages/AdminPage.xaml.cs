using Eversports.Models;
using Eversports.Views;
using System.Xml.Linq;
using Eversports.Services;

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
        await GetAll();
    }


    public async Task GetAll()
    {
        FindToPlayScrollView.Children.Clear();
        try
        {
            var response = await _lookingToPlayService.GetAllLookingToPlay();
            if (response.status == "success")
            {
                XDocument doc = response.obj as XDocument;
                //await DisplayAlert("A", doc.ToString(), "OK");
                FindToPlayView view = new FindToPlayView("AdminPage");

                foreach (XElement item in doc.Descendants("item"))
                {
                    view.SetID(Convert.ToInt32(item.Element("id")!.Value));
                    view.SetCountry(item.Element("country")!.Value);
                    view.SetCity(item.Element("city")!.Value);
                    int userId = Convert.ToInt32(item.Element("user_id")!.Value);
                    Response response1 = await _userService.GetUserData("getUserData", userId);
                    view.SetName((response1.obj as UserInfo).name);
                    view.SetSurname((response1.obj as UserInfo).surname);
                    view.SetEmail((response1.obj as UserInfo).email);
                    foreach (XElement availabledatetimes in item.Descendants("availabledatetimes"))
                    {
                        foreach (XElement availabledatetime in availabledatetimes.Descendants("availabledatetime"))
                        {
                            view.SetDate(availabledatetime.Element("Date")!.Value);
                            view.SetFromTime(availabledatetime.Element("FromTime")!.Value);
                            view.SetToTime(availabledatetime.Element("ToTime")!.Value);
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
                    string sportsString = string.Join(", ", sports);

                    view.SetSports(sportsString);
                }
                FindToPlayScrollView.Children.Add(view);

                //await DisplayAlert("XML Response", doc.ToString(), "OK");
            }
            else
            {
                await DisplayAlert("Error", response.obj as String, "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "FindToPlayPage:" + ex.Message, "OK");
        }
    }
}