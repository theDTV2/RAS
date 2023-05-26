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

namespace control.Pages.Managment.Door
{
    public class ManageModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ManageModel(control.Data.controlContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin))
                return Unauthorized();

            return Page();
        }
    }
}
