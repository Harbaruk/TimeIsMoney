using System;
using TimeIsMoney.Services.Auth.Models;
using TimeIsMoney.Services.Token.Models;

namespace TimeIsMoney.Services.Token
{
    public interface ITokenService
    {
        TokenModel GetToken(LoginCredentialsModel loginCredentials);

        TokenModel GetRefreshToken(RefreshTokenModel refreshToken);

        TokenModel GetTokenForConfirmedUser(Guid guid);
    }
}