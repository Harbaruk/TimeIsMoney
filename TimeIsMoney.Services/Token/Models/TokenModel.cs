using System;
using TimeIsMoney.Services.Enums;

namespace TimeIsMoney.Services.Token.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public DateTimeOffset IssuedAt { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public Guid UserId { get; set; }
        public Role Role { get; set; }
    }
}