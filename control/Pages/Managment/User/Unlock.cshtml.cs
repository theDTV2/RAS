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
using control.Generator;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace control.Pages.Managment.User
{
    public class UnlockModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public UnlockModel(control.Data.controlContext context)
        {
            _context = context;
        }



        [BindProperty(SupportsGet = true)]
        public string? UserIdentifier { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public bool FoundElement { get; set; } = false;



        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string Secret { get; set; }

        public new Models.User User { get; set; }

        public IActionResult OnGet()
        {
            if (_context.User.Where(u => u.UserName == UserIdentifier).Count() != 1)
            {
                return new RedirectResult("List");
            }

            User = _context.User.Where(u => u.UserName == UserIdentifier).First();


            if ((UserIdentifier is null) || (User is null))
            {
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, "User missing or invalid");

                return new RedirectResult("List");
            }

            LastName = User.LastName;
            FirstName = User.FirstName;
            Secret = User.SecretCode;

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            User = _context.User.Where(u => u.UserName == UserIdentifier).First();

            _context.Attach(User).State = EntityState.Modified;
            User.LastName = LastName;
            User.FirstName = FirstName;
            User.SecretCode = Secret;

            try { _context.SaveChanges(); }
            catch {
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, "Error while saving data");
            
            }
            //TODO: Proper Error catching

            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, "Saving successfull");

            return new RedirectResult("List");
        }

    }
}
