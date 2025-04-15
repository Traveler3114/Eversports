using Eversports.Models;
using Eversports.Services;

namespace Eversports.Pages;

public partial class ChatPage : ContentPage
{
    private readonly ChatService _chatService;

    private int lookingtoplay_id;
	public ChatPage(int _lookingtoplay_id)
	{
		InitializeComponent();
        lookingtoplay_id = _lookingtoplay_id;
        _chatService = new ChatService();
	}

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        ((App)Application.Current!)?.SetToAppShellMain();
    }

    private async void SendMsgButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var response = await _chatService.SendMessage("sendMessage", lookingtoplay_id, Message.Text);
            //await DisplayAlert("ok", response, "ok");
            Message.Text = "";
            if (response["status"] == "error")
            {
                await DisplayAlert("ok", response["message"], "ok");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "ChatPage:" + ex.Message, "OK");
        }
    }
}