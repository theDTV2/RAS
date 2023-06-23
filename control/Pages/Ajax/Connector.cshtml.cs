using control.Helper;
using control.Manager;
using control.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace control.Ajax
{
    public class ConnectorModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ConnectorModel(control.Data.controlContext context)
        {
            _context = context;
        }


        [BindProperty(SupportsGet = true)]
        public string? Language { get; set; }


        public void OnGetChangeLanguage(string language)
        {
            if (!LanguageManager.IsValidLanguage(language))
                return;

            AccountHelper.SetUserLanguage(HttpContext, language);

            //If we are logged in, we adjust the language of the user
            if (AccountHelper.GetLoggedIn(HttpContext))
            {
                User _user = AccountHelper.GetLoggedInUser(_context, HttpContext);
                _user.Language = language;
                _context.SaveChanges();
            }

            return;
        }
        public IActionResult OnGet()
        {
            return NotFound();
        }
    }
}
