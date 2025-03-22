using Eversports.Models;
using Eversports.Views;
using Eversports.Services;
using CsvHelper;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Collections.ObjectModel;

namespace Eversports.Pages;

public partial class LookingToPlayPage : ContentPage
{

    //stranica gdje korisnici mogu postavljati trazenja igraca
    //lista na kojoj pise koji korisnik trazi igraca, koji sport i koja lokacija,
    //takoder taj korisnik moze postaviti odredenjeno vrijeme, na primjer subota ili nedelja izmedju 10-12

    public LookingToPlayPage()
	{
		InitializeComponent();
        SportPicker.ItemsSource = new List<string>
        {
            "Football",
            "Basketball",
            "Cricket",
            "Tennis",
            "Table Tennis",
            "Baseball",
            "Golf",
            "Volleyball",
            "Badminton",
            "Boxing",
            "Swimming",
            "Athletics",
            "Cycling",
            "Handball",
            "Gymnastics",
            "Skiing and Snowboarding",
            "Surfing",
            "Skateboarding"
        };
    }

    private void OnSportSelected(object sender, EventArgs e)
    {
        var selectedSport = SportPicker.SelectedItem.ToString();
    }
}