using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class ART_8DModel
    {
        [Key]
        public int ARTID { get; set; }

        [Required]
        public string? ControlNo { get; set; }

        [Required]
        public DateTime DateValidated { get; set; }

        [Required]
        public DateTime DateClosed { get; set; }
    }
}
