using Microsoft.Maui.Controls;


namespace Eversports.Shells;

public partial class AppShellLogin : Shell
{
    public AppShellLogin()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
    }
}