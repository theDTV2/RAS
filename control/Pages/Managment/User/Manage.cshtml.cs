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
using control.Helper;
using control.Generator;
using control.Manager;

namespace control.Pages.Managment.User
{
    public class ManageModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ManageModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [BindProperty]
        public new control.Models.User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.User == null)
                return NotFound();

            var user =  await _context.User.Include(m => m.AdminDoors).FirstOrDefaultAsync(m => m.UserName == id);
            if (user == null)
                return NotFound();

            var _userEAccessLevel = AccountHelper.GetEAccessLevel(HttpContext);
            if (user.AccessLevel >= _userEAccessLevel && _userEAccessLevel !=EAccessLevel.kSuperAdmin )
                return NotFound();

            ViewData["AccessLevelSelectList"] = SelectListGenerator.GetAccessListByUserAccess(_context, HttpContext, user);

            User = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var _user = await _context.User.FirstOrDefaultAsync(m => m.UserName == User.UserName);

            if (_user == null) 
                return NotFound();

            var _userEAccessLevel = AccountHelper.GetEAccessLevel(HttpContext);
            if (_user.AccessLevel >= _userEAccessLevel && _userEAccessLevel != EAccessLevel.kSuperAdmin)
                return NotFound();

            

            //TODO: Check for unique access card code
            //TODO: Detect manipulation of user rights

            if (!ModelState.IsValid)
                return Page();

              //Use TryUpdateModelAsync to prevent data manipulation
            
            _user.FirstName = User.FirstName;
            _user.LastName = User.LastName;
            _user.AccessLevel = User.AccessLevel;
            _user.ExpiryDate = User.ExpiryDate;
            _user.SecretCode = User.SecretCode;


            try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) 
                {
                    return NotFound();
                }

            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, "Saving successfull");

            UserStateManager.SetUpdatePermissionsRequired(User.UserName, User.AccessLevel);


            return RedirectToPage("./List");
        }

    }
}
