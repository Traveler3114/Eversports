using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;
using Eversports.Models;
using Eversports.Services;
using Eversports.Views;
using Eversports.Resources;


namespace Eversports.Pages
{
    public partial class ChatPage : ContentPage
    {
        private readonly ChatService _chatService;
        private readonly UserService _userService;
        private Dictionary<int, Dictionary<int, string>> messages = new Dictionary<int, Dictionary<int, string>>();
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
                await DisplayAlert("Error", Strings.MessageCantBeEmpty, "OK");
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
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void UpdateMessages()
        {
            _messageUpdateTimer = new System.Timers.Timer(500);
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
                var response = await _chatService.GetMessages(_lookingToPlayId);
                XDocument doc = response.obj as XDocument;

                if (doc == null)
                    return;

                var currentUserResponse = await _userService.GetUserData(false);
                var currentUser = currentUserResponse.obj as UserInfo;

                foreach (XElement item in doc.Descendants("item"))
                {
                    int messageID = Convert.ToInt32(item.Element("id")!.Value);
                    int ownerID = Convert.ToInt32(item.Element("sender_id")!.Value);
                    string message = item.Element("encrypted_message")!.Value;

                    if (!messages.ContainsKey(messageID))
                    {
                        messages[messageID] = new Dictionary<int, string>();
                        messages[messageID][ownerID] = message;

                        var messageOwnerResponse = await _userService.GetUserData(false, ownerID);
                        var owner = messageOwnerResponse.obj as UserInfo;
                        var ownerName = owner.name;

                        var messageColor = (ownerID == currentUser.id)
                            ? Color.FromRgb(0, 255, 0)
                            : Color.FromRgb(0, 0, 255);

                        MessagesScrollView.Children.Add(new MessageView(ownerName, message, messageColor));
                    }
                }

                //if (MessagesScrollView.Children.Count > 0)
                //{
                //    await Task.Delay(50);
                //    var lastMessage = MessagesScrollView.Children[MessagesScrollView.Children.Count - 1] as VisualElement;
                //    if (lastMessage != null)
                //    {
                //        await MessagesScroll.ScrollToAsync(lastMessage, ScrollToPosition.End, true);
                //    }
                //}
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "ChatPage: " + ex.Message, "OK");
            }
        }
    }
}
