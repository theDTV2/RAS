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

namespace control.Pages.Managment.Door
{
    public class ManageModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ManageModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string[] DoorAdminToGiveAccessTo { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DoorIdentifier { get; set; } = "";

        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin))
                return Unauthorized();

            control.Models.Door? _door = _context.Door.Where(d => d.Id == DoorIdentifier).Include(d => d.AdminUsers).FirstOrDefault();

            if (_door == null)
                return NotFound();


            ViewData["SelectBox"] = SelectListGenerator.GenerateSelectListForUser(_context, HttpContext, _door);

            return Page();
        }

        public IActionResult OnPost()
        {
            control.Models.Door? _door = _context.Door.Where(d => d.Id == DoorIdentifier).Include(d => d.AdminUsers).FirstOrDefault();

            _door.AdminUsers = SelectListHelper.ConvertUserIdStringsToReferences(_context, DoorAdminToGiveAccessTo);

            _context.SaveChanges();

            return RedirectToPage("List");
        }
    }
}
