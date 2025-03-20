using ModelLayer.DTO;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        string Register(UserRegisterDTO userDto);
        string Login(UserLoginDTO loginDto);
        void GenerateResetToken(string email); // ✅ Add this
        bool ResetPassword(string email, string newPassword, string resetToken); // ✅ Add this
    }
}
