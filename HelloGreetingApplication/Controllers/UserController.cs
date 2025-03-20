using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AddressBookSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userService;

        public UserController(IUserBL userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterDTO userDto)
        {
            var result = _userService.Register(userDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO loginDto)
        {
            var result = _userService.Login(loginDto);
            return Ok(result);
        }

        [HttpPost("forget-password")]
        public IActionResult ForgetPassword(string email)
        {
            _userService.GenerateResetToken(email);  // ✅ Corrected variable name
            return Ok("Reset link sent to email (token generated)");
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(string email, string newPassword, string resetToken)
        {
            bool result = _userService.ResetPassword(email, newPassword, resetToken);  // ✅ Corrected variable name
            if (!result) return BadRequest("Invalid token or expired");
            return Ok("Password reset successful!");
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok($"Welcome, {email}!");
        }

    }
}
