namespace HrAppSimple.Models
{
    public class DetaliiAngajat
    {
        public int Matricula { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Email { get; set; }
        public DateTime DataNastere { get; set; }
        public DateTime DataAngajare { get; set; }

        public int MatriculaAng {  get; set; }
        public Angajat Angajat { get; set; } = null;

    }
}
