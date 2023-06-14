
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


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPost()
        {
            if (AccountHelper.GetLoggedIn(HttpContext))
                Redirect("Account/Dashboard");

            EAccessReturnValue res = AuthHelper.ChallengeLoginRequestWithLoginCode(_context, HttpContext, AccountHelper.GetUserName(HttpContext), LoginCode);

            if (res == EAccessReturnValue.kAccessGranted)
            {
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, "Logged in successfully");

                res = AuthHelper.CheckUserPermission(_context, HttpContext);

                if (res == EAccessReturnValue.kAccountEulaNotAccepted)
                    return RedirectToPage("Privacy");

                if (res == EAccessReturnValue.kAccountRegistrationNotCompleted)
                {
                    return RedirectToPage("Registration");
                }

                if (res == EAccessReturnValue.kAccountLocked)
                {
                    
                    AccountHelper.LogoutUser(HttpContext);
                    AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, "Account locked");
                    return RedirectToPage("Login");

                }

                return RedirectToPage("Dashboard");
            }
            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, "Login failed");
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
                    AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, "Logged in successfully");
                    res = AuthHelper.CheckUserPermission(_context, HttpContext);


                    if (res == EAccessReturnValue.kAccountEulaNotAccepted)
                        return RedirectToPage("Privacy");

                    if (res == EAccessReturnValue.kAccountRegistrationNotCompleted)
                        return RedirectToPage("Registration");


                    return RedirectToPage("Dashboard");
                }
                //If we do not find the login key, redirect to 404
                return NotFound();
            }

            return Page();
        }
    }
}