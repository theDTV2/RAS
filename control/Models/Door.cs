using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace control.Models
{
    public enum EDoorEntryMode
    {
        [Display(Name = "registrierte Moderatoren/Administratoren")]
        kNoAccess,
        [Display(Name = "freigeschaltete Nutzer")]
        kCardAccess,
        [Display(Name = "registrierten Nutzer")]
        kUniversalAccess
    }

    public class Door
    {
        [Required]
        [Key] 
        public required string Id { get; set; }

        public EDoorEntryMode EntryStatus { get; set; } = EDoorEntryMode.kCardAccess;

        public bool Registered { get; set; } = false;

        public string Secret { get; set; } = "";

        public DateTime LastCheckInTime { get; set; }

        public string DisplayName { get; set; } = "";

        public bool SelfRegisterAllowed { get; set; } = false;


        [InverseProperty("AccessDoors")]
        public virtual ICollection<User> AccessUsers { get; set; }  = new List<User>();

        [InverseProperty("AdminDoors")]
        public virtual ICollection<User> AdminUsers { get; set; } = new List<User>();



    }
}
