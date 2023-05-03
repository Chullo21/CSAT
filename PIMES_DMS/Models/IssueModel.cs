using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class IssueModel
    {
        [Key]
        public int IssueID { get; set; }

        [Required]
        public string IssueCreator { get; set; } = string.Empty;

        [Required]
        [DisplayName("Issue#")]
        public string IssueNo { get; set; } = string.Empty;

        [Required]
        [DisplayName("Date Found")]
        public string DateFound { get; set; } = string.Empty;

        [Required]
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required]
        public string Product { get; set; } = string.Empty;

        [Required]
        [DisplayName("Serial#")]
        public int SerialNo { get; set; }

        [Required]
        [DisplayName("Affected P/N")]
        public string AffectedPN { get; set; } = string.Empty;

        [Required]
        [DisplayName("Description")]
        public string Desc { get; set; } = string.Empty;

        [Required]
        [DisplayName("Problem Description")]
        public string ProbDesc { get; set; } = string.Empty;

        [DisplayName("Attachment")]
        public byte[]? ClientRep { get; set; }

        public bool Acknowledged { get; set; } = false;

        [DisplayName("Validated Status")]
        public bool ValidatedStatus { get; set; }

        [DisplayName("Validation Report")]
        public string ValidationRepSum { get; set; } = string.Empty;

        [DisplayName("8D or CAPA")]
        public string CoD { get; set; } = string.Empty; // 8D or CAPA

        [DisplayName("Validation Attachment")]
        public byte[]? Report { get; set; }

        [DisplayName("Control No.")]
        public string ControlNumber { get; set; } = string.Empty;
    }
}
