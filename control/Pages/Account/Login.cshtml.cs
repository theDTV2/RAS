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
           

    public class LoginModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public LoginModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (AccountHelper.GetLoggedIn(HttpContext))
                Redirect("Account/Dashboard");

            return Page();
        }

        [BindProperty]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (AccountHelper.GetLoggedIn(HttpContext))
                Redirect("Account/Dashboard");

            await AuthHelper.CreateLoginRequest(_context,HttpContext, UserEmail);
            return Redirect("LoginCode");
        }
    }
}
