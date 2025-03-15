using Microsoft.Maui.Controls;

namespace Eversports.Shells;

public partial class AppShellMain : Shell
{
    public AppShellMain()
    {
        InitializeComponent();
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        SecureStorage.Default.Remove("StayLoggedIn");
        // Call the OnLogout method in the App class
        ((App)Application.Current!)?.SetToAppShellLogin();
    }
}