using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Models;
using System.Data;

namespace OdevDagitimPortali.Repository
{
    public class AssignmentRepository: GenericRepository<Assignment>
    {

        public AssignmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Assignment>> GetByUserIdAsync(int userId)
        {
            return await _context.Assignments
                .Where(a => a.user_ID == userId)
                .ToListAsync();
        }
    }
}
