using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class IssueModel
    {
        [Key]
        public int IssueID { get; set; }

        [DisplayName("Issue Creator")]
        [Required(ErrorMessage = "Failed to obtain issue creator.")]
        public string IssueCreator { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Input Issue number.")]
        [DisplayName("Issue#")]
        public string IssueNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please insert date found.")]
        [DisplayName("Date Found")]
        public string DateFound { get; set; } = string.Empty;

        [Required(ErrorMessage = "Failed to obtain date.")]
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please input product.")]
        public string Product { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please input Serial number.")]
        [DisplayName("Serial#")]
        public int SerialNo { get; set; }

        [Required(ErrorMessage = "Please input affected part number.")]
        [DisplayName("Affected P/N")]
        public string AffectedPN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please input at least a short description."), DisplayName("Description")]
        public string Desc { get; set; } = string.Empty;

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

        [DisplayName("Date Acknowledged")]
        public DateTime DateAck { get; set; }

        [DisplayName("Date Validated")]
        public DateTime DateVdal { get; set; }

        public bool HasCR { get; set; } = false;

        public bool isDeleted { get; set; }

        [DisplayName("Affected Quantity"), Required(ErrorMessage = "Please input quantity")]
        public int Qty { get; set; }
    }
}
