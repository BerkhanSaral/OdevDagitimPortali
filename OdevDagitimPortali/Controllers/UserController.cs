using AspNetCoreHero.ToastNotification.Abstractions;
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
        private readonly INotyfService _notyf;

        public UserController(UserRepository userRepository, INotyfService notyf)
        {
            _userRepository = userRepository;
            _notyf = notyf;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userRepository.GetAllAsync(); // Kullanıcıları al
            var userModels = user.Select(u => new UserModel
            {
                user_id = u.user_id,
                fullname = u.fullname,
                user_name = u.user_name,
                email = u.email,
                password = u.password,
                role = u.role
            }).ToList();

            // View'e dönüştürülmüş UserModel listesini gönder
            return View(userModels);
            return View(user); // Veriyi view'a gönder
        }

        public IActionResult Add()
        {
            // Enum'dan rolleri alıyoruz ve ViewBag'e gönderiyoruz
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType)));
            return View();
        }

        // Add Action (POST)
        [HttpPost]
        public async Task<IActionResult> Add(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                // Model geçerli değilse, rolleri tekrar gönderiyoruz
                ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType)), model.role);
                return View(model);
            }

            // Manuel dönüşüm: UserModel'i User entity'sine dönüştürme
            var user = new Users
            {
                fullname = model.fullname,
                user_name = model.user_name,
                email = model.email,
                password = model.password,
                role = model.role
            };

            // Veritabanına ekliyoruz
            await _userRepository.AddAsync(user);

            // Kullanıcı eklenmişse bildirim gönderiyoruz
            _notyf.Success("Kullanıcı Eklendi...");
            return RedirectToAction("Index");
        }

        // Update Action (GET)
        public async Task<IActionResult> Update(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            // Kullanıcı verisini ve enum değerlerini View'e gönderiyoruz
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType)), user.role);
            return View(user);
        }

        // Update Action (POST)
        [HttpPost]
        public async Task<IActionResult> Update(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                // Model geçerli değilse, rolleri tekrar View'a gönderiyoruz
                ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType)), model.role);
                return View(model);
            }

            // Manuel dönüşüm: UserModel'i User entity'sine dönüştürme
            var user = new Users
            {
                user_id= model.user_id,
                fullname= model.fullname,
                user_name= model.user_name,
                email = model.email,
                password= model.password,
                role=model.role

            };

            // Kullanıcıyı güncelliyoruz
            await _userRepository.UpdateAsync(user);

            // Kullanıcı güncellenmişse bildirim gönderiyoruz
            _notyf.Success("Kullanıcı Güncellendi...");
            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> Delete(UserModel model)
        {
            if (model == null || model.user_id == 0)
            {
                TempData["ErrorMessage"] = "Geçersiz kullanıcı bilgisi."; // Geçersiz model için hata mesajı
                return RedirectToAction("Index");
            }

            await _userRepository.DeleteAsync(model.user_id); // Kullanıcıyı siliyoruz
            _notyf.Success("Kullanici Silindi...");
            return RedirectToAction("Index"); // Silme işlemi başarılıysa, Index sayfasına yönlendiriyoruz
        }

    }
}
