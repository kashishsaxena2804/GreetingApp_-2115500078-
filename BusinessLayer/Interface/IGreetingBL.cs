using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        ResponseModel<string> AddGreeting(GreetingModel greeting);
        GreetingModel GetGreetingById(int id);
        ResponseModel<string> UpdateGreeting(GreetingModel greeting);
        ResponseModel<string> DeleteGreeting(int id);
    }



}
