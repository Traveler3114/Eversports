using Eversports.Model;

namespace Eversports;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage()
	{
		InitializeComponent();
	}
    private void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        UserInfo user = new UserInfo()
        {
            Name = NameEntry.Text,
            Surname = SurnameEntry.Text,
            Password = PasswordEntry.Text,
            Email = EmailEntry.Text,
            Address = AddressEntry.Text,
            Phone = PhoneNumberEntry.Text
        };
    }
}