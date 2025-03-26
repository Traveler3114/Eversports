using Eversports.Models;
using Eversports.Views;
using Eversports.Services;
using CsvHelper;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eversports.Pages;

public partial class LookingToPlayPage : ContentPage
{
    //stranica gdje korisnici mogu postavljati trazenja igraca
    //lista na kojoj pise koji korisnik trazi igraca, koji sport i koja lokacija,
    //takoder taj korisnik moze postaviti odredenjeno vrijeme, na primjer subota ili nedelja izmedju 10-12

    private readonly LookingToPlayService _lookingToPlayService;
    List<string> _choosenSports { get; set; } = new List<string>();
    List<AvailableDateTime> _availableDateTimes { get; set; }=new List<AvailableDateTime>();
    public LookingToPlayPage()
    {
        InitializeComponent();
        _lookingToPlayService = new LookingToPlayService();

        CountryPicker.ItemsSource = new List<string>
        {
            "Croatia"
        };
        CityPicker.ItemsSource = new List<string>
        {
            "Zagreb"
        };

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

    private void RemoveSport(string sport)
    {
        _choosenSports.Remove(sport);  // This removes the sport from the list.
    }

    private void RemoveDateTime(AvailableDateTime availableDateTime)
    {
        _availableDateTimes.Remove(availableDateTime);  // This removes the sport from the list.
    }

    private void OnSportSelected(object sender, EventArgs e)
    {
        if (!(_choosenSports.Count > 5))
        {
            string selectedSport = SportPicker.SelectedItem.ToString()!;
            if (!_choosenSports.Contains(selectedSport))
            {
                SportStackLayout.Children.Add(new ItemView(selectedSport, () => RemoveSport(selectedSport)));
                _choosenSports.Add(selectedSport);
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
            ToTime = ToTimePicker.Time.ToString(@"hh\:mm")
        };
        if (!_availableDateTimes.Contains(dateTime))
        {
            _availableDateTimes.Add(dateTime);
            string dateTimeEntry = $"{dateTime.Date} from {dateTime.FromTime} to {dateTime.ToTime}";
            TimeStackLayout.Children.Add(new ItemView(dateTimeEntry, ()=>RemoveDateTime(dateTime)));
        }
        else
        {
            DisplayAlert("Error", "You already selected that date and time", "Ok");
        }
    }

    public async void OnCreateButtonClicked(object sender, EventArgs e)
    {


        LookingToPlay lookingToPlay = new LookingToPlay()
        {
            availableDateTimes = _availableDateTimes,
            country = CountryPicker.SelectedItem.ToString(),
            city = CityPicker.SelectedItem.ToString(),
            detailedLocation = DetailedLocationEntry.Text,
            choosenSports=_choosenSports,
            description = DescriptionEntry.Text,
            user_id = Convert.ToInt32(await SecureStorage.Default.GetAsync("UserID"))
        };

        try
        {
            var response = await _lookingToPlayService.AddLookingToPlay(lookingToPlay);
        }
        catch (Exception ex) 
        {
            await DisplayAlert("Error", "Error in " + ex.Message, "OK");
        }
    }
}
