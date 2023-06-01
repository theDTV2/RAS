using control.Generator;
using control.Helper;
using control.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace control.Pages.Account
{
    public class ExtendModel : PageModel
    {
        private readonly control.Data.controlContext _context;
        public ExtendModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kUser))
                return Unauthorized();

            if (!DateHelper.LoggedInUserIsAllowedToExtend(_context, HttpContext))
            {
                //TODO: Guard against page entry instead of here
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kWarning, "Extension is not possible until shortly before semester end.");

                return RedirectToPage("Dashboard");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kUser))
                return Unauthorized();

            User _user = AccountHelper.GetLoggedInUser(_context, HttpContext);

            if (_user == null)
                return Unauthorized();

            if (DateHelper.LoggedInUserIsAllowedToExtend(_context, HttpContext))
            {
                _user.ExpiryDate = DateHelper.GenerateNextSemesterEnd();
            }

            _context.SaveChanges();
            //TODO: Catch errors here necessary?

            return RedirectToPage("Dashboard");
        }
    }




}
