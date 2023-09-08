
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using control.Helper;
using control.Generator;
using System.Web;
using control.Manager;

namespace control.Pages.Account
{

    public class LoginCodeModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public LoginCodeModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty]
        [DataType(DataType.Text)]
        public string LoginCode { get; set; }


        [BindProperty(SupportsGet = true)]
        public string? SecretLoginCode { get; set; }


        public IActionResult OnPost()
        {
            if (AccountHelper.GetLoggedIn(HttpContext))
                Redirect("Account/Dashboard");

            EAccessReturnValue res = AuthHelper.ChallengeLoginRequestWithLoginCode(_context, HttpContext, AccountHelper.GetUserName(HttpContext), LoginCode);

            if (res == EAccessReturnValue.kAccessGranted)
            {
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, LanguageManager.GetLocalizedString("ACCOUNT_LOGINCODE_SUCCESS", AccountHelper.GetUserLanguage(HttpContext)));

                res = AuthHelper.CheckUserPermission(_context, HttpContext);

                //TODO: Alerts for these redirects
                if (res == EAccessReturnValue.kAccountEulaNotAccepted)
                    return RedirectToPage("Privacy");

                if (res == EAccessReturnValue.kAccountRegistrationNotCompleted)
                {
                    return RedirectToPage("Registration");
                }

                if (res == EAccessReturnValue.kAccountLocked)
                {
                    
                    AccountHelper.LogoutUser(HttpContext);
                    AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, LanguageManager.GetLocalizedString("ACCOUNT_LOGINCODE_LOCKED", AccountHelper.GetUserLanguage(HttpContext)));
                    return RedirectToPage("Login");

                }

                return RedirectToPage("Dashboard");
            }

             if (AuthHelper.CheckIfMaxAuthTriesReached(_context, AccountHelper.GetUserName(HttpContext)))
            {
                AccountHelper.LogoutUser(HttpContext);
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, LanguageManager.GetLocalizedString("ACCOUNT_LOGINCODE_MAX_RETRIES", AccountHelper.GetUserLanguage(HttpContext)));
                return RedirectToPage("Login");
            }

            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, LanguageManager.GetLocalizedString("ACCOUNT_LOGINCODE_FAILED", AccountHelper.GetUserLanguage(HttpContext)));
            return Page();
        }


        public IActionResult OnGet()
        {
            if (AccountHelper.GetLoggedIn(HttpContext))
                Redirect("Account/Dashboard");

            if (SecretLoginCode is not null)
            {

                EAccessReturnValue res = AuthHelper.ChallengeLoginRequestWithLoginKey(_context, HttpContext, SecretLoginCode);
                if (res == EAccessReturnValue.kAccessGranted)
                {
                    AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, LanguageManager.GetLocalizedString("ACCOUNT_LOGINCODE_SUCCESS", AccountHelper.GetUserLanguage(HttpContext)));
                    res = AuthHelper.CheckUserPermission(_context, HttpContext);


                    //TODO: Alerts for these redirects
                    if (res == EAccessReturnValue.kAccountEulaNotAccepted)
                        return RedirectToPage("Privacy");

                    if (res == EAccessReturnValue.kAccountRegistrationNotCompleted)
                        return RedirectToPage("Registration");

                    if (res == EAccessReturnValue.kAccountLocked)
                    {
                        AccountHelper.LogoutUser(HttpContext);
                        AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, LanguageManager.GetLocalizedString("ACCOUNT_LOGINCODE_LOCKED", AccountHelper.GetUserLanguage(HttpContext)));
                        return RedirectToPage("Login");

                    }


                    return RedirectToPage("Dashboard");
                }
                //If we do not find the login key, redirect to 404
                return NotFound();
            }

            return Page();
        }
    }
}