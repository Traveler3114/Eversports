using Eversports.Services;

namespace Eversports.Views;

public partial class UserView : ContentView
{
	private readonly UserService _userService;
	public UserView(int user_id,string name,string surname,string email)
	{
		InitializeComponent();
		_userService = new UserService();
		User_id.Text = user_id.ToString();
		Name.Text = name;
		Surname.Text = surname;
		Email.Text = email;
	}

    private async void OnTapped(object sender, EventArgs e)
    {
		await _userService.DeleteUser(Convert.ToInt32(User_id.Text));
    }
}