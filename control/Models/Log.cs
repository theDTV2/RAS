using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace control.Models
{

    public enum EntryResult
    {
        [Display(Name = "USER_ACCESS_ALLOWED")]
        AccessAllowed,
        [Display(Name = "USER_ACCESS_DENIED")]
        AccessDenied
    }

    public class Log
    {
        [Key]
 
        public int Id { get; set; }

        public required User User { get; set; }

        public required Door Door { get; set; }

        public required DateTime EntryTime { get; set; }

        public required EntryResult EntryResult { get; set; }


    }
}
