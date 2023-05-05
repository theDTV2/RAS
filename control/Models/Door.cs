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
        public required string AccessToken { get; set; }

        public required DateTime LastCheckInTime { get; set; }

        //TODO: Think about how to do this efficiently
        // public required List<DoorLog>

    }
}
