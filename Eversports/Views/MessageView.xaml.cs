namespace Eversports.Views;

public partial class MessageView : ContentView
{
    public MessageView(string sednerName,string message, Color color)
    {
        InitializeComponent();
        Message.Text = message;
        MessageBorder.Background = color;
        SenderName.Text = sednerName;
    }
}
