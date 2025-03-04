using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private string _greetingMessage = "Hello, Welcome!";

        public ResponseModel<string> GetGreeting()
        {
            return new ResponseModel<string> { Data = _greetingMessage, Message = "Greeting fetched successfully!" };
        }

        public ResponseModel<string> UpdateGreeting(string message)
        {
            _greetingMessage = message;
            return new ResponseModel<string> { Data = _greetingMessage, Message = "Greeting updated successfully!" };
        }

        public ResponseModel<string> PatchGreeting(string message)
        {
            _greetingMessage += " " + message;
            return new ResponseModel<string> { Data = _greetingMessage, Message = "Greeting patched successfully!" };
        }

        public ResponseModel<string> ResetGreeting()
        {
            _greetingMessage = "Hello, Welcome!";
            return new ResponseModel<string> { Data = _greetingMessage, Message = "Greeting reset to default!" };
        }
    }
}
