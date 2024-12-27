using AspNetCoreHero.ToastNotification.Abstractions;
using OdevDagitimPortali.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Models;
using OdevDagitimPortali.Models.user;
using OdevDagitimPortali.Repository;
using OdevDagitimPortali.ViewModels;

using System.Diagnostics;

namespace OdevDagitimPortali.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly AssignmentRepository _assignmentRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserRepository _userRepository;

        private readonly INotyfService _notyf;

        public AssignmentController(UserRepository userRepository, AssignmentRepository assignmentRepository, ApplicationDbContext applicationDbContext, INotyfService notyf)
        {
            _assignmentRepository = assignmentRepository;
            _applicationDbContext = applicationDbContext;
            _userRepository = userRepository;
            _notyf = notyf;

        }
        public async Task<IActionResult> Index()
        {
            var assignments = await _assignmentRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();

            // Görevleri ViewModel ile eşleştiriyoruz
            var assignmentViewModels = assignments.Select(a => new AssignmentModel
            {
                title = a.title,
                deadline = a.dead_line,
                create_date = a.created_date,
                role = a.role,
                CreatedBy = users.FirstOrDefault(u => u.user_id == a.user_ID)?.user_name // User_ID ile kullanıcıyı eşleştir
            }).ToList();

            return View(assignmentViewModels); // View'e ViewModel gönderiyoruz
        }

        public async Task<IActionResult> Add()
        {
            // Teacher veya Admin rolüne sahip kullanıcıları alıyoruz
            var teachersAndAdmins = await _userRepository
               .Where(u => u.role == RoleType.TEACHER || u.role == RoleType.ADMIN)
               .Select(u => new
               {
                   u.user_id,
                   user_display = u.user_name + " (" + u.role.ToString() + ")"
               })
               .ToListAsync();



            // ViewBag'e doğru filtrelenmiş liste atanıyor
            ViewBag.user_ID = new SelectList(teachersAndAdmins, "user_id", "user_display");
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType))); // Enum'dan rolleri alıyoruz


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AssignmentModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage); // Hataları konsola yazdırabilirsiniz
            }
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType)));


            var user = await _userRepository.GetByIdAsync(model.user_ID);
            if (user == null || (user.role != RoleType.TEACHER && user.role != RoleType.ADMIN))
            {
                ModelState.AddModelError("user_ID", "Seçilen kullanıcı öğretmen veya admin olmalıdır.");
                return View(model);
            }
            var assignment = new Assignment
            {
                title = model.title,
                description = model.description,
                created_date = model.create_date,
                dead_line = model.deadline,
                user_ID = model.user_ID,
                role = model.role
            };
            try
            {
                await _assignmentRepository.AddAsync(assignment);
                _notyf.Success("Ödev eklendi!");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ödev eklenirken bir hata oluştu: " + ex.Message);
                ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType)));
                return View(model);
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(id);

            var teachersAndAdmins = await _userRepository
                .Where(u => u.role == RoleType.TEACHER || u.role == RoleType.ADMIN)
                .Select(u => new { u.user_id, u.user_name })
                .ToListAsync();

            ViewBag.user_ID = new SelectList(teachersAndAdmins, "user_id", "user_name", assignment?.user_ID);
            return View(assignment);
        }


        [HttpPost]
        public async Task<IActionResult> Update(AssignmentModel model)
        {
            if (!ModelState.IsValid)
            {
                var teachersAndAdmins = await _userRepository
                    .Where(u => u.role == RoleType.TEACHER || u.role == RoleType.ADMIN)
                    .Select(u => new { u.user_id, u.user_name })
                    .ToListAsync();

                ViewBag.user_ID = new SelectList(teachersAndAdmins, "user_id", "user_name");
                return View(model);
            }

            var assignment = await _assignmentRepository.GetByIdAsync(model.assignment_id);
            if (assignment == null)
            {
                return NotFound("Ödev bulunamadı.");
            }

            assignment.title = model.title;
            assignment.description = model.description;
            assignment.dead_line = model.deadline;
            assignment.user_ID = model.user_ID;
            assignment.role = model.role;

            await _assignmentRepository.UpdateAsync(assignment);
            _notyf.Success("Ödev güncellendi!");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var assignment = await _assignmentRepository.GetByIdAsync(id);
                if (assignment == null)
                {
                    TempData["ErrorMessage"] = "Ödev bulunamadı.";
                    return RedirectToAction("Index");
                }
                return View(assignment);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hata: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _assignmentRepository.DeleteAsync(id);
                _notyf.Success("Ödev başarıyla silindi!");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hata: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
