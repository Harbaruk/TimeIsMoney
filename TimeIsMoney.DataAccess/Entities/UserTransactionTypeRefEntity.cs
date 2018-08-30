using System;
using System.Collections.Generic;
using System.Text;

namespace TimeIsMoney.DataAccess.Entities
{
    public class UserTransactionTypeRefEntity
    {
        public Guid UserId { get; set; }
        public int TransactionTypeId { get; set; }
        public UserEntity User { get; set; }
        public TransactionTypeEntity TransactionType { get; set; }
    }
}