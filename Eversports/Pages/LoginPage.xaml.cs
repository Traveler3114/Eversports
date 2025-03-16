using Microsoft.Maui.ApplicationModel.Communication;

namespace Eversports;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
	private async void OnLoginButtonClicked(object sender, EventArgs e)
	{
       // await SecureStorage.Default.SetAsync("UserEmail", email);

        if (RememberMeCheckBox.IsChecked)
        {
            await SecureStorage.Default.SetAsync("StayLoggedIn", "true");
        }
        ((App)Application.Current!)?.SetToAppShellMain();
    }
}