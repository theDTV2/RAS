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
            HttpContext.Response.Redirect("/Index");
        }
    }
}
