using Microsoft.AspNetCore.Identity;

namespace HrAppSimple.Models
{
    public class Utilizator: IdentityUser
    {
        public int Id { get; set; }
        public string Nume { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsAdmin { get; set; } = false;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

    }
}
