using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Models;
using OdevDagitimPortali.Models.user;
using OdevDagitimPortali.ViewModels;
using System.Linq;
using System.Linq.Expressions;

namespace OdevDagitimPortali.Repository
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<UserModel> GetList()
        {
            var user = _context.User.Select(x => new UserModel()
            {
                user_id = x.user_id,
                fullname = x.fullname,
                user_name = x.user_name,
                email = x.email,
                password = x.password,
                role=x.role
            }).ToList();

            return user ?? new List<UserModel>(); // Eğer null ise boş liste döndürüyoruz
        }

        public UserModel GetById(int id)
        {
            var user = _context.User.Where(s => s.user_id == id).Select(x => new UserModel()
            {
                user_id = x.user_id,
                fullname = x.fullname,
                user_name = x.user_name,
                email = x.email,
                password = x.password,
                role=x.role
            }).FirstOrDefault();

            return user;
        }

        public void Add(UserModel model)
        {
            var user = new Users()
            {
                fullname = model.fullname,
                user_name = model.user_name,
                email = model.email,
                password = model.password,
                role = model.role
            };
            _context.User.Add(user);
            _context.SaveChanges();
        }

        public void Update(UserModel model)
        {
            var user = _context.User.Where(s => s.user_id == model.user_id).FirstOrDefault();
            if (user != null)
            {
                user.fullname = model.fullname;
                user.user_name= model.user_name;
                user.email= model.email;
                user.password= model.password;
                user.role = model.role;
            }

            _context.User.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.User.Where(s => s.user_id == id).FirstOrDefault();
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

        public async Task<Users> AuthenticateUserAsync(string userName, string password)
        {
            // Kullanıcıyı veritabanında bul
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.user_name == userName);

            if (user == null)
            {
                return null; // Kullanıcı bulunamazsa null döndür
            }

            // Şifreyi doğrula (PasswordHasher ile)
            var passwordHasher = new PasswordHasher<Users>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.password, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return null; // Şifre yanlışsa null döndür
            }

            return user; // Kullanıcı doğrulandıysa, kullanıcıyı döndür
        }
    }
}
