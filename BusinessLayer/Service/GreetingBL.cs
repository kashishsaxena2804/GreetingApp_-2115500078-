using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using System.Collections.Generic;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }

        public List<GreetingModel> GetAllGreetings()
        {
            return _greetingRL.GetAllGreetings();
        }

        public ResponseModel<string> AddGreeting(GreetingModel greeting, int userId)
        {
            return _greetingRL.AddGreeting(greeting, userId);
        }

        public GreetingModel GetGreetingById(int id)
        {
            return _greetingRL.GetGreetingById(id);
        }

        public ResponseModel<string> UpdateGreetingMessage(GreetingModel greeting, int userId)
        {
            return _greetingRL.UpdateGreetingMessage(greeting, userId);
        }

        public ResponseModel<string> DeleteGreeting(int id, int userId)
        {
            return _greetingRL.DeleteGreeting(id, userId);
        }
    }
}
