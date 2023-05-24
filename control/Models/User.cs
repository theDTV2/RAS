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
        public string UserName { get; set; } = "";

        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddDays(7);

        public string AccessCode { get; set; } = "";

        public DateTime? AccessCodeGenerationTime { get; set; }

        public DateTime? LastLogin {get;set;}

        [Required]
        public EAccessLevel AccessLevel { get; set; } = EAccessLevel.kUser;

        public List<Door> AccessDoors = new List<Door>();

        public List<Door> AdminDoors = new List<Door>();

        public bool AcceptedEula { get; set; } = false;



    }
}
