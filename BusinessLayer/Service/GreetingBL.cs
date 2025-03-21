using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System.Collections.Generic;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;
        private readonly IRabbitMQService _rabbitMQService;

        public GreetingBL(IGreetingRL greetingRL, IRabbitMQService rabbitMQService)
        {
            _greetingRL = greetingRL;
            _rabbitMQService = rabbitMQService;
        }

        public List<GreetingModel> GetAllGreetings()
        {
            return _greetingRL.GetAllGreetings();
        }

        public ResponseModel<string> AddGreeting(GreetingModel greeting, int userId)
        {
            var response = _greetingRL.AddGreeting(greeting, userId);
            if (response.Success)
            {
                _rabbitMQService.PublishMessage($"New Greeting Added: {greeting.Message}");
            }
            return response;
        }

        public GreetingModel GetGreetingById(int id)
        {
            return _greetingRL.GetGreetingById(id);
        }

        public ResponseModel<string> UpdateGreetingMessage(GreetingModel greeting, int userId)
        {
            var response = _greetingRL.UpdateGreetingMessage(greeting, userId);
            if (response.Success)
            {
                _rabbitMQService.PublishMessage($"Greeting Updated: {greeting.Message}");
            }
            return response;
        }

        public ResponseModel<string> DeleteGreeting(int id, int userId)
        {
            var response = _greetingRL.DeleteGreeting(id, userId);
            if (response.Success)
            {
                _rabbitMQService.PublishMessage($"Greeting Deleted (ID: {id})");
            }
            return response;
        }
    }
}
