using ModelLayer.Model;
using System.Collections.Generic;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        List<GreetingModel> GetAllGreetings();
        ResponseModel<string> AddGreeting(GreetingModel greeting, int userId);
        GreetingModel GetGreetingById(int id);
        ResponseModel<string> UpdateGreetingMessage(GreetingModel greeting, int userId);
        ResponseModel<string> DeleteGreeting(int id, int userId);
    }
}
