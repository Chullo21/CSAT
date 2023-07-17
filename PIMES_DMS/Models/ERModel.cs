using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class ERModel
    {
        [Key]
        public int ERID { get; set; }

        public string IssueNo { get; set; }
        public string? ControlNo { get; set; }

        public string? WHSESOH { get; set; }
        public string? WHSEGOOD { get; set; }
        public string? WHSENOGOOD { get; set; }
        public string? WHSEDis { get; set; }

        public string? IQASOH { get; set; }
        public string? IQAGOOD { get; set; }
        public string? IQANOGOOD { get; set; }
        public string? IQADis { get; set; }

        public string? WIPSOH { get; set; }
        public string? WIPGOOD { get; set; }
        public string? WIPNOGOOD { get; set; }
        public string? WIPDis { get; set; }

        public string? FGSOH { get; set; }
        public string? FGGOOD { get; set; }
        public string? FGNOGOOD { get; set; }
        public string? FGDis { get; set; }

        public bool Rep { get; set; }

        public string? RMAno { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }
}
