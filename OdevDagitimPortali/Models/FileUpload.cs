using OdevDagitimPortali.Models.user;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OdevDagitimPortali.Models
{
    public class FileUpload
    {
        [Key]
        public int Id { get; set; }  // Dosyanın benzersiz ID'si
        public string FileName { get; set; }  // Dosyanın adı
        public byte[] FileContent { get; set; }  // Dosyanın içeriği (binary format)
        public DateTime UploadDate { get; set; }  // Yüklenme tarihi

        public int user_ID { get; set; }
        [ForeignKey("user_ID")]
        public virtual Users User { get; set; }

    }
}
