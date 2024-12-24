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

        public async Task<IActionResult> DownloadFile(int id)
        {
            var fileUpload = await _applicationDbContext.FileUploads.FindAsync(id);
            if (fileUpload == null)
            {
                return NotFound();
            }

            return File(fileUpload.FileContent, "application/octet-stream", fileUpload.FileName);
        }


    }

}
