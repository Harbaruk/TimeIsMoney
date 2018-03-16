using TimeIsMoney.Services.Enums;

namespace TimeIsMoney.Services.ProviderAbstraction
{
    public interface IAuthenticaterUserProvider
    {
        bool IsAuthenticated { get; }
        string Name { get; }
        int UserId { get; }
        string Email { get; }
        Role UserType { get; }
    }
}