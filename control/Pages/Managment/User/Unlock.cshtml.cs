using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using control.Data;
using control.Models;
using control.Generator;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using control.Helper;
using control.Manager;

namespace control.Pages.Managment.User
{
    public class UnlockModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public UnlockModel(control.Data.controlContext context)
        {
            _context = context;
        }



        [BindProperty(SupportsGet = true)]
        public string? UserIdentifier { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public bool FoundElement { get; set; } = false;



        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string Secret { get; set; }
        [BindProperty]
        public string[] DoorsToGiveAccessTo { get; set; }


        public new Models.User User { get; set; }

        public IActionResult OnGet()
        {
            if (_context.User.Where(u => u.UserName == UserIdentifier).Count() != 1)
            {
                return RedirectToPage("List");
            }

            User = _context.User.Where(u => u.UserName == UserIdentifier).Include(u => u.AccessDoors).Include(u => u.AdminDoors).Include(u => u.AdminDoors).First();


            if ((UserIdentifier is null) || (User is null))
            {
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, "User missing or invalid");

                return RedirectToPage("List");
            }

            LastName = User.LastName;
            FirstName = User.FirstName;
            Secret = User.SecretCode;
            ViewData["SelectBox"] = SelectListGenerator.GenerateSelectListForDoor(_context, HttpContext, User);

            return Page();
        }

        public IActionResult OnPost()
        {
            User = _context.User.Where(u => u.UserName == UserIdentifier).Include(u => u.AccessDoors).Include(u => u.AdminDoors).First();

            //Check, if secret code is unique
            if (_context.User.Where(u => u.SecretCode == Secret&& u.UserName != User.UserName).Any())
            {
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, "Card code is already being used");
                return OnGet();
            }


            User.LastName = LastName;
            User.FirstName = FirstName;
            User.SecretCode = Secret;

            User.AccessDoors = SelectListHelper.ConvertDoorIdStringsToReferences(_context, DoorsToGiveAccessTo).ToList();

            _context.SaveChanges();

            //TODO: Proper Error catching

            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, "Saving successfull");

            UserStateManager.SetUpdatePermissionsRequired(User.UserName, User.AccessLevel);


            return RedirectToPage("List");
        }

    }
}
