using System.Globalization;
using Microsoft.Maui.Controls;


namespace Eversports.Shells;

public partial class AppShellLogin : Shell
{
    public AppShellLogin()
    {
        InitializeComponent();

    }
    private void SetLanguageToHR(object sender, EventArgs e)
    {
        Localization.SetLanguage("hr");
        //var currentRoute = Shell.Current?.CurrentState?.Location.OriginalString;
        //DisplayAlert("OK", currentRoute, "OK");
    }
    private void SetLanguageToEN(object sender, EventArgs e)
    {
        Localization.SetLanguage("default");
    }
}