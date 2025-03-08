using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

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

        public ResponseModel<string> AddGreeting(GreetingModel greeting)
        {
            return _greetingRL.AddGreeting(greeting);
        }

        public GreetingModel GetGreetingById(int id)
        {
            return _greetingRL.GetGreetingById(id);
        }

        public ResponseModel<string> UpdateGreeting(GreetingModel greeting)
        {
            return _greetingRL.UpdateGreeting(greeting);
        }

        public ResponseModel<string> DeleteGreeting(int id)
        {
            return _greetingRL.DeleteGreeting(id);
        }
    }

}