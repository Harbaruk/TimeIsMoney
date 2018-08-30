using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TimeIsMoney.Common;
using TimeIsMoney.DataAccess;
using TimeIsMoney.DataAccess.Entities;
using TimeIsMoney.Services.Auth.Models;
using TimeIsMoney.Services.Crypto;
using TimeIsMoney.Services.Token.Models;

namespace TimeIsMoney.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DomainTaskStatus _taskStatus;
        private readonly ICryptoContext _cryptoContext;
        private readonly IOptions<JwtOptions> _options;

        public TokenService(IUnitOfWork unitOfWork, DomainTaskStatus taskStatus, ICryptoContext cryptoContext, IOptions<JwtOptions> options)
        {
            _unitOfWork = unitOfWork;
            _taskStatus = taskStatus;
            _cryptoContext = cryptoContext;
            _options = options;
        }

        public TokenModel GetRefreshToken(RefreshTokenModel refreshToken)
        {
            throw new NotImplementedException();
        }

        public TokenModel GetToken(LoginCredentialsModel loginCredentials)
        {
            var user = _unitOfWork.Repository<UserEntity>().Set.FirstOrDefault(x => x.Email == loginCredentials.Email);

            if (user == null)
            {
                _taskStatus.AddError("email", "User doesn't exists");
                return null;
            }

            if (!_cryptoContext.ArePasswordsEqual(loginCredentials.Password, user.Password, user.Salt))
            {
                _taskStatus.AddError("email", "Invalid credentials");
                return null;
            }

            return BuildToken(user);
        }

        public TokenModel GetTokenForConfirmedUser(Guid guid)
        {
            throw new NotImplementedException();
        }

        private TokenModel BuildToken(UserEntity user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Acr, user.Role),
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Firstname + " " + user.Lastname)
                };

            // TODO Change expiration date
            var token = new JwtSecurityToken(
              issuer: _options.Value.ValidIssuer,
              audience: _options.Value.ValidAudience,
              claims: claims,
              expires: DateTime.Now.AddDays(30),
              signingCredentials: creds);

            var refreshToken = Guid.NewGuid().ToString();

            _unitOfWork.SaveChanges();

            return new TokenModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = DateTimeOffset.Now.AddMinutes(30),
                IssuedAt = DateTimeOffset.Now,
                UserId = user.Id
            };
        }
    }
}