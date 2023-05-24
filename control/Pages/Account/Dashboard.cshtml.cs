using control.Generator;
using control.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace control.Pages.Account
{
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, "Hello World1");
            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kInformation, "Hello World2");
            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kWarning, "Hello World3");
            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, "Hello World4");

        }
    }
}
