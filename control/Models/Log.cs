using System.ComponentModel.DataAnnotations;

namespace control.Models
{
    public class Log
    {
        public enum ELogEntryType
        {
            kGeneral,
            kInfo,
            kWarning,
            kError,
            kFatal,
            kDebug
        }

        [Required]
        [Key]
        public string Id { get; set; }

        [Required]
        public ELogEntryType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
         


    }
}
