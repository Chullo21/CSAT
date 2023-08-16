using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PIMES_DMS.Models
{
    public class ActionModel
    {
        [Key]
        public int ActionID { get; set; }

        [DisplayName("Action(s)"), Required(ErrorMessage = "No actions")]
        public string? Action { get; set; }

        [DisplayName("PIC"), Required(ErrorMessage = "PIC left blank")]
        public string? PIC { get; set; }

        public string? Remarks { get; set; }

        public string? ControlNo { get; set; }

        public string? TESID { get; set; }

        public string? Type { get; set; }

        [Required(ErrorMessage = "Please input target date.")]
        public DateTime TargetDate { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public string Dependency { get; set; }

        public bool IsDeleted { get; set; }

        public bool HasVer { get; set; } = false;

        public string ActionStatus { get; set; } = "Open";

        public bool VerStatus { get; set; }

        public DateTime? DateVerified { get; set; }

        public byte[]? VerificationFile { get; set; }

        public string? VerRemarks { get; set; }
    }
}
