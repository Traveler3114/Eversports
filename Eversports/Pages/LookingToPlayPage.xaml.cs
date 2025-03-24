using Eversports.Models;
using Eversports.Views;
using Eversports.Services;
using CsvHelper;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Eversports.Pages;

public partial class LookingToPlayPage : ContentPage
{
    //stranica gdje korisnici mogu postavljati trazenja igraca
    //lista na kojoj pise koji korisnik trazi igraca, koji sport i koja lokacija,
    //takoder taj korisnik moze postaviti odredenjeno vrijeme, na primjer subota ili nedelja izmedju 10-12
    List<string> ChoosenSports { get; set; } = new List<string>();
    List<AvailableDateTime> availableDateTimes { get; set; }=new List<AvailableDateTime>();
    public LookingToPlayPage()
    {
        InitializeComponent();
        SportPicker.ItemsSource = new List<string>
        {
            "Running",
            "Hiking",
            "Tennis",
            "Table Tennis",
            "Cycling",
            "Football",
            "Basketball",
            "Cricket",
            "Baseball",
            "Golf",
            "Volleyball",
            "Badminton",
            "Boxing",
            "Swimming",
            "Athletics",
            "Handball",
            "Gymnastics",
            "Skiing and Snowboarding",
            "Surfing",
            "Skateboarding",
        };
        BindingContext = this;
    }

    private void OnSportSelected(object sender, EventArgs e)
    {
        if (!(ChoosenSports.Count > 5))
        {
            string selectedSport = SportPicker.SelectedItem.ToString()!;
            if (!ChoosenSports.Contains(selectedSport))
            {
                SportStackLayout.Children.Add(new ItemView(selectedSport));
                ChoosenSports.Add(selectedSport);
            }
            else
            {
                DisplayAlert("Error", "You already choose " + selectedSport, "OK");
            }
        }
        else
        {
            DisplayAlert("Too many sports", "I think you have already choosen to many sports, don't you? ", "OK");
        }
    }

    private void OnAddDateTimeClicked(object sender, EventArgs e)
    {

        AvailableDateTime dateTime = new AvailableDateTime
        {
            Date = DatePicker.Date.ToString("d"),
            FromTime = FromTimePicker.Time.ToString(@"hh\:mm"),
            ToTime = FromTimePicker.Time.ToString(@"hh\:mm")
        };
        if (!availableDateTimes.Contains(dateTime))
        {
            availableDateTimes.Add(dateTime);
            string dateTimeEntry = $"{dateTime.Date} from {dateTime.FromTime} to {dateTime.ToTime}";
            TimeStackLayout.Children.Add(new ItemView(dateTimeEntry));
        }
        else
        {
            DisplayAlert("Error", "You already selected that date and time", "Ok");
        }
    }
}
