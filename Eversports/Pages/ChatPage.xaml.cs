using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Eversports.Models;
using Eversports.Services;
using Eversports.Views;


namespace Eversports.Pages
{
    public partial class ChatPage : ContentPage
    {
        private readonly ChatService _chatService;
        private readonly UserService _userService;
        private int _lookingToPlayId;
        private System.Timers.Timer? _messageUpdateTimer;


        public ChatPage(int lookingToPlayId)
        {
            InitializeComponent();

            _lookingToPlayId = lookingToPlayId;
            _chatService = new ChatService();
            _userService = new UserService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            UpdateMessages();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _messageUpdateTimer?.Stop();
            _messageUpdateTimer?.Dispose();
        }


        private void BackButton_Clicked(object sender, EventArgs e)
        {
            ((App)Application.Current!)?.SetToAppShellMain();
        }

        private async void SendMsgButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Message?.Text))
            {
                await DisplayAlert("Error", "Message cannot be empty.", "OK");
                return;
            }

            try
            {
                var response = await _chatService.SendMessage(_lookingToPlayId, Message.Text);
                Message.Text = "";

                if (response["status"] == "error")
                {
                    await DisplayAlert("Error", response["message"], "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "ChatPage: " + ex.Message, "OK");
            }
        }

        private void UpdateMessages()
        {
            _messageUpdateTimer = new System.Timers.Timer(3000);
            _messageUpdateTimer.Elapsed += async (s, e) =>
            {
                await MainThread.InvokeOnMainThreadAsync(ShowAllMessages);
            };
            _messageUpdateTimer.AutoReset = true;
            _messageUpdateTimer.Start();
        }

        private async Task ShowAllMessages()
        {
            try
            {
                MessagesScrollView.Children.Clear();

                var response = await _chatService.GetMessages(_lookingToPlayId);
                XDocument doc = response.obj as XDocument;

                if (doc == null)
                    return;

                foreach (XElement item in doc.Descendants("item"))
                {
                    int ownerID = Convert.ToInt32(item.Element("sender_id")!.Value);
                    string message = item.Element("encrypted_message")!.Value;

                    var currentUserResponse = await _userService.GetUserData(false);
                    var currentUser = currentUserResponse.obj as UserInfo;

                    var MessageOwnerResponse= await _userService.GetUserData(false,ownerID);
                    var Owner= MessageOwnerResponse.obj as UserInfo;
                    var ownerName = Owner.name;

                    if (ownerID == currentUser.id)
                    {
                        MessagesScrollView.Children.Add(new MessageView(ownerName, message, Color.FromRgb(0, 255, 0)));
                    }
                    else
                    {
                        MessagesScrollView.Children.Add(new MessageView(ownerName, message, Color.FromRgb(0, 0, 255)));
                    }
                    
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "ChatPage: " + ex.Message, "OK");
            }
        }
    }
}
