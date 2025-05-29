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

        // Cache user data to avoid redundant calls
        private Dictionary<int, UserInfo> _userCache = new();

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
            await InitializeUserCache();
            UpdateMessages();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _messageUpdateTimer?.Stop();
            _messageUpdateTimer?.Dispose();
        }

        private async Task InitializeUserCache()
        {
            try
            {
                var currentUserResponse = await _userService.GetUserData();
                var currentUser = currentUserResponse.obj as UserInfo;
                if (currentUser != null && !_userCache.ContainsKey(currentUser.id))
                {
                    _userCache[currentUser.id] = currentUser;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"User cache initialization failed: {ex.Message}", "OK");
            }
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

                    if (!_userCache.ContainsKey(ownerID))
                    {
                        var ownerResponse = await _userService.GetUserData(ownerID);
                        var ownerInfo = ownerResponse.obj as UserInfo;
                        if (ownerInfo != null)
                        {
                            _userCache[ownerID] = ownerInfo;
                        }
                    }

                    var owner = _userCache[ownerID];
                    string ownerName = owner.name + " " + owner.surname;

                    var currentUser = _userCache.ContainsKey(owner.id) ? _userCache[owner.id] : null;

                    // Message bubble color: green if current user, blue if other
                    Color bubbleColor = (currentUser != null && currentUser.id == ownerID) ? Color.FromRgb(0, 255, 0) : Color.FromRgb(0, 0, 255);

                    // If messages are encrypted, decrypt here
                    // string decryptedMessage = Decrypt(message); // implement if needed

                    MessagesScrollView.Children.Add(new MessageView(ownerName, message, bubbleColor));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "ChatPage: " + ex.Message, "OK");
            }
        }
    }
}
