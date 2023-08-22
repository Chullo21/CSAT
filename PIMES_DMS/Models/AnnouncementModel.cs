using System.ComponentModel.DataAnnotations;

namespace PIMES_DMS.Models
{
    public class AnnouncementModel
    {
        [Key]
        public int AnnID { get; set; }

        public string AnnouncementMessage { get; set; } = string.Empty;


    }
}
