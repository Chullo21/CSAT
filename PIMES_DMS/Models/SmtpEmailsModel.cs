using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class SmtpEmailsModel
    {
        [Key]
        public int SMTPID { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public int Port { get; set; }

        public string? SmtpServer { get; set; }
    }
}
