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
using Microsoft.AspNetCore.Http.HttpResults;

namespace control.Pages.Managment.Door
{
    public class ListModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ListModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IList<control.Models.Door> Door { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin))
                return Unauthorized();

            if (_context.Door != null)
            {
                Door = await _context.Door.ToListAsync();
            }
            return Page();
        }
    }
}
