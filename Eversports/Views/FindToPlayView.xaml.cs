namespace Eversports.Views;

public partial class FindToPlayView : ContentView
{
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
}