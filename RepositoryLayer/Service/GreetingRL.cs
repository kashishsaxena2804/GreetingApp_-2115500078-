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

        public ResponseModel<string> DeleteGreetingMessage(int id)
        {
            try
            {
                var greeting = _context.Greetings.Find(id);
                if (greeting == null)
                {
                    logger.Warn($"Delete failed: Greeting with ID {id} not found.");
                    return new ResponseModel<string> { Success = false, Message = "Greeting not found!" };
                }

                _context.Greetings.Remove(greeting);
                _context.SaveChanges();

                logger.Info($"Greeting with ID {id} deleted successfully.");
                return new ResponseModel<string> { Success = true, Message = "Greeting deleted successfully!" };
            }
            catch (Exception ex)
            {
                logger.Error($"Error deleting greeting: {ex.Message}");
                throw;
            }
        }
        public ResponseModel<string> UpdateGreetingMessage(GreetingModel greeting)
        {
            var existingGreeting = _context.Greetings.Find(greeting.Id);
            if (existingGreeting == null)
                return new ResponseModel<string> { Success = false, Message = "Greeting not found!" };

            existingGreeting.Message = greeting.Message;
            _context.SaveChanges();

            return new ResponseModel<string> { Success = true, Message = "Greeting updated successfully!", Data = existingGreeting.Message };
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

        public List<GreetingModel> GetAllGreetings()  // Added method to list all greetings
        {
            return _context.Greetings.ToList();
        }
    }
}
