using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdevDagitimPortali.Models.user;
using OdevDagitimPortali.Repository;
using OdevDagitimPortali.ViewModels;

namespace OdevDagitimPortali.Controllers
{
    public class UserController : Controller
    {

        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public IActionResult Index()
        {
            var user = _userRepository.GetList(); // Kullanıcıları al
            return View(user); // Veriyi view'a gönder
        }

        public IActionResult Add()
        {
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType))); // Enum'dan rolleri alıyoruz

            return View();
        }


        [HttpPost]
        public IActionResult Add(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType))); // Rolleri tekrar View'a gönderiyoruz

                return View(model);
            }
            _userRepository.Add(model);
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var user = _userRepository.GetById(id);
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType)), user.role);

            return View(user);
        }

        [HttpPost]
        public IActionResult Update(UserModel model)
        {
            if (!ModelState.IsValid)
            {        ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType)), model.role);

                return View(model);
            }
            _userRepository.Update(model);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var user = _userRepository.GetById(id); // Kullanıcıyı ID ile alıyoruz
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı."; // Eğer kullanıcı yoksa, hata mesajı gönderiyoruz
                return RedirectToAction("Index"); // Kullanıcı bulunamazsa Index sayfasına yönlendiriyoruz
            }
            return View(user); // Kullanıcıyı View'a gönderiyoruz
        }


        [HttpPost]
        public IActionResult Delete(UserModel model)
        {
            if (model == null || model.user_id == 0)
            {
                TempData["ErrorMessage"] = "Geçersiz kullanıcı bilgisi."; // Geçersiz model için hata mesajı
                return RedirectToAction("Index");
            }

            _userRepository.Delete(model.user_id); // Kullanıcıyı siliyoruz
            return RedirectToAction("Index"); // Silme işlemi başarılıysa, Index sayfasına yönlendiriyoruz
        }

    }
}
