using System;

namespace TimeIsMoney.DataAccess.Entities
{
    public class ConfirmationCodeEntity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public UserEntity User { get; set; }

        public DateTimeOffset ExpiresAt { get; set; }
    }
}