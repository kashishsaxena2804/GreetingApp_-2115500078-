using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;

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

        [HttpGet("greet")]
        public IActionResult GetGreeting()
        {
            logger.Info("GreetingController: GET request received.");
            var message = _greetingBL.GetGreeting();
            return Ok(message);
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting fetched successfully",
                Data = "Hello, World!"
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] RequestModel requestModel)
        {
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting added successfully",
                Data = $"Key: {requestModel.Key}, Value: {requestModel.Value}"
            });
        }

        [HttpPut]
        public IActionResult Put([FromBody] RequestModel requestModel)
        {
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting updated successfully",
                Data = $"Key: {requestModel.Key}, New Value: {requestModel.Value}"
            });
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] RequestModel requestModel)
        {
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting modified successfully",
                Data = $"Key: {requestModel.Key}, Updated Value: {requestModel.Value}"
            });
        }

        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting deleted successfully",
                Data = $"Key: {key} removed"
            });
        }
    }
}
