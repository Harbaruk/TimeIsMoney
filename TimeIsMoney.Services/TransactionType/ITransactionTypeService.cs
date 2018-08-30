using System;
using System.Collections.Generic;
using System.Text;
using TimeIsMoney.Services.TransactionType.Models;

namespace TimeIsMoney.Services.TransactionType
{
    public interface ITransactionTypeService
    {
        TransactionTypeModel AddType(CreateTransactionTypeModel typeModel);
        ICollection<TransactionTypeModel> Autocomplete(string name);
    }
}