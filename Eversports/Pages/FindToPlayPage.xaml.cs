using Eversports.Models;
using Eversports.Services;
using Eversports.Views;
using System.Xml.Linq;

namespace Eversports.Pages;

public partial class FindToPlayPage : ContentPage
{
    private readonly LookingToPlayService _lookingToPlayService;
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

    List<AvailableDateTime> _availableDateTimes { get; set; } = new List<AvailableDateTime>();
    public FindToPlayPage()
	{
		InitializeComponent();
        _lookingToPlayService = new LookingToPlayService();



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
            TimeStackLayout.Children.Add(new ItemView(dateTimeEntry, () => RemoveDateTime(dateTime)));
        }
        else
        {
            DisplayAlert("Error", "You already selected that date and time", "Ok");
        }
    }

    public async void OnSearchButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Get the XML response from the PHP script
            XDocument doc = await _lookingToPlayService.GetAllLookingToPlay();

            // For debugging, show the XML string in an alert
            await DisplayAlert("XML Response", doc.ToString(), "OK");

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}