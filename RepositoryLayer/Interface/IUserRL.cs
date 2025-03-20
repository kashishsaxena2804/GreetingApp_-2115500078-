using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Models;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        void Register(User user);
        User GetUserByEmail(string email);
        void GenerateResetToken(string email);
        bool ResetPassword(string email, string newPassword, string resetToken);

    }
}
