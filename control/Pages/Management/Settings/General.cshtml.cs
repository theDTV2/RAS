using control.Generator;
using control.Helper;
using control.Manager;
using control.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace control.Pages.Management.Site
{
    [BindProperties]
    public class GeneralModel : PageModel
    {
        private readonly control.Data.controlContext _context;
        public GeneralModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [Display(Name = "Login Timeout (In Minutes)")]
        
        public int UserLoginTimeout { get; set; }

        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kSuperAdmin))
                return Unauthorized();

            UserLoginTimeout = Convert.ToInt32(GeneralSettingsManager.GeneralSettingsObj.UserLoginTimeout.TotalMinutes);

            //TODO: General Settings Management Page

            return Page();
        }
        public IActionResult OnPost()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kSuperAdmin))
                return Unauthorized();

            GeneralSettingsManager.GeneralSettingsObj.UserLoginTimeout = TimeSpan.FromMinutes(UserLoginTimeout);

            GeneralSettingsManager.SaveGeneralSettingsToConfig();

            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, LanguageManager.GetLocalizedString("MANAGEMENT_SETTINGS_GENERAL_SUCCESS", AccountHelper.GetUserLanguage(HttpContext)));


            return Page();
        }

    }
}
