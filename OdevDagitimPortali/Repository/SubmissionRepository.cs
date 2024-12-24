using OdevDagitimPortali.Models;
using OdevDagitimPortali.Models.user;
using System.Diagnostics;

namespace OdevDagitimPortali.Repository
{
    public class SubmissionRepository : GenericRepository<Submission>
    {
        public SubmissionRepository(ApplicationDbContext context) : base(context)  // OdevDagitimDbContext kullanılıyor
        {
        }
    }
}
