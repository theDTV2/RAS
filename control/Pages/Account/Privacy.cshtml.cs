using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using control.Data;
using control.Models;
using control.Helper;

namespace control.Pages.Account
{
    public class PrivacyModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public PrivacyModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty]
        public bool AcceptEula { get; set; }

        public IActionResult OnGet()
        {
            AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kNone);

            var _user = AccountHelper.GetLoggedInUser(_context, HttpContext);

            if (_user.AcceptedEula) {
                //TODO: Improve redirect after alert implementation
                return RedirectToPage("Dashboard");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kNone);

            if (AcceptEula)
            {

                //TODO: Move this
                var _user = AccountHelper.GetLoggedInUser(_context, HttpContext);
                _user.AcceptedEula = true;
                _context.SaveChanges();

                //TODO: Improve redirect after alert implementation
                return RedirectToPage("Dashboard");
            }

            return Page();
        }
    }
}
