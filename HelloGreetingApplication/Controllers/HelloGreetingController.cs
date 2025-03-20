using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;
using System.Security.Claims;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }

        // ✅ Get all greetings
        [HttpGet]
        [Authorize]
        public IActionResult GetAllGreetings()
        {
            var greetings = _greetingBL.GetAllGreetings();
            return Ok(greetings);
        }

        // ✅ Get greeting by ID
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetGreetingById(int id)
        {
            logger.Info($"Fetching greeting with ID: {id}");
            var response = _greetingBL.GetGreetingById(id);

            if (response == null)
                return NotFound(new ResponseModel<string> { Success = false, Message = "Greeting not found" });

            return Ok(new ResponseModel<string> { Success = true, Data = response.Message, Message = "Greeting retrieved successfully" });
        }

        // ✅ Add new greeting (Requires user authentication)
        [HttpPost("add")]
        [Authorize]
        public IActionResult AddGreeting([FromBody] GreetingModel greeting)
        {
            if (greeting == null || string.IsNullOrWhiteSpace(greeting.Message))
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Invalid greeting data" });

            int userId = GetUserIdFromToken();
            logger.Info($"Adding new greeting for User ID: {userId}");

            var response = _greetingBL.AddGreeting(greeting, userId);
            return Ok(response);
        }

        // ✅ Update existing greeting (Requires user authentication)
        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateGreeting([FromBody] GreetingModel greeting)
        {
            if (greeting == null || greeting.Id <= 0)
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Invalid greeting data" });

            int userId = GetUserIdFromToken();
            logger.Info($"Updating greeting ID: {greeting.Id} for User ID: {userId}");

            var response = _greetingBL.UpdateGreetingMessage(greeting, userId);

            return response.Success ? Ok(response) : NotFound(response);
        }

        // ✅ Delete greeting by ID (Requires user authentication)
        [HttpDelete("delete/{id}")]
        [Authorize]
        public IActionResult DeleteGreeting(int id)
        {
            int userId = GetUserIdFromToken();
            logger.Info($"Deleting greeting ID: {id} for User ID: {userId}");

            var response = _greetingBL.DeleteGreeting(id, userId);

            return response.Success ? Ok(response) : NotFound(response);
        }

        // ✅ Extract User ID from JWT Token
        private int GetUserIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            return 0; // Return 0 if user ID is not found (should never happen if authentication is working)
        }
    }
}
