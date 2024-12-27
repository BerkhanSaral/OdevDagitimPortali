using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Models;
using OdevDagitimPortali.Repository;

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


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var submissionModels = await _applicationDbContext.Submissions
                .Include(s => s.Assignment)
                .Include(s => s.User)
                .Include(s => s.file_url) // file_url ilişkisini dahil et
                .ToListAsync();

<<<<<<< HEAD
            if (submissionModels == null || !submissionModels.Any())
            {
                ViewBag.Message = "Henüz bir ödev yüklenmedi.";
                return View(new List<SubmissionModel>()); // Boş model döndür
            }

            var model = submissionModels.Select(s => new SubmissionModel
            {
                submission_id = s.submission_id,
                assignment_ID = s.assignment_ID,
                user_ID = s.user_ID,
                submission_date = s.submission_date,
                FileUploadId = s.FileUploadId,
                file_url = s.file_url,
                Assignment = s.Assignment,
                User = s.User
            }).ToList();

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Download(int id)
        {
            var fileUpload = await _applicationDbContext.FileUploads.FindAsync(id);
            if (fileUpload == null)
            {
                return Json(new { success = false, message = "Dosya bulunamadı." });
            }

            var fileBytes = fileUpload.FileContent;
            var fileName = fileUpload.FileName;

            return File(fileBytes, "application/octet-stream", fileName);
        }
=======
            return View(submissionModels);
        }

        public async Task<IActionResult> DownloadFile(int id)
        {
            var fileUpload = await _applicationDbContext.FileUploads.FindAsync(id);
            if (fileUpload == null)
            {
                return NotFound();
            }

            return File(fileUpload.FileContent, "application/octet-stream", fileUpload.FileName);
        }


>>>>>>> 69414b7d9e73ab87a30fa9f36aa951a195c8c4ae
    }

}
