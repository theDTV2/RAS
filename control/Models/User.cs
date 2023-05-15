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
        [Key]
        public required string Email { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? AccessCode { get; set; }

        public DateTime? AccessCodeGenerationTime { get; private set; }

        public DateTime? LastDoorAccessTime { get; set; }

        public DateTime? LastControlLogin {get;set; }

        [Required]
        public EAccessLevel AccessLevel { get; set; }

        public List<Door>? AccessDoors; 





    }
}
