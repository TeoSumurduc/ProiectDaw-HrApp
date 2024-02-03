namespace HrAppSimple.Models
{
    public class Locatie
    {
        public int CodLocatie { get; set; }
        public string Oras {  get; set; }

        public string Tara  { get; set; }
        public Departament Departament { get; set; }
    }
}
