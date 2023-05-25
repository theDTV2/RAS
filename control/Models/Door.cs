using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace control.Models
{
    public enum EDoorStatusType
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

        [Required]
        public string PrivateKeyServer{ get; set; } = "";

        [Required]
        public string PublicKeyClient { get; set; } = "";

        public EDoorStatusType DoorStatus { get; set; } = EDoorStatusType.kCardAccess;

        public DateTime LastCheckInTime { get; set; }

        public string DisplayName { get; set; } = "";

        public bool SelfRegisterAllowed { get; set; } = false;


    }
}
