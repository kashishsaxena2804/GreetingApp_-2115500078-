using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ModelLayer.Models;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly GreetingContext _context;

        public UserRL(GreetingContext context)
        {
            _context = context;
        }

        // ✅ Register User
        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            Console.WriteLine($"User Registered: {user.Email}");
        }

        // ✅ Get User by Email
        public User GetUserByEmail(string email)
        {
            Console.WriteLine($"Fetching user by email: {email}");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                Console.WriteLine("User NOT FOUND!");
            else
                Console.WriteLine($"User FOUND: {user.Email}");

            return user;
        }

        // ✅ Generate Reset Token
        public void GenerateResetToken(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) throw new Exception("User not found");

            user.ResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.Now.AddMinutes(15);

            _context.SaveChanges();
            Console.WriteLine($"Reset Token: {user.ResetToken}");
        }

        // ✅ Reset Password
        public bool ResetPassword(string email, string newPassword, string resetToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || user.ResetToken != resetToken || user.ResetTokenExpiry < DateTime.Now)
                return false;

            string salt = GenerateSalt();
            string hashedPassword = HashPassword(newPassword, salt);

            user.PasswordHash = hashedPassword;
            user.Salt = salt;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            _context.SaveChanges();
            return true;
        }

        // ✅ Generate Salt
        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        // ✅ Hash Password
        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
