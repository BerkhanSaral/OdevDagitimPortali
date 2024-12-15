using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Models;
using OdevDagitimPortali.Models.user;
using OdevDagitimPortali.ViewModels;
using System.Data;

namespace OdevDagitimPortali.Repository
{
    public class AssignmentRepository: GenericRepository<Assignment>
    {

        public AssignmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        /*private readonly ApplicationDbContext _context;

        public AssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }



       public List<AssignmentModel> GetList()
        {
            var assignment = _context.Assignments.Select(x => new AssignmentModel()
            {
                assignment_id = x.assignment_id,
                title = x.title,
                description = x.description,
                create_date = x.created_date,
                deadline = x.dead_line,
                role= x.role,
                user_ID = x.user_ID
                
            }).ToList();

            return assignment;
        }

        public AssignmentModel GetById(int id)
        {
            var assignment = _context.Assignments
                .Where(s => s.assignment_id == id)
                .Include(x => x.User)  // İlişkili kullanıcı bilgilerini de dahil et
                .Select(x => new AssignmentModel
                {
                    assignment_id = x.assignment_id,
                    title = x.title,
                    description = x.description,
                    create_date = x.created_date,
                    deadline = x.dead_line,
                    user_ID = x.user_ID,
                    CreatedBy = x.User.user_name // Kullanıcı adını almak için ekledik
                })
                .FirstOrDefault();

            return assignment;
        }

        public void Add(AssignmentModel model)
        {
            var user = _context.User
                .Where(u => u.user_id == model.user_ID)
                .Select(u => new { u.role })
                .FirstOrDefault();

            if (user == null)
            {
                throw new Exception("Seçilen kullanıcı mevcut değil.");
            }

            var assignment = new Assignment()
            {
                title = model.title,
                description = model.description,
                created_date = model.create_date,
                dead_line = model.deadline,
                user_ID = model.user_ID,
                role = user.role // Kullanıcının doğru rolü atanıyor
            };

            _context.Assignments.Add(assignment); // Doğru DbSet kullanılıyor
            _context.SaveChanges();
        }



        public void Update(AssignmentModel model)
        {
            var assignment = _context.Assignments.Where(s => s.assignment_id == model.assignment_id).FirstOrDefault();
            if (assignment != null)
            {
                assignment.title = model.title;
                assignment.description = model.description;
                assignment.created_date = model.create_date;
                assignment.dead_line = model.deadline;
                assignment.user_ID = model.user_ID;

                _context.Assignments.Update(assignment);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var assignment = _context.Assignments.Where(s => s.assignment_id == id).FirstOrDefault();
            if (assignment == null)
            {
                throw new Exception("Silinecek ödev bulunamadı.");
            }
            _context.Assignments.Remove(assignment);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        */

    }
}
