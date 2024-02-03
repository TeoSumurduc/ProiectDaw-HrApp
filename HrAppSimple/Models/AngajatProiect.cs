namespace HrAppSimple.Models
{
    public class AngajatProiect
    {
        public int Matricula { get; set; }
        public int CodProiect { get; set; }

        public Angajat Angajat { get; set; }
        public Proiect Proiect { get; set; }
    }
}
