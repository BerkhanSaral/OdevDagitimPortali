using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OdevDagitimPortali.Models.user
{
    public class Users
    {
        [Key]
        public int user_id { get; set; }
        public string fullname { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public RoleType role { get; set; }


        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }

    }
}
