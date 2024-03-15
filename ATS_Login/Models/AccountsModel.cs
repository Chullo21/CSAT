using System.ComponentModel.DataAnnotations;

namespace ATS_Login.Models
{
    public class AccountsModel
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
