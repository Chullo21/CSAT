using System.ComponentModel.DataAnnotations;

namespace ATS_Login.Models
{
    public class SessionModel
    {
        [Key]
        public int SessionId { get; set; }

        [Required]
        public string UniqueKey { get; set; }

        [Required]
        public string AccountName { get; set; }

        public SessionModel CreateSession(string accountName, string uniqueKey)
        {
            UniqueKey = uniqueKey;
            AccountName = accountName;
            return this;
        }

    }
}
