using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class SCVersModel
    {
        [Key]
        public int VerID { get; set; }

        [Required]
        public string? Verificator { get; set; }

        [Required]
        public int ActionID { get; set; }

        [Required]
        public string ControlNo { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "o";

        [DataType(DataType.Upload)]
        public byte[]? Files { get; set; }

        public DateTime DateVer { get; set; }

        public DateTime? DateClosed { get; set; }

        public string? Result { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
    }
}
