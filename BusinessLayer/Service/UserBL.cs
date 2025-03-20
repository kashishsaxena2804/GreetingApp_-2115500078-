using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BusinessLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTO;
using ModelLayer.Models;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailBL _emailService;

        public UserBL(IUserRL userRepository, IConfiguration configuration, IEmailBL emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
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
                return GenerateJwtToken(user);

            return "Invalid credentials!";
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
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

        public bool ResetPassword(string token, string newPassword)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            try
            {
                var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var email = claims.Identity.Name;
                var user = _userRepository.GetUserByEmail(email);
                if (user == null) return false;

                string salt = GenerateSalt();
                string hashedPassword = HashPassword(newPassword, salt);

                user.PasswordHash = hashedPassword;
                user.Salt = salt;

                _userRepository.UpdateUser(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ForgetPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");

            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
                throw new Exception("User not found");

            // Generate JWT token
            string token = GenerateJwtToken(user);

            // Email body with just the token
            string emailBody = $"Your password reset token is: {token}";

            try
            {
                _emailService.SendEmail(email, "Password Reset Token", emailBody);
                Console.WriteLine($"Password reset email sent to: {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

    }
}
