using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        ResponseModel<string> GetGreeting();
        ResponseModel<string> UpdateGreeting(string message);
        ResponseModel<string> PatchGreeting(string message);
        ResponseModel<string> ResetGreeting();
    }
}
