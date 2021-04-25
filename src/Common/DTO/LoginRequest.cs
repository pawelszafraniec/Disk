using System.ComponentModel.DataAnnotations;

namespace Disk.Common.DTO
{
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public LoginRequest()
        {

        }

        public LoginRequest(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}
