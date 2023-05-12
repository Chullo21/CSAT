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

        public bool? Status { get; set; }

        [DataType(DataType.Upload)]
        public byte[]? Files { get; set; }

        public string? Remarks { get; set; }

        public string? ControlNo { get; set; }

        public string? TSC { get; set; }

        public string? EC { get; set; }

        public DateTime? TargetDate { get; set; }
    }
}
