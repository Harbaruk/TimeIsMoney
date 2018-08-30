using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using TimeIsMoney.Services.Enums;
using TimeIsMoney.Services.ProviderAbstraction;

namespace TimeIsMoney.Api.Providers
{
    internal sealed class AuthenticatedUserProvider : IAuthenticaterUserProvider
    {
        private readonly IHttpContextAccessor _httpContext;

        public AuthenticatedUserProvider(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public bool IsAuthenticated
        {
            get
            {
                ClaimsPrincipal principal = _httpContext.HttpContext.User;
                return principal?.Identity?.IsAuthenticated ?? false;
            }
        }

        public Guid UserId
        {
            get
            {
                var user = _httpContext.HttpContext.User;
                if (!(user?.Identity?.IsAuthenticated) ?? false)
                {
                    return default(Guid);
                }
                string id = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid).Value;
                return Guid.Parse(id);
            }
        }

        public string Name
        {
            get
            {
                var user = _httpContext.HttpContext.User;
                return user?.Identity?.IsAuthenticated ?? false
                    ? user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value
                    : null;
            }
        }

        public string Email
        {
            get
            {
                var user = _httpContext.HttpContext.User;
                return user?.Identity?.IsAuthenticated ?? false
                    ? user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value
                    : null;
            }
        }

        public Role UserType
        {
            get
            {
                var user = _httpContext.HttpContext.User;
                return user?.Identity?.IsAuthenticated ?? false
                    ? (Role)Enum.Parse(typeof(Role), user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value)
                    : Role.NotDefined;
            }
        }
    }
}