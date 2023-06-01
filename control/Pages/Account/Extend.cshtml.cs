using control.Helper;
using control.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace control.Pages.Account
{
    public class ExtendModel : PageModel
    {
        private readonly control.Data.controlContext _context;
        public ExtendModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kUser))
                return Unauthorized();


            return Page();
        }

        public IActionResult OnPost()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kUser))
                return Unauthorized();


            return RedirectToPage("Dashboard");
        }
    }




}
