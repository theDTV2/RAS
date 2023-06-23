using control.Generator;
using control.Helper;
using control.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace control.Pages.Account
{
    public class LogoutModel : PageModel
    {

        public void OnGet()
        {
            AccountHelper.LogoutUser(HttpContext);
            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, LanguageManager.GetLocalizedString("ACCOUNT_LOGOUT_SUCCESS", AccountHelper.GetUserLanguage(HttpContext)));
            HttpContext.Response.Redirect("/Index");
        }
    }
}
