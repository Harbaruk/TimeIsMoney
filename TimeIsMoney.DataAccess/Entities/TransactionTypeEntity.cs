using System;
using System.Collections.Generic;
using System.Text;

namespace TimeIsMoney.DataAccess.Entities
{
    public class TransactionTypeEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public List<UserTransactionTypeRefEntity> Users { get; set; }
    }
}