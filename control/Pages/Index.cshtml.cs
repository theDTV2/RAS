using control.Manager;
using control.Models;
using control.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using NuGet.Packaging.Signing;
using System.Reflection;
using System.Resources;
using System.Text.Json.Nodes;
using System;



namespace control.Pages
{
    public class IndexModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public IndexModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            //ResourceManager myManager = new ResourceManager(typeof(language));
            //string myString = myManager.GetString("StringKey");



            var test1 = control.Resources.English.Test;
            var test2 = control.Resources.German.Test;
      

            return Page();
        }
    }
}