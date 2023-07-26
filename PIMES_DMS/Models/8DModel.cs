using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class _8DModel
    {
        [Key]
        public int _8DID { get; set; }

        [Required]
        public byte[] Report { get; set; }

        [Required]
        public string ControlNo { get; set; }

    }
}
