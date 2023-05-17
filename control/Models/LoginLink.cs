using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace control.Models
{
    public class LoginLink
    {

        [Required]
        [Key]
        public required string Key { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public required string Email { get; set; }

        [Required]
        public required DateTime GenerationTime { get; set; }

    }
}
