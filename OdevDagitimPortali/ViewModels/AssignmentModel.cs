using OdevDagitimPortali.Models;
using OdevDagitimPortali.Models.user;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OdevDagitimPortali.ViewModels
{
    public class AssignmentModel
    {
        public int assignment_id { get; set; }


        [Display(Name = "Odev Basligi")]
        [Required(ErrorMessage = "Lutfen Odevin Adini Giriniz!")]
        public string title { get; set; }


        [Display(Name = "Odev Aciklama")]
        [Required(ErrorMessage = "Lutfen Odevin Detaylarini Giriniz!")]
        public string description { get; set; }


        [Display(Name = "Odev Baslangic Tarihi")]
        [Required(ErrorMessage = "Lutfen Odevin Baslangic Tarihini Giriniz!")]
        public string create_date { get; set; }


        [Display(Name = "Odev Bitis Tarihi")]
        [Required(ErrorMessage = "Lutfen Odevin Bitis Tarihini Giriniz!")]
        public string deadline { get; set; }

        [Display(Name = "Role Girin")]
        [Required(ErrorMessage = "Lutfen Rolunu Girin!")]
        public RoleType role { get; set; }


        public string CreatedBy { get; set; } // Görevi kim ekledi



        [Display(Name = "Odevi Veren")]
        [Required(ErrorMessage = "Lutfen Odevle Ilgili Akademisyeni Giriniz!")]
        public int user_ID { get; set; }
        public virtual Users User { get; set; } // User tablosu ile ilişkilendirme
    }
}
