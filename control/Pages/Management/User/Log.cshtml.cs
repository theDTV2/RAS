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
using control.Generator;
using control.Manager;

namespace control.Pages.Management.User
{
    public class LogModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public LogModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public required string UserIdentifier { get; set; }


        public IList<Log> LogEntries { get; set; } = default!;

        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kModerator))
                return Unauthorized();

            if (UserIdentifier is null)
                return NotFound();


            LogEntries = _context.Log.Where(u => u.User!.UserName == UserIdentifier).Include(d => d.Door).AsEnumerable().Reverse().ToList();


            if (!LogEntries.Any())
            {
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kWarning, LanguageManager.GetLocalizedString("MANAGEMENT_USER_LOG_NO_ENTRIES_FOUND", AccountHelper.GetUserLanguage(HttpContext)));
                return RedirectToPage("List");
            }

            return Page();

        }
    }
}
