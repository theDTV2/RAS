using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using control.Data;
using control.Models;

namespace control.Pages.Shared
{
    public class _LanguageSelectorPartialModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public _LanguageSelectorPartialModel(control.Data.controlContext context)
        {
            _context = context;
        }

      public User User { get; set; } = default!; 


        public void OnPost()
        {
            return;
        }
    }
}
