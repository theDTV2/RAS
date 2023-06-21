using control.Generator;
using control.Helper;
using control.Manager;
using control.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace control.Pages.Managment.Site
{
    [BindProperties]
    public class MailModel : PageModel
    {
        private readonly control.Data.controlContext _context;
        public MailModel(control.Data.controlContext context)
        {
            _context = context;
        }

        [Display(Name = "STMP User name:")]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "Password:")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "SMTP Adress:")]
        public string SMTPServer { get; set; } = string.Empty;

        [Display(Name = "Port:")]
        public int Port { get; set; } = 0;



        public IActionResult OnGet()
        {
            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kSuperAdmin))
                return Unauthorized();

            UserName = EMailManager.EMailSettingObj.MailUserName;
            SMTPServer = EMailManager.EMailSettingObj.MailSMTPAdress;
            Port = EMailManager.EMailSettingObj.MailPort;


            return Page();
        }


        public IActionResult OnPost()
        {

            if (!AuthHelper.CheckUserAccessWithRedirect(_context, HttpContext, EAccessLevel.kSuperAdmin))
                return Unauthorized();
            EMailManager.SetMailParameters(SMTPServer, Port, UserName, Password);

            if (!EMailManager.TestMailSettings())
            {
                AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kError, "Error. Check Mail settings");
                return Page();
            }

            AlertGenerator.AddAlertToSession(HttpContext, AlertGenerator.EAlertLevel.kSuccess, "Mail Settings correct");
            return Page();
        }


    }
}
