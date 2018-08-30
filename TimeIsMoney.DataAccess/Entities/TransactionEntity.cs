using System;
using System.Collections.Generic;
using System.Text;

namespace TimeIsMoney.DataAccess.Entities
{
    public class TransactionEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public double Amount { get; set; }
        public DateTimeOffset Time { get; set; }
        public TransactionTypeEntity Type { get; set; }
        public UserEntity Owner { get; set; }
    }
}