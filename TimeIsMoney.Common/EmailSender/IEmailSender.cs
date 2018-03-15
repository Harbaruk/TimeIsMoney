using TimeIsMoney.Common.EmailSender.Models;

namespace TimeIsMoney.Common.EmailSender
{
    public interface IEmailSender
    {
        void Confirmation(EmailConfirmModel receiver);
    }
}