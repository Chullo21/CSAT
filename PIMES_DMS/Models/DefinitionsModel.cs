using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class DefinitionsModel
    {
        [Key]
        public int DefID { get; set; }

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Definition { get; set; } = string.Empty;
    }
}
