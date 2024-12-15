using OdevDagitimPortali.Models.user;

namespace OdevDagitimPortali.Repository
{
    public class UserRepository : GenericRepository<Users>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
