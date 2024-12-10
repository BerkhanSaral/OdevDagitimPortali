using System.ComponentModel.DataAnnotations;

namespace OdevDagitimPortali.ViewModels
{
    public class LoginModel
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı Adı Giriniz!")]
        public string user_name { get; set; }

        [Display(Name = "Parola")]
        [Required(ErrorMessage = "Parola Giriniz!")]
        public string password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool keep_me { get; set; }
    }
}
