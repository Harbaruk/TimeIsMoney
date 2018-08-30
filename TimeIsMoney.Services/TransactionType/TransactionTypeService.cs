using System.Collections.Generic;
using System.Linq;
using TimeIsMoney.Common;
using TimeIsMoney.DataAccess;
using TimeIsMoney.DataAccess.Entities;
using TimeIsMoney.Services.ProviderAbstraction;
using TimeIsMoney.Services.TransactionType.Models;

namespace TimeIsMoney.Services.TransactionType
{
    public class TransactionTypeService : ITransactionTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticaterUserProvider _authenticaterUser;
        private readonly DomainTaskStatus _taskStatus;

        public TransactionTypeService(
            IUnitOfWork unitOfWork,
            IAuthenticaterUserProvider authenticaterUser,
            DomainTaskStatus taskStatus)
        {
            _unitOfWork = unitOfWork;
            _authenticaterUser = authenticaterUser;
            _taskStatus = taskStatus;
        }

        public TransactionTypeModel AddType(CreateTransactionTypeModel typeModel)
        {
            if (!_authenticaterUser.IsAuthenticated)
            {
                _taskStatus.AddError("Unauthorized", "Unathorized access");
                return null;
            }

            if (_unitOfWork.Repository<TransactionTypeEntity>().Set.Any(x => x.Name == typeModel.Name))
            {
                _taskStatus.AddUnkeyedError("Type is already exists");
                return null;
            }

            var currUser = _unitOfWork.Repository<UserEntity>().Set.FirstOrDefault(x => x.Id == _authenticaterUser.UserId);

            var type = new TransactionTypeEntity
            {
                Icon = typeModel.Icon,
                Name = typeModel.Name,
                Users = new List<UserTransactionTypeRefEntity>()
            };

            type.Users.Add(new UserTransactionTypeRefEntity
            {
                TransactionType = type,
                User = currUser
            });

            _unitOfWork.Repository<TransactionTypeEntity>().Insert(type);
            _unitOfWork.SaveChanges();

            return new TransactionTypeModel
            {
                Icon = type.Icon,
                Id = type.Id,
                Name = type.Name
            };
        }

        public ICollection<TransactionTypeModel> Autocomplete(string name)
        {
            return _unitOfWork.Repository<TransactionTypeEntity>().Set
                .Where(x => x.Name.ToLower().StartsWith(name.ToLower()))
                .Select(x => new TransactionTypeModel
                {
                    Icon = x.Icon,
                    Name = x.Name,
                    Id = x.Id
                })
                .ToList(); ;
        }
    }
}