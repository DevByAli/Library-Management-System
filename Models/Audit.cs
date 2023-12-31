using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Audit
    {
        [Key]
        public int id { get; set; }
        public string CreatedBy { get; set; } = "admin";
        public string ModifiedBy { get; set; } = "admin";
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
