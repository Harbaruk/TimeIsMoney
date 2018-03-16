using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;
using TimeIsMoney.Common.EmailSender.Models;

namespace TimeIsMoney.Common.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<EmailOptions> _options;
        private readonly IOptions<RedirectOptions> _redirecOptions;
        private readonly string _templatesFolder;

        public EmailSender(IOptions<EmailOptions> options, IOptions<RedirectOptions> redirectOptions)
        {
            _options = options;
            _redirecOptions = redirectOptions;
            _templatesFolder = Environment.CurrentDirectory + "\\Templates\\";
        }

        private async Task SendMail(string to, string subject, string html)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.Value.From));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject ?? "PTTS";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = html
            };
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_options.Value.Host, _options.Value.Port, false).ConfigureAwait(false);
                    await client.AuthenticateAsync(_options.Value.From, _options.Value.Password).ConfigureAwait(false);
                    await client.SendAsync(message).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to send message", e);
            }
        }

        public void Confirmation(EmailConfirmModel receiver)
        {
            var link = _redirecOptions.Value.RedirectUrl + $"/auth/confirm?guid={receiver.Code}";
            string html = File.ReadAllText(_templatesFolder + EmailFiles.Invitation)
                .Replace("{TITLE}", "PTS")
                .Replace("{LINK}", link)
                .Replace("{NAME}", receiver.Firstname + " " + receiver.Lastname);

            SendMail(receiver.Email, null, html);
        }
    }
}