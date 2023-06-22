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
using System.Globalization;

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



            // Assuming you have a reference to the generated resource class "control.Resources"
            var resourceManager1 = new ResourceManager("control.Resources.Localization", Assembly.GetExecutingAssembly());
            //var resourceManager1 = new ResourceManager("control.Resource", Assembly.GetExecutingAssembly());
         
            // Get the German version of the "Test" resource
            var eng = resourceManager1.GetString("Test", new CultureInfo("en"));
            var eng2 = resourceManager1.GetString("Test", new CultureInfo("de"));

            

            // Get the English version of the "Test" resource
           // var englishTest = resourceManager.GetString("Test", new System.Globalization.CultureInfo("en"));


            return Page();
        }
    }
}