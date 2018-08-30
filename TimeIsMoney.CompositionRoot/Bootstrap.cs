using Microsoft.Extensions.DependencyInjection;
using TimeIsMoney.DataAccess;
using TimeIsMoney.Services.Auth;
using TimeIsMoney.Services.Token;
using TimeIsMoney.Services.TransactionType;

namespace TimeIsMoney.CompositionRoot
{
    public static class Bootstrap
    {
        public static void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            serviceCollection.AddScoped<ITokenService, TokenService>();
            serviceCollection.AddScoped<IAuthService, AuthService>();

            serviceCollection.AddScoped<ITransactionTypeService, TransactionTypeService>();
        }
    }
}