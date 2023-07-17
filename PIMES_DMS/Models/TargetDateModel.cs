using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class TargetDateModel
    {
        [Key]
        public int TDID { get; set; }

        public int ActionID { get; set; }

        public string ControlNo { get; set; }

        public string Status { get; set; }

        public DateTime TD { get; set; }
    }
}
