using control.Generator;
using control.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace control.Pages.Account
{
    public class LogoutModel : PageModel
    {

        public void OnGet()
        {
            AccountHelper.LogoutUser(HttpContext);
            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, "Logged out successfully!");
            HttpContext.Response.Redirect("/Index");
        }
    }
}
