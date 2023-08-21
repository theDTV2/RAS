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

namespace control.Pages.Management.Door
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

        public string DoorID { get; set; }

        [BindProperty]
        public string DoorDisplayText { get; set; }

        public EDoorEntryMode DoorEntryMode { get; set; }

        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin))
                return Unauthorized();

            control.Models.Door? _door = _context.Door.Where(d => d.Id == DoorIdentifier).Include(d => d.AdminUsers).FirstOrDefault();

            if (_door is null)
                return NotFound();


            DoorID = _door.Id;
            DoorDisplayText = _door.DisplayText;
            DoorEntryMode = _door.EntryStatus;

            TempData["OldDoorAdminList"] = _door.AdminUsers.Select(s => s.UserName).ToArray();
            
            ViewData["SelectBox"] = SelectListGenerator.GenerateSelectListForUser(_context, HttpContext, _door);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kAdmin))
                return Unauthorized();

            control.Models.Door? _door = _context.Door.Where(d => d.Id == DoorIdentifier).Include(d => d.AdminUsers).FirstOrDefault();
            if (_door == null)
                return NotFound();

            _door.AdminUsers = SelectListHelper.ConvertUserIdStringsToReferences(_context, DoorAdminToGiveAccessTo);

            string[]? OldDoorAdminList = (string[]?)TempData["OldDoorAdminList"];

            if (OldDoorAdminList is null)
                OldDoorAdminList = new string[0];

            _door.DisplayText = DoorDisplayText;
            _door.EntryStatus = DoorEntryMode;

            _context.SaveChanges();

            //This must run after the SaveChanges
            AccessHelper.UpdateModeratorStatus(_context, DoorAdminToGiveAccessTo, OldDoorAdminList);

            return RedirectToPage("List");
        }
    }
}
