using OdevDagitimPortali.Models.user;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OdevDagitimPortali.Models
{
    public class Submission
    {
        [Key]
        public int submission_id { get; set; }

        
        public int assignment_ID { get; set; }
        [ForeignKey("assignment_ID")]
        public virtual Assignment Assignment { get; set; } 


        public int user_ID { get; set; }  
        [ForeignKey("user_ID")]
        public virtual Users User { get; set; }


        public string submission_date{ get; set; }

        public int FileUploadId { get; set; }
        public virtual FileUpload file_url{ get; set; }

    }
}
