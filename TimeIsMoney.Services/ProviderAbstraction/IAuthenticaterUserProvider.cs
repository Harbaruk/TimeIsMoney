using System;
using TimeIsMoney.Services.Enums;

namespace TimeIsMoney.Services.ProviderAbstraction
{
    public interface IAuthenticaterUserProvider
    {
        bool IsAuthenticated { get; }
        string Name { get; }
        Guid UserId { get; }
        string Email { get; }
        Role UserType { get; }
    }
}