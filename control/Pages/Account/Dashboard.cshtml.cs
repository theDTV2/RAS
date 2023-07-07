using control.Generator;
using control.Helper;
using control.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace control.Pages.Account
{
    public class DashboardModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public DashboardModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string ExpiryDate { get; set; }
        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kUser, IgnoreExpiryDate: true))
                return Unauthorized();

            ExpiryDate = AccountHelper.GetExpiryDate(_context, HttpContext);

            return Page();
        }
    }
}
