﻿using Eversports.Models;
using Eversports.Services;
using Eversports.Views;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Xml.Linq;
//using Windows.System;

namespace Eversports.Pages;

public partial class FindToPlayPage : ContentPage
{
    private readonly LookingToPlayService _lookingToPlayService;
    private readonly UserService _userService;
    private readonly Dictionary<string, List<string>> _countryCities = new()
    {
    { "Afghanistan", new List<string> { "Kabul", "Kandahar", "Herat", "Mazar-i-Sharif", "Jalalabad" } },
    { "Albania", new List<string> { "Tirana", "Durrës", "Vlorë", "Shkodër", "Fier" } },
    { "Algeria", new List<string> { "Algiers", "Oran", "Constantine", "Annaba", "Blida" } },
    { "Argentina", new List<string> { "Buenos Aires", "Córdoba", "Rosario", "Mendoza", "La Plata" } },
    { "Australia", new List<string> { "Sydney", "Melbourne", "Brisbane", "Perth", "Adelaide" } },
    { "Austria", new List<string> { "Vienna", "Graz", "Linz", "Salzburg", "Innsbruck" } },
    { "Bangladesh", new List<string> { "Dhaka", "Chittagong", "Khulna", "Rajshahi", "Sylhet" } },
    { "Belgium", new List<string> { "Brussels", "Antwerp", "Ghent", "Charleroi", "Liège" } },
    { "Brazil", new List<string> { "São Paulo", "Rio de Janeiro", "Brasília", "Salvador", "Fortaleza" } },
    { "Canada", new List<string> { "Toronto", "Montreal", "Vancouver", "Calgary", "Ottawa" } },
    { "China", new List<string> { "Beijing", "Shanghai", "Guangzhou", "Shenzhen", "Chengdu" } },
    { "Colombia", new List<string> { "Bogotá", "Medellín", "Cali", "Barranquilla", "Cartagena" } },
    { "Croatia", new List<string> { "Zagreb", "Split", "Rijeka", "Osijek", "Dubrovnik" } },
    { "Czech Republic", new List<string> { "Prague", "Brno", "Ostrava", "Plzeň", "Liberec" } },
    { "Denmark", new List<string> { "Copenhagen", "Aarhus", "Odense", "Aalborg", "Esbjerg" } },
    { "Egypt", new List<string> { "Cairo", "Alexandria", "Giza", "Shubra El Kheima", "Port Said" } },
    { "Finland", new List<string> { "Helsinki", "Espoo", "Tampere", "Vantaa", "Oulu" } },
    { "France", new List<string> { "Paris", "Marseille", "Lyon", "Toulouse", "Nice" } },
    { "Germany", new List<string> { "Berlin", "Hamburg", "Munich", "Cologne", "Frankfurt" } },
    { "Greece", new List<string> { "Athens", "Thessaloniki", "Patras", "Heraklion", "Larissa" } },
    { "Hungary", new List<string> { "Budapest", "Debrecen", "Szeged", "Miskolc", "Pécs" } },
    { "India", new List<string> { "Mumbai", "Delhi", "Bangalore", "Hyderabad", "Chennai" } },
    { "Indonesia", new List<string> { "Jakarta", "Surabaya", "Bandung", "Medan", "Semarang" } },
    { "Ireland", new List<string> { "Dublin", "Cork", "Limerick", "Galway", "Waterford" } },
    { "Italy", new List<string> { "Rome", "Milan", "Naples", "Turin", "Palermo" } },
    { "Japan", new List<string> { "Tokyo", "Osaka", "Yokohama", "Nagoya", "Sapporo" } },
    { "Malaysia", new List<string> { "Kuala Lumpur", "George Town", "Johor Bahru", "Kota Kinabalu", "Ipoh" } },
    { "Mexico", new List<string> { "Mexico City", "Guadalajara", "Monterrey", "Puebla", "Tijuana" } },
    { "Netherlands", new List<string> { "Amsterdam", "Rotterdam", "The Hague", "Utrecht", "Eindhoven" } },
    { "New Zealand", new List<string> { "Auckland", "Wellington", "Christchurch", "Hamilton", "Dunedin" } },
    { "Norway", new List<string> { "Oslo", "Bergen", "Trondheim", "Stavanger", "Drammen" } },
    { "Pakistan", new List<string> { "Karachi", "Lahore", "Islamabad", "Rawalpindi", "Peshawar" } },
    { "Peru", new List<string> { "Lima", "Arequipa", "Trujillo", "Chiclayo", "Piura" } },
    { "Philippines", new List<string> { "Manila", "Quezon City", "Cebu City", "Davao City", "Zamboanga City" } },
    { "Poland", new List<string> { "Warsaw", "Kraków", "Łódź", "Wrocław", "Poznań" } },
    { "Portugal", new List<string> { "Lisbon", "Porto", "Braga", "Coimbra", "Funchal" } },
    { "Romania", new List<string> { "Bucharest", "Cluj-Napoca", "Timișoara", "Iași", "Constanța" } },
    { "Russia", new List<string> { "Moscow", "Saint Petersburg", "Novosibirsk", "Yekaterinburg", "Kazan" } },
    { "Saudi Arabia", new List<string> { "Riyadh", "Jeddah", "Mecca", "Medina", "Dammam" } },
    { "South Africa", new List<string> { "Johannesburg", "Cape Town", "Durban", "Pretoria", "Port Elizabeth" } },
    { "South Korea", new List<string> { "Seoul", "Busan", "Incheon", "Daegu", "Daejeon" } },
    { "Spain", new List<string> { "Madrid", "Barcelona", "Valencia", "Seville", "Bilbao" } },
    { "Sweden", new List<string> { "Stockholm", "Gothenburg", "Malmö", "Uppsala", "Västerås" } },
    { "Switzerland", new List<string> { "Zurich", "Geneva", "Basel", "Lausanne", "Bern" } },
    { "Thailand", new List<string> { "Bangkok", "Chiang Mai", "Pattaya", "Phuket", "Hat Yai" } },
    { "Turkey", new List<string> { "Istanbul", "Ankara", "Izmir", "Bursa", "Antalya" } },
    { "Ukraine", new List<string> { "Kyiv", "Kharkiv", "Odessa", "Dnipro", "Lviv" } },
    { "United Arab Emirates", new List<string> { "Dubai", "Abu Dhabi", "Sharjah", "Al Ain", "Ajman" } },
    { "United Kingdom", new List<string> { "London", "Manchester", "Birmingham", "Liverpool", "Edinburgh" } },
    { "United States", new List<string> { "New York", "Los Angeles", "Chicago", "Houston", "Miami" } },
    { "Vietnam", new List<string> { "Hanoi", "Ho Chi Minh City", "Da Nang", "Haiphong", "Nha Trang" } }
    };

