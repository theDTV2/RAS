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
using control.Generator;

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
            EAccessReturnValue _val =  AuthHelper.CheckUserPermission(_context, HttpContext);

            if (_val != EAccessReturnValue.kAccountEulaNotAccepted) {
                return RedirectToPage("Dashboard");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            EAccessReturnValue _val = AuthHelper.CheckUserPermission(_context, HttpContext);

            if (AcceptEula && _val == EAccessReturnValue.kAccountEulaNotAccepted)
            {
                //TODO: Move this
                var _user = AccountHelper.GetLoggedInUser(_context, HttpContext);
                _user.AcceptedEula = true;
                _context.SaveChanges();

                //TODO: Add Redirect Alert
                return RedirectToPage("Dashboard");
            }
            return Page();
        }
    }
}
