using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class TESModel
    {
        [Key]
        public int TESID { get; set; }

        public string? ControlNo { get; set; }

        public string? TCWhy1 { get; set; }

        public string? TCWhy2 { get; set; }

        public string? TCWhy3 { get; set; }

        public string? TCWhy4 { get; set; }

        public string? TCWhy5 { get; set; }

        public string? TRC { get; set; }

        public string? ECWhy1 { get; set; }

        public string? ECWhy2 { get; set; }

        public string? ECWhy3 { get;set; }

        public string? ECWhy4 { get; set; }

        public string? ECWhy5 { get; set; }

        public string? ERC { get; set; }

        public string? SCWhy1 { get; set; }

        public string? SCWhy2 { get; set; }
        
        public string? SCWhy3 { get; set; }

        public string? SCWhy4 { get; set; }

        public string? SCWhy5 { get; set; }

        public string? SRC { get; set; }
    }
}