    List<string> _choosenSports { get; set; } = new List<string>();

    List<string> Dates { get; set; } = new List<string>();
    List<string> FromTimes { get; set; } = new List<string>();
    List<string> ToTimes { get; set; } = new List<string>();

    //public FindToPlayPage()
    //{
    //    InitializeComponent();
    //    _userService = new UserService();
    //    _lookingToPlayService = new LookingToPlayService();
    //}

    public FindToPlayPage()
    {
        InitializeComponent();
        _lookingToPlayService = new LookingToPlayService();
        _userService = new UserService();


        CountryPicker.ItemsSource = _countryCities.Keys.ToList();

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

        CountryPicker.SelectedItem = "Croatia";
        CityPicker.SelectedItem = "Zagreb";
    }
    private void OnCountrySelected(object sender, EventArgs e)
    {
        if (CountryPicker.SelectedItem != null)
        {
            string selectedCountry = CountryPicker.SelectedItem.ToString();
            if (_countryCities.ContainsKey(selectedCountry))
            {
                CityPicker.ItemsSource = _countryCities[selectedCountry];
                CityPicker.SelectedIndex = -1;  // Reset selection
            }
        }
    }

    private void RemoveSport(string sport)
    {
        _choosenSports.Remove(sport);
    }

    private void RemoveDate(string date)
    {
        Dates.Remove(date);
    }
    private void RemoveFromTime(string fromTime)
    {
        FromTimes.Remove(fromTime);
    }

    private void RemoveToTime(string toTime)
    {
        ToTimes.Remove(toTime);
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

    private void OnAddDateClicked(object sender, EventArgs e)
    {
        var dateEntry = DatePicker.Date.ToString("d");
        Dates.Add(dateEntry);
        DateStackLayout.Children.Add(new ItemView(dateEntry, () => RemoveDate(dateEntry)));
    }

    private void OnAddFromTimeClicked(object sender, EventArgs e)
    {
        var fromTimeEntry = FromTimePicker.Time.ToString(@"hh\:mm");
        FromTimes.Add(fromTimeEntry);
        FromTimeStackLayout.Children.Add(new ItemView(fromTimeEntry, () => RemoveFromTime(fromTimeEntry)));
    }
    private void OnAddToTimeClicked(object sender, EventArgs e)
    {
        var toTimeEntry = ToTimePicker.Time.ToString(@"hh\:mm");
        ToTimes.Add(toTimeEntry);
        ToTimeStackLayout.Children.Add(new ItemView(toTimeEntry, () => RemoveToTime(toTimeEntry)));
    }



    public async void OnSearchButtonClicked(object sender, EventArgs e)
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
                    Response response1 = await _userService.GetUserData("getUserData", userId);
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
                }

                FindToPlayView view = new FindToPlayView("FindToPlayPage", id, country, city, name, surname, email, date, fromTime, toTime, sportsString);
                FindToPlayScrollView.Children.Add(view);
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