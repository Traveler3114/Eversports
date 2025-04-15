namespace Eversports.Views;

public partial class MessageView : ContentView
{
    public MessageView(string message, Color color)
    {
        InitializeComponent();
        Message.Text = message;
        MessageBorder.Background = color;
    }
}
