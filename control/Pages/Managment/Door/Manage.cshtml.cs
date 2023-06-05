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
    public class ManageModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ManageModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Display(Name = "Tür Moderatoren:")]
        public string[] DoorAdminToGiveAccessTo { get; set; }
  

        [BindProperty(SupportsGet = true)]
        public string DoorIdentifier { get; set; } = "";

        public string DoorID { get; set; }

        [BindProperty]
        [Display(Name = "Name des Displays:")]
        public string DoorDisplayName { get; set; }

        [Display(Name = "Tür Eintrittsmodus:")]
        public EDoorEntryMode DoorEntryMode { get; set; }

        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin))
                return Unauthorized();

            control.Models.Door? _door = _context.Door.Where(d => d.Id == DoorIdentifier).Include(d => d.AdminUsers).FirstOrDefault();

            if (_door is null)
                return NotFound();


            DoorID = _door.Id;
            DoorDisplayName = _door.DisplayName;
            DoorEntryMode = _door.EntryStatus;

            TempData["OldDoorAdminList"] = _door.AdminUsers.Select(s => s.UserName).ToArray();
            
            ViewData["SelectBox"] = SelectListGenerator.GenerateSelectListForUser(_context, HttpContext, _door);

            return Page();
        }

        public IActionResult OnPost()
        {
            control.Models.Door? _door = _context.Door.Where(d => d.Id == DoorIdentifier).Include(d => d.AdminUsers).FirstOrDefault();
            if (_door == null)
                return NotFound();

            _door.AdminUsers = SelectListHelper.ConvertUserIdStringsToReferences(_context, DoorAdminToGiveAccessTo);

            string[]? OldDoorAdminList = (string[]?)TempData["OldDoorAdminList"];

            if (OldDoorAdminList is null)
                OldDoorAdminList = new string[0];

            _door.DisplayName = DoorDisplayName;
            _door.EntryStatus = DoorEntryMode;

            _context.SaveChanges();

            //This must run after the SaveChanges
            AccessHelper.UpdateModeratorStatus(_context, DoorAdminToGiveAccessTo, OldDoorAdminList);

            return RedirectToPage("List");
        }
    }
}
