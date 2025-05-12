using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eversports.Shells;

namespace Eversports
{
    public class Localization
    {
        public static async Task SetLanguage(string cultureCode)
        {

            CultureInfo newCulture;
            if (cultureCode == "default")
            {
                newCulture = CultureInfo.InvariantCulture;
            }
            else
            {
                newCulture = new CultureInfo(cultureCode);
            }
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;

            if (Shell.Current is AppShellLogin) 
            {
                ((App)Application.Current!)?.SetToAppShellLogin();
            }
            else
            {
                ((App)Application.Current!)?.SetToAppShellMain();
            }
        }
    }
}
