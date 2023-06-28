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
using System.ComponentModel.DataAnnotations;

namespace control.Pages.Managment.Door
{
    public class LogModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public LogModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string DoorIdentifier { get; set; } = "";

        public List<Log> DoorEntries { get; set; }

        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kModerator))
                return Unauthorized();


            if (DoorIdentifier != null)
                DoorEntries = _context.Log.Where(d => d.Door.Id == DoorIdentifier).Include(u => u.User).Include(d => d.Door).ToList();
            else
                DoorEntries = _context.Log.Include(u => u.User).Include(d => d.Door).ToList();

            DoorEntries.Reverse();

            return Page();
        }

    }
}
