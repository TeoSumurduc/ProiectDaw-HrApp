namespace HrAppSimple.Dto
{
    public class UtilizatorDto
    {
        public int Id { get; set; }
        public string Nume { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;

        public string Parola { get; set; } = string.Empty;
    }
}
