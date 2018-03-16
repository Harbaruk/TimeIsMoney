using TimeIsMoney.Services.Password.Models;

namespace TimeIsMoney.Services.Password
{
    public interface IPasswordService
    {
        void ChangePassword(ChangePasswordModel changePassword);

        void ForgotPassword(ForgotPasswordModel forgotPassword);

        void SetPassword(SetPasswordModel setPassword)
    }
}