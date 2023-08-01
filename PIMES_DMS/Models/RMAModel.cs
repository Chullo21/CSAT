
using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class RMAModel
    {
        [Key]
        public int RMAID { get; set; }

        public DateTime DateCreated { get; set; }

        public string RMANo { get; set; } = string.Empty;

        public string IssueNo { get; set; } = string.Empty;

        public string Product { get; set; } = string.Empty;

        public string AffectedPN { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ProblemDesc { get; set; } = string.Empty;

        public int QTY { get; set; }

        public DateTime? DateReceived { get; set; }

        public byte[] FA { get; set; }
    }
}
