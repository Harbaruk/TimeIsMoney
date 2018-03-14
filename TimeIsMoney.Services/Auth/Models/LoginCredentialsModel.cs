namespace TimeIsMoney.Services.Auth.Models
{
    public class LoginCredentialsModel
    {
        public string GrantType { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}