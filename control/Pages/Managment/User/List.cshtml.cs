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

namespace control.Pages.Managment.User
{
    public class ListModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ListModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public new IList<Models.User> User { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public bool SearchedSomething { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kModerator))
                return Unauthorized();

            SearchedSomething = false;

            if (_context.User != null)
            {
                User = await _context.User.ToListAsync();
            }
            return Page();
        }

        public IActionResult OnGetSearch()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kModerator))
                return Unauthorized();

            SearchedSomething = true;
            User = SearchHelper.SearchUser(_context, SearchTerm).ToList();
            //TODO: Make this better

            return Page();
        }


    }
}
