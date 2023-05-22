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

        //public string? Status { get; set; } = "OPEN";

        //[DataType(DataType.Upload)]
        //public byte[]? Files { get; set; }

        public string? Remarks { get; set; }

        public string? ControlNo { get; set; }

        public string? TSC { get; set; }

        public string? EC { get; set; }

        [Required(ErrorMessage = "Please input target date.")]
        public DateTime TargetDate { get; set; }

        //public DateTime DateVer { get; set; }

        //public string Result { get; set; } = string.Empty;

        public bool IsDeleted { get; set; }

        public bool HasVer { get; set; } = false;
    }
}
