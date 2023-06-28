using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace control.Models
{
    public class Log
    {
        [Required]
        [Key]
        public required string Id { get; set; }

        public required User User { get; set; }

        public required Door Door { get; set; }


        public required DateTime EntryTime { get; set; }

    }
}
