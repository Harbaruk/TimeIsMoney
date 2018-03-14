using TimeIsMoney.Services.Auth.Models;

namespace TimeIsMoney.Services.Auth
{
    public interface IAuthService
    {
        void Register(UserRegistrationModel model);
    }
}