using System.Security.Principal;

namespace Disk.Ui.Authentication
{
    public class UserPrincipal : GenericPrincipal
    {
        public string Name { get; } = "Name";

        public string? Token { get; }

        public UserPrincipal(string login, string? token = null) : base(new GenericIdentity("name"), new string[] { })
        {
            this.Token = token;
            this.Name = login;
        }

        public override string? ToString()
        {
            return this.Name;
        }
    }
}