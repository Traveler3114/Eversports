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

    public FindToPlayView()
	{
		InitializeComponent();
	}

    private void OnTapped(object sender, EventArgs e)
    {
        // Do something when the view is clicked
        ((App)Application.Current!)?.SetToChatPage(lookingtoplay_id);

        // For example, you could send a message to the ViewModel, raise an event, etc.
    }
}