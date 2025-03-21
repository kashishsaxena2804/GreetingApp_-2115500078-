using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using Newtonsoft.Json;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryLayer.Service
{

    public class GreetingRL : IGreetingRL
    {
        private readonly IDatabase _cache;
        private readonly GreetingContext _context;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public GreetingRL(GreetingContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _cache = redis.GetDatabase();
        }

        public List<GreetingModel> GetAllGreetings()
        {
            string cacheKey = "greetings";
            string cachedData = _cache.StringGet(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonConvert.DeserializeObject<List<GreetingModel>>(cachedData);
            }

            var greetings = _context.Greetings.Include(g => g.User).ToList();
            _cache.StringSet(cacheKey, JsonConvert.SerializeObject(greetings), TimeSpan.FromMinutes(10));

            return greetings;
        }


        public ResponseModel<string> AddGreeting(GreetingModel greeting, int userId)
        {
            greeting.UserId = userId;
            _context.Greetings.Add(greeting);
            _context.SaveChanges();

            _cache.KeyDelete("greetings"); // ❌ Clear cache after adding

            return new ResponseModel<string> { Success = true, Message = "Greeting saved successfully!" };
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

            _cache.KeyDelete("greetings"); // ❌ Clear cache after update

            return new ResponseModel<string> { Success = true, Message = "Greeting updated successfully!" };
        }


        public ResponseModel<string> DeleteGreeting(int id, int userId)
        {
            var greeting = _context.Greetings.FirstOrDefault(g => g.Id == id && g.UserId == userId);
            if (greeting == null)
                return new ResponseModel<string> { Success = false, Message = "Greeting not found or unauthorized!" };

            _context.Greetings.Remove(greeting);
            _context.SaveChanges();

            _cache.KeyDelete("greetings"); // ❌ Clear cache after deletion

            return new ResponseModel<string> { Success = true, Message = "Greeting deleted successfully!" };
        }

    }
}
