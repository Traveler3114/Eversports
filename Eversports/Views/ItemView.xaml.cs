using System.Collections.ObjectModel;

namespace Eversports.Views;

public partial class ItemView : ContentView
{


    public ItemView(string text)
    {
        InitializeComponent();
        ItemLabel.Text = text;
    }

    private void OnRemoveButtonClicked(object sender, EventArgs e)
    {
        var parent = this.Parent as HorizontalStackLayout;
        if (parent != null)
        {
            parent.Children.Remove(this);
        }
    }
}
