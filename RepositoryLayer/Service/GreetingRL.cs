using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private readonly GreetingContext _context;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public GreetingRL(GreetingContext context)
        {
            _context = context;
        }

        public List<GreetingModel> GetAllGreetings()
        {
            return _context.Greetings.Include(g => g.User).ToList();
        }

        public ResponseModel<string> AddGreeting(GreetingModel greeting, int userId)
        {
            try
            {
                greeting.UserId = userId; // ✅ Assign User ID
                _context.Greetings.Add(greeting);
                _context.SaveChanges();

                return new ResponseModel<string> { Success = true, Message = "Greeting saved successfully!" };
            }
            catch (DbUpdateException ex)
            {
                logger.Error($"Error adding greeting: {ex.InnerException?.Message}");
                throw;
            }
        }

        public GreetingModel GetGreetingById(int id)
        {
            return _context.Greetings.Include(g => g.User).FirstOrDefault(g => g.Id == id);
        }

        public ResponseModel<string> UpdateGreetingMessage(GreetingModel greeting, int userId)
        {
            var existingGreeting = _context.Greetings.FirstOrDefault(g => g.Id == greeting.Id && g.UserId == userId);
            if (existingGreeting == null)
                return new ResponseModel<string> { Success = false, Message = "Greeting not found or unauthorized!" };

            existingGreeting.Message = greeting.Message;
            _context.SaveChanges();

            return new ResponseModel<string> { Success = true, Message = "Greeting updated successfully!" };
        }

        public ResponseModel<string> DeleteGreeting(int id, int userId)
        {
            var greeting = _context.Greetings.FirstOrDefault(g => g.Id == id && g.UserId == userId);
            if (greeting == null)
                return new ResponseModel<string> { Success = false, Message = "Greeting not found or unauthorized!" };

            _context.Greetings.Remove(greeting);
            _context.SaveChanges();

            return new ResponseModel<string> { Success = true, Message = "Greeting deleted successfully!" };
        }
    }
}
