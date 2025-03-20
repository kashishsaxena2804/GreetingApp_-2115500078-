using ModelLayer.DTO;

namespace BusinessLayer.Interfaces
{
    public interface IUserBL
    {
        string Register(UserRegisterDTO userDto);
        string Login(UserLoginDTO loginDto);
        void ForgetPassword(string email);
        bool ResetPassword(string token, string newPassword);
    }
}
