using Eversports.Shells;

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
            
            // Replace this with your actual login check
            var isUserLoggedIn = SecureStorage.Default.GetAsync("StayLoggedIn").Result;

            if (isUserLoggedIn == "true")
            {
                return new Window(new AppShellMain()); // Main app shell
            }
            else
            {
                return new Window(new AppShellLogin()); // Login shell
            }
        }

        public void SetToAppShellMain()
        {
            // Set the login state to Secure Storage
            Windows[0].Page = new AppShellMain();
        }

        public void SetToAppShellLogin()
        {
            // Remove the login state from Secure Storage
            Windows[0].Page = new AppShellLogin();
        }
    }
}
