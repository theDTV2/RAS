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
        [Display(Name = "USER_LEVEL_LOCKED")]
        kNone,
        [Display(Name = "USER_LEVEL_USER")]
        kUser,
        [Display(Name = "USER_LEVEL_MODERATOR")]
        kModerator,
        [Display(Name = "USER_LEVEL_ADMIN")]
        kAdmin,
        [Display(Name = "USER_LEVEL_SUPERADMIN")]
        kSuperAdmin
    }

    public enum EAccessReturnValue
    {
        kAccessGranted,
        kAdminGranted,
        kAccessDenied,
        kPermissionDenied,
        kAccountExpired,
        kAccountLocked,
        kAccountEulaNotAccepted,
        kAccountRegistrationNotCompleted,
        kAccountNotFound
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

        [Required]
        public EAccessLevel AccessLevel { get; set; } = EAccessLevel.kUser;
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddDays(7);

        public string AccessCode { get; set; } = "";
        public DateTime? AccessCodeGenerationTime { get; set; }
        public DateTime? LastLogin {get;set;}

        [ForeignKey("AccessUsers")]
        public virtual ICollection<Door> AccessDoors { get; set; } = new List<Door>();

        [ForeignKey("AdminUsers")]
        public virtual ICollection<Door> AdminDoors { get; set; } = new List<Door>();

        public bool AcceptedEula { get; set; } = false;

        public bool CompletedRegistration { get; set; } = false;

        public string SecretCode { get; set; } = "";

        public string Language { get; set; } = "en";

    }
}


