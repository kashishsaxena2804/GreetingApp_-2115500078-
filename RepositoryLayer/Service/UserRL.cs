using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Models;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly GreetingContext _context;
        private readonly string secretKey = "YourSuperSecureLongSecretKey123!"; // Replace with a secure key from config

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
            Console.WriteLine(user == null ? "User NOT FOUND!" : $"User FOUND: {user.Email}");
            return user;
        }

        // ✅ Generate Reset Token (Fixed - Using URL-safe Base64 OR JWT)
        public void GenerateResetToken(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) throw new Exception("User not found");

            // 🔹 Generate JWT-based Reset Token (Recommended)
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            _context.SaveChanges();
            Console.WriteLine($"Reset Token Generated: {user.ResetToken}");
        }

        // ✅ Reset Password (Fixed Validation)
        public bool ResetPassword(string email, string newPassword, string resetToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || string.IsNullOrEmpty(user.ResetToken) || user.ResetToken.Trim() != resetToken.Trim() || user.ResetTokenExpiry < DateTime.UtcNow)
                return false;

            // 🔹 Generate Salt & Hash Password
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
            RandomNumberGenerator.Fill(saltBytes);
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

        // ✅ Update User
        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
