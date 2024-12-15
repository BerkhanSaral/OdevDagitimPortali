using OdevDagitimPortali.Models;
using OdevDagitimPortali.Models.user;
using System.Diagnostics;

namespace OdevDagitimPortali.Repository
{
    public class SubmissionRepository : GenericRepository<Users>
    {
        public SubmissionRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
