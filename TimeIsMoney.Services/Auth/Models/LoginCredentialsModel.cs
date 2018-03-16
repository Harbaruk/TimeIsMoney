namespace TimeIsMoney.Services.Auth.Models
{
    public class LoginCredentialsModel
    {
        public string GrantType { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}