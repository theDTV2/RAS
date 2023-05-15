using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace control.Models
{

    public enum EAccessLevel
    {
        kNone,
        kUser,
        kModerator,
        kAdmin,
        kSuperAdmin
    }


    public class User
    {

        [Required]
        public required string Email { get; set; }

        [Required]
        public int? AccessCode { get; set; }

        public DateTime? AccessCodeGenerationTime { get; set; }

        [Required]
        public EAccessLevel AccessLevel { get; set; }

        public List<Door>? AccessDoors; 

    }
}
