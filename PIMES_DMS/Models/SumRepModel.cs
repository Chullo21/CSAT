using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class SumRepModel
    {
        [Key]
        public int SumRepID { get; set; }

        public string? DefCat { get; set; }

        public DateTime DateFound { get; set; } = DateTime.Now;

        public string?  IssueNo { get; set; }

        public string? Product { get; set; }

        public int AffectedQty { get; set; }

        public string? ProblemDesc { get; set; }

    }
}
