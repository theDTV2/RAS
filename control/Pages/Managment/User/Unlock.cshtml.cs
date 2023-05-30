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
        public string Name { get; set; }

        public void OnGet()
        {

            return;
        }
        public void OnGetSearch()
        {
            if (UserIdentifier is not null)
            {

            }


            return ;
        }



        public IActionResult OnPostAsync()
        {


            return Page();
        }

        private bool UserExists(string id)
        {
          return (_context.User?.Any(e => e.UserName == id)).GetValueOrDefault();
        }
    }
}
