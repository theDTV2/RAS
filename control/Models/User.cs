using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public enum EAccessReturnValue
    {
        kAccessGranted,
        kAdminGranted,
        kAccessDenied,
        kCodeExpired,
        kPermissionDenied,
        kAccountExpired,
        kAccountLocked
    }

    public class User
    {

        [Required]
        [Key]
        public required string Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public required DateTime ExpiryDate { get; set; }

        public string? AccessCode { get; set; }

        public DateTime? AccessCodeGenerationTime { get; set; }

        public DateTime? LastLogin {get;set; }

        [Required]
        public EAccessLevel AccessLevel { get; set; }

        public List<Door> AccessDoors = new List<Door>();

        public List<Door> AdminDoors = new List<Door>();





    }
}
