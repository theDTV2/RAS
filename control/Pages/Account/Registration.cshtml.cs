using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using control.Data;
using control.Models;
using control.Helper;

namespace control.Pages.Account
{
    public class RegistrationModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public RegistrationModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
			EAccessReturnValue _val = AuthHelper.CheckUserPermission(_context, HttpContext);

			if (_val != EAccessReturnValue.kAccountRegistrationNotCompleted)
				return Unauthorized();

			return Page();
		}


        [BindProperty]
        public required string firstName { get; set; }

		[BindProperty]
		public required string lastName { get; set; }

		[BindProperty]
		public bool AreYouSure { get; set; }
		public IActionResult OnPost()
        {
			EAccessReturnValue _val = AuthHelper.CheckUserPermission(_context, HttpContext);

            //TODO: Handle First/Last name empty

            if (AreYouSure && _val == EAccessReturnValue.kAccountRegistrationNotCompleted)
            {
				User _user = AccountHelper.GetLoggedInUser(_context, HttpContext);

				_user.CompletedRegistration = true;

                if (_user.AccessLevel == EAccessLevel.kNone)
                    _user.AccessLevel = EAccessLevel.kUser;

                _user.FirstName = firstName;
                _user.LastName = lastName;
				_context.SaveChanges();
				return RedirectToPage("Dashboard");
			}

            return Page(); ;
		}

        private bool UserExists(string id)
        {
          return (_context.User?.Any(e => e.UserName == id)).GetValueOrDefault();
        }
    }
}
