
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
            // HttpContext.Session.SetString("key", AccessHelper.GenerateAccessCode());

            //   System.Console.WriteLine(HttpContext.Session.GetString("key"));

            /* var c = AccountHelper.GetLoggedIn(HttpContext);
             var b = AccountHelper.GetUserName(HttpContext);
             var a = AccountHelper.GetEAccessLevel(HttpContext);
               */

            //return RedirectToPage("./Index");

            AuthHelper.ChallengeLoginRequestWithAccessCode(_context,HttpContext, AccountHelper.GetUserName(HttpContext), LoginCode);

            return Page();
        }


        public IActionResult OnGet()
        {
            if (SecretLoginCode is not null)
            {


                //TODO: Act after secret Login Link
            }



            AuthHelper.ChallengeLoginRequestWithAccessCode(_context, HttpContext, AccountHelper.GetUserName(HttpContext), LoginCode);

            return Page();
        }



    }
}