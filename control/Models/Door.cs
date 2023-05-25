using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace control.Models
{
    public enum EDoorEntryMode
    {
        kNoAccess,
        kCardAccess,
        kUniversalAccess
    }

    public class Door
    {
        [Required]
        [Key] 
        public required string Id { get; set; }

        public EDoorEntryMode EntryStatus { get; set; } = EDoorEntryMode.kCardAccess;

        public bool DoorRegistered { get; set; } = false;

        public DateTime LastCheckInTime { get; set; }

        public string DisplayName { get; set; } = "";

        public bool SelfRegisterAllowed { get; set; } = false;


    }
}
