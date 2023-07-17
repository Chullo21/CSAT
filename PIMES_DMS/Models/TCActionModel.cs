using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PIMES_DMS.Models
{
    public class TCActionModel
    {
        [Key]
        public int TCAID { get; set; }

        [DisplayName("Action(s)"), Required(ErrorMessage = "No actions")]
        public string? Action { get; set; }

        [DisplayName("PIC"), Required(ErrorMessage = "PIC left blank")]
        public string? PIC { get; set; }

        public string? Remarks { get; set; }

        public string? ControlNo { get; set; }

        public string? TESID { get; set; }

        [Required(ErrorMessage = "Please input target date.")]
        public DateTime TargetDate { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; }

        public bool HasVer { get; set; } = false;

        public string ActionStatus { get; set; } = "open";
    }
}
