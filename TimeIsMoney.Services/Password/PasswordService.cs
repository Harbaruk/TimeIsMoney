using System;
using System.Linq;
using TimeIsMoney.Common;
using TimeIsMoney.Common.EmailSender;
using TimeIsMoney.DataAccess;
using TimeIsMoney.DataAccess.Entities;
using TimeIsMoney.Services.Crypto;
using TimeIsMoney.Services.Password.Models;
using TimeIsMoney.Services.ProviderAbstraction;

namespace TimeIsMoney.Services.Password
{
    public class PasswordService : IPasswordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly DomainTaskStatus _taskStatus;
        private readonly IAuthenticaterUserProvider _authenticaterUser;
        private readonly ICryptoContext _cryptoContext;

        public PasswordService(IUnitOfWork unitOfWork, IEmailSender emailSender, DomainTaskStatus taskStatus, IAuthenticaterUserProvider authenticaterUser, ICryptoContext cryptoContext)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _taskStatus = taskStatus;
            _authenticaterUser = authenticaterUser;
            _cryptoContext = cryptoContext;
        }

        public void ChangePassword(ChangePasswordModel changePassword)
        {
            var user = _unitOfWork.Repository<UserEntity>().Set.FirstOrDefault(x => x.Id == _authenticaterUser.UserId);

            if (user == null)
            {
                _taskStatus.AddUnkeyedError("Invalid user");
                return;
            }

            if (_cryptoContext.ArePasswordsEqual(changePassword.OldPassword, user.Password, user.Salt))
            {
                var newSalt = _cryptoContext.GenerateSaltAsBase64();
                var newPassword = _cryptoContext.DeriveKey(changePassword.NewPassword, newSalt);

                user.Salt = newSalt;
                user.Password = Convert.ToBase64String(newPassword);

                _unitOfWork.SaveChanges();
            }
        }

        public void ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            throw new NotImplementedException();
        }

        public void SetPassword(SetPasswordModel setPassword)
        {
            var user = _unitOfWork.Repository<UserEntity>().Include(x => x.ConfirmationCode).FirstOrDefault(x => x.ConfirmationCode.Code == setPassword.Code);

            if (user == null)
            {
                _taskStatus.AddUnkeyedError("Invalid code");
                return;
            }

            var salt = _cryptoContext.GenerateSaltAsBase64();
            var password = _cryptoContext.DeriveKey(setPassword.NewPassword, salt);

            user.Salt = salt;
            user.Password = Convert.ToBase64String(password);

            _unitOfWork.SaveChanges();
        }
    }
}