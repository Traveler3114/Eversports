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
using System.Net.Http.Headers;
using CountriesAPI;
using Eversports.Resources;

namespace Eversports.Pages;

public partial class LookingToPlayPage : ContentPage
{
    //stranica gdje korisnici mogu postavljati trazenja igraca
    //lista na kojoj pise koji korisnik trazi igraca, koji sport i koja lokacija,
    //takoder taj korisnik moze postaviti odredenjeno vrijeme, na primjer subota ili nedelja izmedju 10-12

    private readonly LookingToPlayService _lookingToPlayService;

    List<string> _choosenSports { get; set; } = new List<string>();
    List<AvailableDateTime> _availableDateTimes { get; set; }=new List<AvailableDateTime>();

    Dictionary<string,List<string>> countryAndCities { get; set; }=new Dictionary<string,List<string>>();
    public LookingToPlayPage()
    {
        InitializeComponent();
        _lookingToPlayService = new LookingToPlayService();

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
            await DisplayAlert("Error", ex.Message, "OK");
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
            string dateTimeEntry = $"{dateTime.Date} {Strings.From} {dateTime.FromTime} {Strings.To} {dateTime.ToTime}";
            TimeStackLayout.Children.Add(new ItemView(dateTimeEntry, ()=>RemoveDateTime(dateTime)));
        }
        else
        {
            DisplayAlert("Error", "You already selected that date and time", "Ok");
        }
    }

    public async void OnCreateButtonClicked(object sender, EventArgs e)
    {
        if ((_availableDateTimes == null || !_availableDateTimes.Any()) ||
            CountryPicker.SelectedItem == null || string.IsNullOrWhiteSpace(CountryPicker.SelectedItem.ToString()) ||
            CityPicker.SelectedItem == null || string.IsNullOrWhiteSpace(CityPicker.SelectedItem.ToString()) ||
            (_choosenSports == null || !_choosenSports.Any()))
        {
            await DisplayAlert("Validation Error", "Please fill in all required fields before submitting.", "OK");
            return;
        }

        LookingToPlay lookingToPlay = new LookingToPlay()
        {
            availableDateTimes = _availableDateTimes,
            country = CountryPicker.SelectedItem.ToString(),
            city = CityPicker.SelectedItem.ToString(),
            detailedLocation = DetailedLocationEntry.Text, // Can be empty
            choosenSports = _choosenSports,
            description = DescriptionEntry.Text,             // Can be empty
            jwt = await SecureStorage.Default.GetAsync("JWTToken")
        };

        try
        {
            var response = await _lookingToPlayService.AddLookingToPlay(lookingToPlay);
            await DisplayAlert("OK", response["message"], "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "LookingToPlayPage: " + ex.Message, "OK");
        }
    }

}
