using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System.Linq;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private readonly GreetingContext _context;

        public GreetingRL(GreetingContext context)
        {
            _context = context;
        }

        public ResponseModel<string> AddGreeting(GreetingModel greeting)
        {
            try
            {
                _context.Greetings.Add(greeting);
                _context.SaveChanges();
                return new ResponseModel<string> { Success = true, Message = "Greeting saved successfully!", Data = greeting.Message };
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
                throw;
            }
        }


        public GreetingModel GetGreetingById(int id)
        {
            return _context.Greetings.Find(id);
        }

        public ResponseModel<string> UpdateGreeting(GreetingModel greeting)
        {
            var existingGreeting = _context.Greetings.Find(greeting.Id);
            if (existingGreeting == null)
                return new ResponseModel<string> { Success = false, Message = "Greeting not found!" };

            existingGreeting.Message = greeting.Message;
            _context.SaveChanges();

            return new ResponseModel<string> { Success = true, Message = "Greeting updated successfully!", Data = existingGreeting.Message };
        }

        public ResponseModel<string> DeleteGreeting(int id)
        {
            var greeting = _context.Greetings.Find(id);
            if (greeting == null)
                return new ResponseModel<string> { Success = false, Message = "Greeting not found!" };

            _context.Greetings.Remove(greeting);
            _context.SaveChanges();

            return new ResponseModel<string> { Success = true, Message = "Greeting deleted successfully!" };
        }
    }

}
