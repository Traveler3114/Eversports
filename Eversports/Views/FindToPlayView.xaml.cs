using Eversports.Models;
using Eversports.Services;

namespace Eversports.Views;

public partial class FindToPlayView : ContentView
{
    private int lookingtoplay_id;

    public void SetID(int _lookingtoplay_id)
    {
        lookingtoplay_id = _lookingtoplay_id;
    }

    public void SetName(string name)
    {
        Name.Text = name;
    }
    public void SetSurname(string surname)
    {
        Surname.Text = surname;
    }
    public void SetEmail(string email)
    {
        Email.Text = email;
    }
    public void SetCountry(string country)
	{
		Country.Text = country;
	}
    public void SetCity(string city)
    {
        City.Text = city;
    }
    public void SetDate(string date)
    {
        Date.Text = date;
    }

    public void SetFromTime(string fromTime)
    {
        FromTime.Text = fromTime;
    }

    public void SetToTime(string toTime)
    {
        ToTime.Text = toTime;
    }

    public void SetSports(string sports)
    {
        Sports.Text = sports;
    }


    private string page;
    private readonly LookingToPlayService _lookingToPlayService;
    public FindToPlayView(string _page,int id,string country,string city,string name,string surname,string email,string date,string toTime,string fromTime,string sports)
	{
		InitializeComponent();
        page = _page;
        _lookingToPlayService=new LookingToPlayService();
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