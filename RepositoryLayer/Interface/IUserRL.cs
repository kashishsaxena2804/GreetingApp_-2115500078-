using ModelLayer.Models;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        void Register(User user);
        User GetUserByEmail(string email);
        void GenerateResetToken(string email);
        bool ResetPassword(string email, string newPassword, string resetToken);
        void UpdateUser(User user);

    }
}
