using Eversports.Models;
using Eversports.Services;
using Eversports.Views;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Text.Json;
using System.Xml.Linq;
using CountriesAPI;

namespace Eversports.Pages;

public partial class FindToPlayPage : ContentPage
{
    private readonly LookingToPlayService _lookingToPlayService;
    private readonly UserService _userService;
    Dictionary<string, List<string>> countryAndCities { get; set; } = new Dictionary<string, List<string>>();

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

    }


    protected async override void OnAppearing()
    {
        base.OnAppearing();

        string cacheFilePath = Path.Combine(FileSystem.AppDataDirectory, "country_cities.json");

        if (File.Exists(cacheFilePath))
        {
            // Load cached data
            var json = await File.ReadAllTextAsync(cacheFilePath);
            var cachedData = JsonSerializer.Deserialize<List<CountryCities>>(json);

            foreach (var item in cachedData)
            {
                CountryPicker.Items.Add(item.Country);
                countryAndCities[item.Country] = item.Cities;
            }
        }
        else
        {
            // First time: download and cache
            var soapClient = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

            List<CountryCities> dataToCache = new List<CountryCities>();
            var response = await soapClient.ListOfCountryNamesByNameAsync();

            foreach (var country in response.Body.ListOfCountryNamesByNameResult)
            {
                string name = country.sName;
                string iso = country.sISOCode;
                var cities = await GetCityData(iso);

                CountryPicker.Items.Add(name);
                countryAndCities[name] = cities;

                dataToCache.Add(new CountryCities
                {
                    Country = name,
                    ISOCode = iso,
                    Cities = cities
                });
            }

            // Save to local file
            var json = JsonSerializer.Serialize(dataToCache);
            await File.WriteAllTextAsync(cacheFilePath, json);
        }
    }

    public async Task<List<string>> GetCityData(string countryCode)
    {
        try
        {
            var handler = new HttpClientHandler()
            {
                // Disable SSL certificate validation
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };

            var client = new HttpClient(handler);
            var url = $"http://api.geonames.org/searchJSON?country={countryCode}&featureClass=P&maxRows=10&username=traveler3114";

            var response = await client.GetStringAsync(url);
            JsonDocument doc = JsonDocument.Parse(response);

            List<string> cityNames = new List<string>();
            foreach (var city in doc.RootElement.GetProperty("geonames").EnumerateArray())
            {
                string name = city.GetProperty("name").GetString();
                cityNames.Add(name);
            }
            return cityNames;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "GetCityData: " + ex.Message, "OK");
            return null;
        }
    }

    private void OnCountrySelected(object sender, EventArgs e)
    {
        if (CountryPicker.SelectedItem != null)
        {
            string selectedCountry = CountryPicker.SelectedItem.ToString();
            if (countryAndCities.ContainsKey(selectedCountry))
            {
                CityPicker.ItemsSource = countryAndCities[selectedCountry];
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
            var response = await _lookingToPlayService.GetLookingToPlay(CountryPicker.SelectedItem.ToString(),CityPicker.SelectedItem.ToString(),Dates,FromTimes,ToTimes,_choosenSports);
            if (response.status == "success")
            {
                XDocument doc = response.obj as XDocument;

                foreach (XElement item in doc.Descendants("item"))
                {
                    id = Convert.ToInt32(item.Element("id")!.Value);
                    country = item.Element("country")!.Value;
                    city = item.Element("city")!.Value;
                    userId = Convert.ToInt32(item.Element("user_id")!.Value);
                    Response response1 = await _userService.GetUserData(1);
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
                    FindToPlayScrollView.Children.Add(new FindToPlayView("FindToPlayPage", id, country, city, name, surname, email, date, fromTime, toTime, sportsString));
                }
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