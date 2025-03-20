using System;
using System.Security.Cryptography;
using System.Text;
using BusinessLayer.Interface;
using ModelLayer.DTO;
using ModelLayer.Models;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRepository;

        public UserBL(IUserRL userRepository)
        {
            _userRepository = userRepository;
        }

        public string Register(UserRegisterDTO userDto)
        {
            string salt = GenerateSalt();
            string hashedPassword = HashPassword(userDto.Password, salt);

            User newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            _userRepository.Register(newUser);
            return "User registered successfully!";
        }

        public string Login(UserLoginDTO loginDto)
        {
            var user = _userRepository.GetUserByEmail(loginDto.Email);
            if (user == null)
                return "User not found";

            string hashedInputPassword = HashPassword(loginDto.Password, user.Salt);
            if (hashedInputPassword == user.PasswordHash)
                return "Login successful!"; // Ideally, return a JWT token

            return "Invalid credentials!";
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes); // Updated for .NET 6+
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public void GenerateResetToken(string email)
        {
            _userRepository.GenerateResetToken(email);
        }

        public bool ResetPassword(string email, string newPassword, string resetToken)
        {
            return _userRepository.ResetPassword(email, newPassword, resetToken);
        }

    }
}
