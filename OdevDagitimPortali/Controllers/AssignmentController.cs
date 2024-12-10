using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Models.user;
using OdevDagitimPortali.Repository;
using OdevDagitimPortali.ViewModels;

namespace OdevDagitimPortali.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly AssignmentRepository _assignmentRepository;
        private readonly ApplicationDbContext _applicationDbContext;

        public AssignmentController(AssignmentRepository assignmentRepository, ApplicationDbContext applicationDbContext)
        {
            _assignmentRepository = assignmentRepository;
            _applicationDbContext = applicationDbContext;

        }
        public IActionResult Index()
        {
            var assignments = _assignmentRepository.GetList();
            var users = _applicationDbContext.User.ToList();

            // Görevleri ViewModel ile eşleştiriyoruz
            var assignmentViewModels = assignments.Select(a => new AssignmentModel
            {
                title = a.title,
                deadline = a.deadline,
                create_date = a.create_date,
                role = a.role,
                CreatedBy = users.FirstOrDefault(u => u.user_id == a.user_ID)?.user_name // User_ID ile kullanıcıyı eşleştir
            }).ToList();

            return View(assignmentViewModels); // View'e ViewModel gönderiyoruz
        }

        public IActionResult Add()
        {
            // Teacher veya Admin rolüne sahip kullanıcıları alıyoruz
            var teachersAndAdmins = _applicationDbContext.User
               .Where(u => u.role == RoleType.TEACHER || u.role == RoleType.ADMIN)
               .Select(u => new
               {
                   u.user_id,
                   user_display = u.user_name + " (" + u.role.ToString() + ")"
               })
               .ToList();



            // ViewBag'e doğru filtrelenmiş liste atanıyor
            ViewBag.user_ID = new SelectList(teachersAndAdmins, "user_id", "user_display");


            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType))); // Enum'dan rolleri alıyoruz


            return View();
        }

        [HttpPost]
        public IActionResult Add(AssignmentModel model)
        {
            var user = _applicationDbContext.User
              .Where(u => u.user_id == model.user_ID)
              .Select(u => new { u.user_id, u.user_name, u.role }) // Role bilgisini ekleyelim
              .FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("user_ID", "Seçilen kullanıcı mevcut değil.");
                return View(model);
            }

            // Kullanıcı rolü Teacher veya Admin değilse hata ekle
            if (user.role != RoleType.TEACHER && user.role != RoleType.ADMIN)
            {
                ModelState.AddModelError("user_ID", "Sadece öğretmen veya admin atanabilir.");
                return View(model);
            }

            // Atama modelini oluştur
            var assignment = new AssignmentModel
            {
                title = model.title,
                description = model.description,
                create_date = model.create_date,
                deadline = model.deadline,
                user_ID = model.user_ID,
                role = model.role // Burada doğru role'ü atıyoruz
            };

            try
            {
                _assignmentRepository.Add(assignment);
                _assignmentRepository.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Veritabanına kayıt eklenirken bir hata oluştu: " + ex.Message);
                return View(model);
            }

            return RedirectToAction("Index");

        }


        public IActionResult Update(int id)
        {

            var assignment = _assignmentRepository.GetById(id);

            // Filtrelenmiş liste tekrar yükleniyor
            var teachersAndAdmins = _applicationDbContext.User
                .Where(u => u.role == RoleType.TEACHER || u.role == RoleType.ADMIN)
                .Select(u => new { u.user_id, u.user_name })
                .ToList();

            ViewBag.user_ID = new SelectList(teachersAndAdmins, "user_id", "user_name");

            return View(assignment);
        }

        [HttpPost]
        public IActionResult Update(AssignmentModel model)
        {
            if (!ModelState.IsValid)
            {
                // Filtrelenmiş liste tekrar yükleniyor
                var teachersAndAdmins = _applicationDbContext.User
                    .Where(u => u.role == RoleType.TEACHER || u.role == RoleType.ADMIN)
                    .Select(u => new { u.user_id, u.user_name })
                    .ToList();

                ViewBag.user_ID = new SelectList(teachersAndAdmins, "user_id", "user_name");

                return View();

            }
            _assignmentRepository.Update(model);
            _assignmentRepository.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var assignment = _assignmentRepository.GetById(id);
            if (assignment == null)
            {
                TempData["ErrorMessage"] = "Ödev bulunamadı.";
                return RedirectToAction("Index");  // Ödev bulunamazsa Index sayfasına yönlendir
            }

            // Ödev bulunursa silme işlemini onaylatmak için View'a gönderiyoruz.
            return View(assignment);
        }

        // POST Delete: Silme işlemi onaylandıktan sonra gerçekleştirilir.
        [HttpPost]
        public IActionResult Delete(AssignmentModel model)
        {
            if (model.assignment_id <= 0)
            {
                TempData["ErrorMessage"] = "Geçersiz ödev ID'si.";
                return RedirectToAction("Index");  // ID geçerli değilse Index sayfasına yönlendir.
            }

            try
            {
                // Silme işlemi
                _assignmentRepository.Delete(model.assignment_id);
                TempData["SuccessMessage"] = "Ödev başarıyla silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Silme işlemi sırasında bir hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
