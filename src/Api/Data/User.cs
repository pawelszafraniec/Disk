using System.ComponentModel.DataAnnotations.Schema;

namespace Disk.Api.Data
{
    [Table("user")]
    public class User
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("login")]
        public string Login { get; set; } = null!;

        [Column("password")]
        public string Password { get; set; } = null!;

        [Column("email")]
        public string EmailAddress { get; set; } = null!;
    }
}
