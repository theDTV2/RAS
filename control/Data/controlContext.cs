using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using control.Models;

namespace control.Data
{
    public class controlContext : DbContext
    {
        public controlContext (DbContextOptions<controlContext> options)
            : base(options)
        {
        }

        public DbSet<control.Models.User> User { get; set; } = default!;
    }
}
