using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OdevDagitimPortali.Repository;
using OdevDagitimPortali.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace OdevDagitimPortali.Controllers
{
    public class HomeController : Controller
    {/*
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;
        private readonly UserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, INotyfService notyf, UserRepository userRepository)
        {
            _logger = logger;
            _notyf = notyf;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) // Kullanıcı giriş yapmamışsa
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            Debug.WriteLine("Suan burada 1 ");

            // Model geçerli mi kontrol et
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Suan burada 2 ");
                return View(model);
            }

            // Kullanıcıyı doğrula
            Debug.WriteLine("Suan burada 3 ");
            var user = await _userRepository.AuthenticateUserAsync(model.user_name, model.password);
            Debug.WriteLine("Suan burada 4 ");

            // Kullanıcı doğrulandı mı?
            if (user != null)
            {
                Debug.WriteLine("Suan burada 5 ");

                // Kullanıcı doğrulandıysa, admin paneline yönlendir
                if (user.role == Models.user.RoleType.ADMIN || user.role == Models.user.RoleType.TEACHER) // Admin rolü ile kontrol edebilirsiniz
                {
                    return RedirectToAction("AdminPanel", "Admin"); // Admin paneline yönlendir
                }

                // Kullanıcı admin değilse, anasayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }

            Debug.WriteLine("Suan burada 6");

            // Geçersiz kullanıcı adı veya şifre
            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya parola.");
            return View(model); // Geçersiz giriş durumunda tekrar Login sayfasını göster
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Burada kullanıcı veritabanına kaydedilebilir.
            // Mock kayıt sonrası Login sayfasına yönlendirme.
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Home");
        }

        */
    }
}
