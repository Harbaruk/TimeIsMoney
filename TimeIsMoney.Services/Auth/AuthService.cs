using System;
using System.Linq;
using TimeIsMoney.Common;
using TimeIsMoney.DataAccess;
using TimeIsMoney.DataAccess.Entities;
using TimeIsMoney.Services.Auth.Models;
using TimeIsMoney.Services.Crypto;
using TimeIsMoney.Services.Enums;

namespace TimeIsMoney.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptoContext _cryptoContext;
        private readonly DomainTaskStatus _taskStatus;

        public AuthService(IUnitOfWork unitOfWork, ICryptoContext cryptoContext, DomainTaskStatus taskStatus)
        {
            _unitOfWork = unitOfWork;
            _cryptoContext = cryptoContext;
            _taskStatus = taskStatus;
        }

        public void Register(UserRegistrationModel model)
        {
            var existedUser = _unitOfWork.Repository<UserEntity>().Set.FirstOrDefault(x => x.Email == model.Email);

            if (existedUser != null)
            {
                _taskStatus.AddError("email", "User already exists");
                return;
            }

            var salt = _cryptoContext.GenerateSaltAsBase64();

            var user = new UserEntity
            {
                Email = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Salt = salt,
                Password = Convert.ToBase64String(_cryptoContext.DeriveKey(model.Password, salt)),
                Role = Role.User.ToString()
            };

            _unitOfWork.Repository<UserEntity>().Insert(user);
            _unitOfWork.SaveChanges();
        }
    }
}