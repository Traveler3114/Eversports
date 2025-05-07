using System.Text.Json;
using System.Text;
using System;
using Eversports.Shells;
using Eversports.Pages;
using Eversports.Services;
using System.Net.Security;
using System.Net;

namespace Eversports
{
    public partial class App : Application
    {
          
        public App()
        {
            InitializeComponent();
        }


        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShellLogin());
        }



        public void SetToAppShellMain()
        {
            Windows[0].Page = new AppShellMain();
        }

        public void SetToAppShellLogin()
        {
            Windows[0].Page = new AppShellLogin();
        }
        public void SetToChatPage(int lookingtoplay_id)
        {
            Windows[0].Page = new ChatPage(lookingtoplay_id);
        }
    }
}
