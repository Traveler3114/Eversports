using System.Collections.ObjectModel;

namespace Eversports.Views;

public partial class ItemView : ContentView
{

    private Action _removeItemAction;
    public ItemView(string text, Action removeItemAction)
    {
        InitializeComponent();
        ItemLabel.Text = text;
        _removeItemAction = removeItemAction;
    }

    private void OnRemoveButtonClicked(object sender, EventArgs e)
    {
        _removeItemAction.Invoke();
        var parent = this.Parent as StackLayout;
        if (parent != null)
        {
            parent.Children.Remove(this);
        }
    }
}
