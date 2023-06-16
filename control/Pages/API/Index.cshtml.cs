using control.Helper;
using control.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;

namespace control.Pages.API
{
    //As our API is stateless, the token is not needed here
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public IndexModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IActionResult OnPostRegister(string doorID, string timeStamp)
        {
            JsonResult _returnVal = new("");

            if (DoorHelper.AttemptToRegisterDoor(_context, doorID, timeStamp, ref _returnVal))
                return _returnVal;
            return StatusCode(500);
        }

        public IActionResult OnPostAccess(string doorID, string cardCode, string timeStamp, string secret)
        {
            JsonResult _returnVal = new("");

            if (DoorHelper.RequestDoorAccess(_context, doorID, secret, cardCode, timeStamp, ref _returnVal))
                return _returnVal;

            return StatusCode(500);
        }

        public IActionResult OnPostHeartbeat(string doorID, string timeStamp, string secret)
        {
            JsonResult _returnVal = new("");


            if (DoorHelper.RegisterHeartBeat(_context, doorID, secret, timeStamp, ref _returnVal))
                return _returnVal;

            return StatusCode(500);
        }

        public ActionResult OnGet()
        {
            //Browsing this page is not intended
            return Page();
        }


    }
}
