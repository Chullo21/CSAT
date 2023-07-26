using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class AccountsModel
    {
        [Key]
        public int AccID { get; set; }

        [Required, DisplayName("Account Unique ID")]
        public string AccUCode { get; set; } = string.Empty;

        [Required]
        [DisplayName("Name")]
        public string AccName { get; set; } = string.Empty;

        [Required, DisplayName("Section")]
        public string Section { get; set; } = string.Empty;

        
        [Required, DisplayName("Role")]
        public string Role { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? Email { get; set; }

        public bool isDeleted { get; set; }
    }

    public enum Roles
    {
        Admin,
        Verificator,
        Viewer
    }

    public enum Sections
    {
        QA,
        QE,
        Engineering,
        BC,
        Purchasing,
        PPIC,
        Production,
        QAManager,
        QEManager,
        EngineeringManager,
        BCManager,
        PurchasingManager,
        PPICManager,
        ProductionManager,
        VicePresident
    }
}
