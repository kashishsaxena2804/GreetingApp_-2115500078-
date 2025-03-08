using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        ResponseModel<string> AddGreeting(GreetingModel greeting);  // Updated return type
        GreetingModel GetGreetingById(int id);
        ResponseModel<string> UpdateGreeting(GreetingModel greeting);
        ResponseModel<string> DeleteGreeting(int id);
    }
}
