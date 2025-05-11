using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using control.Data;
using control.Models;
using System.ComponentModel.DataAnnotations;
using control.Helper;
using control.Generator;
using control.Manager;
using Microsoft.Extensions.Primitives;

namespace control.Pages.Account
{
           

    public class LoginModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public LoginModel(control.Data.controlContext context)
        {
            _context = context;
        }



        [BindProperty]
        [DataType(DataType.EmailAddress)]
		public required string UserEmail { get; set; }


		public IActionResult OnGet()
		{
			if (AccountHelper.GetLoggedIn(HttpContext))
				Redirect("Account/Dashboard");

			return Page();
		}


		public async Task<IActionResult> OnPostAsync()
        {
            if (AccountHelper.GetLoggedIn(HttpContext))
                Redirect("Account/Dashboard");


            string _userMail = UserEmail.ToLower();
            string[] _userMailElements = _userMail.Split("@");

            string _userMailSuffix = GeneralSettingsManager.GetEmailSuffix();


            if (_userMailSuffix.Length > 0)
            {
                if (_userMailElements[1] != _userMailSuffix || _userMailElements[0].Contains('.'))
                {
                    AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, LanguageManager.GetLocalizedStringWithParameter("ACCOUNT_LOGIN_WRONG_EMAIL_PROVIDER", GeneralSettingsManager.GetEmailSuffix(), AccountHelper.GetUserLanguage(HttpContext)));
                    return Page();
                }
            }

            await AuthHelper.CreateLoginRequest(_context,HttpContext, _userMail);
            return Redirect("LoginCode");
        }
    }
}
