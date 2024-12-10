using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Models;
using OdevDagitimPortali.Models.user;
using OdevDagitimPortali.Repository;

namespace OdevDagitimPortali.Controllers
{
    public class SubmissionController : Controller
    {
        private readonly SubmissionRepository _submissionRepository;
        private readonly ApplicationDbContext _applicationDbContext;


        public SubmissionController(SubmissionRepository submissionRepository, ApplicationDbContext applicationDbContext)
        {
            _submissionRepository = submissionRepository;
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            var submission = _submissionRepository.GetList();

            return View(submission);
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
        public IActionResult Add(SubmissionModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcının öğrenci olup olmadığını kontrol et
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
                    // Dosyayı `FileUpload` nesnesine dönüştür
                    var fileUpload = new FileUpload
                    {
                        FileName = model.UploadedFile.FileName,
                        FileContent = GetFileBytes(model.UploadedFile),
                        UploadDate = DateTime.Now,
                        user_ID = model.user_ID
                    };

                    // Dosyayı veritabanına kaydet
                    _applicationDbContext.FileUploads.Add(fileUpload);
                    _applicationDbContext.SaveChanges();

                    // `Submission` nesnesini oluştur
                    var submission = new Submission
                    {
                        assignment_ID = model.assignment_ID,
                        user_ID = model.user_ID,
                        submission_date = model.submission_date,
                        FileUploadId = fileUpload.Id
                    };

                    // Submission'ı veritabanına kaydet
                    _applicationDbContext.Submissions.Add(submission);
                    _applicationDbContext.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("UploadedFile", "Dosya yüklenmesi gereklidir.");
                }
            }

            // Hata durumunda View verilerini yeniden doldur
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



        public IActionResult Update(int id)
        {
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RoleType))); // Enum'dan rolleri alıyoruz

            var submission = _submissionRepository.GetById(id);
            ViewBag.Assignments = new SelectList(_applicationDbContext.Assignments, "assignment_id", "title");

            return View();
        }

        [HttpPost]
        public IActionResult Update(SubmissionModel model)
        {

            ViewBag.Assignments = new SelectList(_applicationDbContext.Assignments, "assignment_id", "title");



            // Mevcut Submission'ı veritabanından getir
            var existingSubmission = _submissionRepository.GetById(model.submission_id);


            return View();
        }

        public IActionResult Delete(int id)
        {
            var submission = _submissionRepository.GetById(id);
            return View(submission);
        }

        [HttpPost]
        public IActionResult Delete(SubmissionModel model)
        {


            _submissionRepository.Delete(model.assignment_ID);
            return RedirectToAction("Index");
        }
    }
}
