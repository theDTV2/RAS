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


        [Display(Name = "Email Login Suffix (Example: htw-berlin.de)")]
        public string EMailSuffix { get; set; }

        [Display(Name = "Full Site Hostname (Example: site.f1.htw-berlin.de")]
        public string Hostname { get; set; }


        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kSuperAdmin))
                return Unauthorized();

            UserLoginTimeout = Convert.ToInt32(GeneralSettingsManager.GeneralSettingsObj.UserLoginTimeout.TotalMinutes);
            EMailSuffix = GeneralSettingsManager.GetEmailSuffix();
            Hostname = GeneralSettingsManager.GetHostName();
            //TODO: General Settings Management Page

            return Page();
        }
        public IActionResult OnPost()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kSuperAdmin))
                return Unauthorized();

            GeneralSettingsManager.GeneralSettingsObj.UserLoginTimeout = TimeSpan.FromMinutes(UserLoginTimeout);
            GeneralSettingsManager.GeneralSettingsObj.EMailSuffix = EMailSuffix;
            GeneralSettingsManager.GeneralSettingsObj.Hostname = Hostname;

            GeneralSettingsManager.SaveGeneralSettingsToConfig();

            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, LanguageManager.GetLocalizedString("MANAGEMENT_SETTINGS_GENERAL_SUCCESS", AccountHelper.GetUserLanguage(HttpContext)));


            return Page();
        }

    }
}
