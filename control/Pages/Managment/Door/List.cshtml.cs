using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using control.Data;
using control.Models;

namespace control.Pages.Managment.Door
{
    public class ListModel : PageModel
    {
        private readonly control.Data.controlContext _context;

        public ListModel(control.Data.controlContext context)
        {
            _context = context;
        }

        public IList<control.Models.Door> Door { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Door != null)
            {
                Door = await _context.Door.ToListAsync();
            }
        }
    }
}
