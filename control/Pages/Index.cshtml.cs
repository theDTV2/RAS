using control.Manager;
using control.Models;
using control.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NuGet.Packaging.Signing;
using System.Reflection;
using System.Resources;
using System.Text.Json.Nodes;
using System;
using System.Globalization;
using control.Helper;

namespace control.Pages
{
    public class IndexModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public IndexModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (!AccountHelper.GetLoggedIn(HttpContext))
                return RedirectToPage("Account/Login");



            return RedirectToPage("Account/Dashboard");
        }
    }
}