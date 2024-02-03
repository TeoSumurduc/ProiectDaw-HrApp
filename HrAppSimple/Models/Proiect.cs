namespace HrAppSimple.Models
{
    public class Proiect
    {
        public int CodProiect { get; set; }
        public string Denumire { get; set; }
        public DateTime DataPredare { get; set; }

        public ICollection<AngajatProiect> AngajatProiect { get; set; }
    }
}
