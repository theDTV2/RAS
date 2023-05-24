using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace control.Models
{
    public class Door
    {
        [Required]
        [Key] 
        public required string Id { get; set; }

        [Required]
        public string AccessToken { get; set; } = "";

        public DateTime LastCheckInTime { get; set; }

        public string DisplayName { get; set; } = "Placeholder";

        public bool SelfRegisterAllowed { get; set; } = false;

        //TODO: Think about how to do this efficiently
        // public required List<DoorLog>

    }
}
