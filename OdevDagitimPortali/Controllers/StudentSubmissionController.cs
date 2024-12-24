using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Models;
using OdevDagitimPortali.Models.user;
using OdevDagitimPortali.Repository;
using OdevDagitimPortali.ViewModels;
using System.Diagnostics;

namespace OdevDagitimPortali.Controllers
{
    public class StudentSubmissionController : Controller
    {

        private readonly SubmissionRepository _submissionRepository;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserRepository _userRepository;
        private readonly INotyfService _notyf;

        public StudentSubmissionController(SubmissionRepository submissionRepository, ApplicationDbContext applicationDbContext, INotyfService notyf, UserRepository userRepository)
        {
            _submissionRepository = submissionRepository;
            _applicationDbContext = applicationDbContext;
            _notyf = notyf;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(int userID)
        {
            if (userID <= 0)
            {
                _notyf.Error("Geçersiz kullanıcı numarası.");
                return View(new List<SubmissionModel>()); // Boş model döndür
            }

            // Veritabanından Submissions bilgilerini çekip SubmissionModel'e dönüştür
            var submissions = await _applicationDbContext.Submissions
                .Where(s => s.user_ID == userID)
                .Include(s => s.Assignment) // Ödev bilgilerini dahil et
                .Select(s => new SubmissionModel
                {
                    submission_id = s.submission_id,
                    assignment_ID = s.assignment_ID,
                    user_ID = s.user_ID,
                    submission_date = s.submission_date,
                    Assignment = s.Assignment,
                    file_url = s.file_url
                })
                .ToListAsync();

            return View(submissions);
        }


        public IActionResult Add()
        {
            // Ödev listesi
            var assignments = _applicationDbContext.Assignments
                .Select(a => new { a.assignment_id, a.title })
                .ToList();

            ViewBag.Assignments = new SelectList(assignments, "assignment_id", "title");

            // Öğrenci listesi
            /* var students = _applicationDbContext.User
                 .Where(u => u.role == RoleType.STUDENT)
                 .Select(u => new { u.user_id, u.user_name })
                 .ToList();

             ViewBag.Students = new SelectList(students, "user_id", "user_name");*/

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(SubmissionModel model)
        {
            // Ödev listesini tekrar doldur
            ViewBag.Assignments = new SelectList(_applicationDbContext.Assignments, "assignment_id", "title");


            try
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var value = ModelState[modelStateKey];
                    foreach (var error in value.Errors)
                    {
                        Debug.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }

                // Öğrencinin geçerli olup olmadığını kontrol et
                var studentExists = _applicationDbContext.User
                    .Any(u => u.user_id == model.user_ID && u.role == RoleType.STUDENT);

                if (!studentExists)
                {
                    ModelState.AddModelError("user_ID", "Geçersiz öğrenci ID veya öğrenci değil.");
                    return View(model);
                }

                // Tarihi burada ata (View'den gelmesini önlemek için)
                model.submission_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Dosya yükleme kontrolü
                if (model.UploadedFile != null && model.UploadedFile.Length > 0)
                {
                    // Dosya yükle ve veritabanına kaydet
                    var fileUpload = new FileUpload
                    {
                        FileName = model.UploadedFile.FileName,
                        FileContent = GetFileBytes(model.UploadedFile),
                        UploadDate = DateTime.Now,
                        user_ID = model.user_ID
                    };

                    _applicationDbContext.FileUploads.Add(fileUpload);
                    await _applicationDbContext.SaveChangesAsync();

                    // Submission oluştur ve veritabanına ekle
                    var submission = new Submission
                    {
                        assignment_ID = model.assignment_ID,
                        user_ID = model.user_ID,
                        submission_date = model.submission_date,
                        FileUploadId = fileUpload.Id
                    };

                    _applicationDbContext.Submissions.Add(submission);
                    await _applicationDbContext.SaveChangesAsync();

                    _notyf.Success("Öğrenci ödevi başarıyla eklendi.");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("UploadedFile", "Lütfen bir dosya yükleyin.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Beklenmeyen bir hata oluştu: {ex.Message}");
            }


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



        /*
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
        */
    }
}
