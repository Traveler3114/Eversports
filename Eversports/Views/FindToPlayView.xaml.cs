using Eversports.Models;
using Eversports.Services;

namespace Eversports.Views;

public partial class FindToPlayView : ContentView
{
    private int lookingtoplay_id;


    private string page;
    private readonly LookingToPlayService _lookingToPlayService;
    public FindToPlayView(string _page,int id,string country,string city,string name,string surname,string email,string date,string toTime,string fromTime,string sports)
	{
		InitializeComponent();
        lookingtoplay_id = id;
        page = _page;
        _lookingToPlayService=new LookingToPlayService();
        Name.Text = name;
        Email.Text = email;
        Surname.Text = surname;
        Country.Text = country;
        City.Text = city;
        Date.Text = date;
        ToTime.Text = toTime;
        FromTime.Text = fromTime;
        Sports.Text = sports;
	}

    private async void OnTapped(object sender, EventArgs e)
    {
        if (page == "FindToPlayPage")
        {
            ((App)Application.Current!)?.SetToChatPage(lookingtoplay_id);
        }
        else
        {
            await _lookingToPlayService.DeleteLookingToPlay(lookingtoplay_id);
        }
    }
}