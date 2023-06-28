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
        public DbSet<control.Models.Door> Door { get; set; } = default!;
        public DbSet<control.Models.LoginLink> LoginLink { get; set; } = default!;
        public DbSet<control.Models.Log> Log { get; set; } = default!;

    }

}
