namespace HrAppSimple.Models
{
    public class Angajat
    {
        public int Matricula {  get; set; }
        public int CodDepartament { get; set; }
        public Departament Departament { get; set; }
        public DetaliiAngajat DetaliiAngajat { get; set; }

        public ICollection<AngajatProiect> AngajatProiect { get; set; }


    }
}
