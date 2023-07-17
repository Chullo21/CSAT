using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class NotifModel
    {
        [Key]
        public int NotifID { get; set; }
        public DateTime DateCreated { get; set; }
        public string Message { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }
}
