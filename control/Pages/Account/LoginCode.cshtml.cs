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

namespace control.Pages.Account
{
           

    public class LoginCodeModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public LoginCodeModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

  
        [BindProperty]
        [DataType(DataType.Text)]
        public string LoginCode { get; set; }


 
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {

            //_context.User.Add(User);
            // await _context.SaveChangesAsync();

            ViewData["email"] = HttpContext.Session.GetString("email");
            string? code_to_verify = HttpContext.Session.GetString("key");
            //return RedirectToPage("./Index");


            return Page();
        }
    }
}
