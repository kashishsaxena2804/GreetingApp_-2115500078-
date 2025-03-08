using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }

        // ✅ GET: Retrieve Greeting by ID
        [HttpGet("{id}")]
        public IActionResult GetGreetingById(int id)
        {
            logger.Info($"Fetching greeting with ID: {id}");
            var response = _greetingBL.GetGreetingById(id);

            if (response == null)
                return NotFound(new ResponseModel<string> { Success = false, Message = "Greeting not found" });

            return Ok(new ResponseModel<string> { Success = true, Data = response.Message, Message = "Greeting retrieved successfully" });
        }

        // ✅ POST: Add Greeting to Database
        [HttpPost("add")]
        public IActionResult AddGreeting([FromBody] GreetingModel greeting)
        {
            if (greeting == null || string.IsNullOrWhiteSpace(greeting.Message))
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Invalid greeting data" });

            logger.Info("Adding new greeting...");
            var response = _greetingBL.AddGreeting(greeting);
            return Ok(response);
        }

        // ✅ PUT: Update an Existing Greeting
        [HttpPut("update")]
        public IActionResult UpdateGreeting([FromBody] GreetingModel greeting)
        {
            if (greeting == null || greeting.Id <= 0)
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Invalid greeting data" });

            logger.Info($"Updating greeting ID: {greeting.Id}");
            var response = _greetingBL.UpdateGreeting(greeting);

            if (response == null)
                return NotFound(new ResponseModel<string> { Success = false, Message = "Greeting not found" });

            return Ok(response);
        }

        // ✅ DELETE: Remove Greeting by ID
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteGreeting(int id)
        {
            logger.Info($"Deleting greeting with ID: {id}");
            var response = _greetingBL.DeleteGreeting(id);

            if (response == null)
                return NotFound(new ResponseModel<string> { Success = false, Message = "Greeting not found" });

            return Ok(response);
        }
    }
}
