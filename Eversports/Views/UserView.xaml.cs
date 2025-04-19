using Eversports.Services;

namespace Eversports.Views;

public partial class UserView : ContentView
{
	public UserView(int user_id,string name,string surname,string email)
	{
		InitializeComponent();
		User_id.Text = user_id.ToString();
		Name.Text = name;
		Surname.Text = surname;
		Email.Text = email;
	}

    private async void OnTapped(object sender, EventArgs e)
    {

    }
}