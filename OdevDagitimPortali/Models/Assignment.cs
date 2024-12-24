using OdevDagitimPortali.Models.user;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OdevDagitimPortali.Models
{
    public class Assignment
    {
        [Key]
        public int assignment_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string created_date { get; set; }
        public string dead_line { get; set; }
        public RoleType role { get; set; }

        // Foreign Key
        public int user_ID { get; set; }
        [ForeignKey("user_ID")]
        public virtual Users User { get; set; }

    }
}
