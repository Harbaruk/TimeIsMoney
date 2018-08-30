using System;
using System.Collections.Generic;

namespace TimeIsMoney.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Salt { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public ConfirmationCodeEntity ConfirmationCode { get; set; }

        public List<TransactionEntity> Transactions { get; set; }

        public List<UserTransactionTypeRefEntity> TransactionTypes { get; set; }
    }
}