using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Models;
using OdevDagitimPortali.Models.user;
using OdevDagitimPortali.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace OdevDagitimPortali.Controllers
{
    public class SubmissionController : Controller
    {
        private readonly SubmissionRepository _submissionRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly INotyfService _notyf;

        public SubmissionController(SubmissionRepository submissionRepository, ApplicationDbContext applicationDbContext, INotyfService notyf)
        {
            _submissionRepository = submissionRepository;
            _applicationDbContext = applicationDbContext;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var submissionModels = await _applicationDbContext.Submissions
                .Include(s => s.Assignment)  // İlişkili Assignment'ı dahil et
                .Include(s => s.User)        // İlişkili User'ı dahil et
                .Select(s => new SubmissionModel
                {
                    submission_id = s.submission_id,
                    assignment_ID = s.assignment_ID,
                    user_ID = s.user_ID,
                    submission_date = s.submission_date,
                    // İlişkili nesneleri alıyoruz
                    Assignment = s.Assignment,
                    User = s.User
                }).ToListAsync();

            return View(submissionModels);
        }


        public IActionResult Add()
        {
            // Ödev listesi
            var assignments = _applicationDbContext.Assignments
                .Select(a => new { a.assignment_id, a.title })
                .ToList();

            ViewBag.Assignments = new SelectList(assignments, "assignment_id", "title");

            // Öğrenci listesi
            var students = _applicationDbContext.User
                .Where(u => u.role == RoleType.STUDENT)
                .Select(u => new { u.user_id, u.user_name })
                .ToList();

            ViewBag.Students = new SelectList(students, "user_id", "user_name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(SubmissionModel model)
        {
            if (ModelState.IsValid)
            {
                // Öğrencinin geçerli olup olmadığını kontrol et
                var studentExists = _applicationDbContext.User
                    .Any(u => u.user_id == model.user_ID && u.role == RoleType.STUDENT);

                if (!studentExists)
                {
                    ModelState.AddModelError("user_ID", "Geçersiz öğrenci ID veya öğrenci değil.");
                    ViewBag.Assignments = new SelectList(_applicationDbContext.Assignments, "assignment_id", "title");
                    ViewBag.Students = new SelectList(_applicationDbContext.User
                        .Where(u => u.role == RoleType.STUDENT)
                        .Select(u => new { u.user_id, u.user_name })
                        .ToList(), "user_id", "user_name");
                    return View(model);
                }

                // Dosya yüklenmiş mi kontrol et
                if (model.UploadedFile != null && model.UploadedFile.Length > 0)
                {
                    // Dosya nesnesini oluştur
                    var fileUpload = new FileUpload
                    {
                        FileName = model.UploadedFile.FileName,
                        FileContent = GetFileBytes(model.UploadedFile),
                        UploadDate = DateTime.Now,
                        user_ID = model.user_ID
                    };

                    // Dosyayı veritabanına kaydet
                    _applicationDbContext.FileUploads.Add(fileUpload);
                    await _applicationDbContext.SaveChangesAsync();

                    // Submission nesnesi oluştur
                    var submission = new Submission
                    {
                        assignment_ID = model.assignment_ID,
                        user_ID = model.user_ID,
                        submission_date = model.submission_date,
                        FileUploadId = fileUpload.Id
                    };

                    // Submission'ı veritabanına kaydet
                    await _applicationDbContext.Submissions.AddAsync(submission);
                    _notyf.Success("Öğrenci ödevi ekledi.");
                    await _applicationDbContext.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("UploadedFile", "Dosya yüklenmesi gereklidir.");
                }
            }

            // Hata durumunda tekrar ödev ve öğrenci listelerini View'a gönder
            ViewBag.Assignments = new SelectList(_applicationDbContext.Assignments, "assignment_id", "title");
            ViewBag.Students = new SelectList(_applicationDbContext.User
                .Where(u => u.role == RoleType.STUDENT)
                .Select(u => new { u.user_id, u.user_name })
                .ToList(), "user_id", "user_name");

            return View(model);
        }

        private byte[] GetFileBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            // Submission'ı al
            var submission = await _submissionRepository.GetByIdAsync(id);

            // Ödev listesi
            ViewBag.Assignments = new SelectList(_applicationDbContext.Assignments, "assignment_id", "title");

            return View(submission); // Burada submission modelini gönderiyoruz
        }

        [HttpPost]
        public async Task<IActionResult> Update(SubmissionModel model)
        {
            ViewBag.Assignments = new SelectList(_applicationDbContext.Assignments, "assignment_id", "title");

            // Mevcut Submission'ı veritabanından al
            var existingSubmission = await _submissionRepository.GetByIdAsync(model.submission_id);

            // Güncellenmiş submission'ı kaydet
            // Burada işlemleri tamamlayın

            _notyf.Success("Öğrenci ödevi güncellendi.");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id);
            return View(submission);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SubmissionModel model)
        {
            await _submissionRepository.DeleteAsync(model.assignment_ID);
            _notyf.Success("Öğrenci ödevi silindi.");
            return RedirectToAction("Index");
        }
    }
}
