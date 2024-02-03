namespace HrAppSimple.Models
{
    public class Departament
    {
        public int CodDepartament { get; set; }
        public string Denumire { get; set; }
        public ICollection<Angajat> Angajat { get; set;}
        public ICollection<Locatie> Locatie { get; set; }
    }
}
