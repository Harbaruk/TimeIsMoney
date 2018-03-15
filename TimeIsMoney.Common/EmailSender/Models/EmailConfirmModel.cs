namespace TimeIsMoney.Common.EmailSender.Models
{
    public class EmailConfirmModel
    {
        public string Code { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }
    }
}