using control.Helper;
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

            if (DoorHelper.AttemptToRegisterDoor(_context, doorID, DateTime.Parse(timeStamp), ref _returnVal))
                return _returnVal;
            return StatusCode(500);
        }

        public JsonResult OnPostAccess(string cardCode, string timeStamp, string secret)
        {
            return new JsonResult("");
        }

        public JsonResult OnPostHearbeat(string timeStamp, string secret)
        {
            return new JsonResult("");
        }

        public ActionResult OnGet()
        {
            //Browsing this page is not intended
            return NotFound();
        }

                    
    }
}
