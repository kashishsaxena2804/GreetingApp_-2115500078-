using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;
using BusinessLayer.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult Register([FromBody] UserRegisterDTO userDto)
        {
            var result = _userService.Register(userDto);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO loginDto)
        {
            var result = _userService.Login(loginDto);
            return Ok(new { token = result });
        }

        [HttpPost("forget-password")]
        public IActionResult ForgetPassword([FromBody] string email)
        {
            _userService.ForgetPassword(email);
            return Ok(new { message = "Reset link sent to email" });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDTO resetDto)
        {
            bool result = _userService.ResetPassword(resetDto.Token, resetDto.NewPassword);
            if (!result) return BadRequest(new { message = "Invalid or expired token" });
            return Ok(new { message = "Password reset successful!" });
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(new { message = $"Welcome, {email}!" });
        }
    }
}
