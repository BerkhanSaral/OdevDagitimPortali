using OdevDagitimPortali.Models;
using System.Diagnostics;

namespace OdevDagitimPortali.Repository
{
    public class SubmissionRepository
    {
        private readonly ApplicationDbContext _context;
        public SubmissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SubmissionModel> GetList()
        {
            var submission = _context.Submissions.Select(x => new SubmissionModel()
            {
                submission_id = x.submission_id,
                assignment_ID = x.assignment_ID,
                user_ID = x.user_ID,
                submission_date = x.submission_date,
                
            }).ToList();

            return submission;
        }

        public SubmissionModel GetById(int id)
        {
            var submission = _context.Submissions.Where(s => s.submission_id == id).Select(x => new SubmissionModel()
            {
                submission_id = x.submission_id,
                assignment_ID = x.assignment_ID,
                user_ID = x.user_ID,
                submission_date = x.submission_date,
            }).FirstOrDefault();

            return submission;
        }

        public void Add(SubmissionModel model)
        {
            var submission = new Submission()
            {
                assignment_ID = model.assignment_ID,
                user_ID = model.user_ID,
                submission_date = model.submission_date,
                file_url = model.file_url
            };
            _context.Submissions.Add(submission);
            _context.SaveChanges();
        }

        public void Update(SubmissionModel model)
        {
            try
            {
                var submission = _context.Submissions.FirstOrDefault(s => s.submission_id == model.submission_id);
                if (submission != null)
                {
                    submission.assignment_ID = model.assignment_ID;
                    submission.user_ID = model.user_ID;

                    Debug.WriteLine($"Submission güncelleniyor: {submission.submission_id}");
                    _context.SaveChanges();
                    Debug.WriteLine("Submission başarıyla güncellendi.");
                }
                else
                {
                    Debug.WriteLine("Submission bulunamadı!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Hata oluştu: " + ex.Message);
            }
        }


        public void Delete(int id)
        {
            var submission = _context.Submissions.Where(s=>s.submission_id == id).FirstOrDefault();
            if (submission != null)
            {
                _context.Submissions.Remove(submission);
                _context.SaveChanges();
            }
        }

    }
}
