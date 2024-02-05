namespace HrAppSimple.Models
{
    public class Utilizator
    {
        public int Id { get; set; }
        public string Nume { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
    }
}
