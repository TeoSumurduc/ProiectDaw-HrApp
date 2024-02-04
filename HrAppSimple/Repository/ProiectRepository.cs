using HrAppSimple.Data;
using HrAppSimple.Interface;
using HrAppSimple.Models;

namespace HrAppSimple.Repository
{
    public class ProiectRepository : IProiectRepository
    {
        private readonly DataContext _context;
        //constructor
        public ProiectRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateProiect(Proiect proiect)
        {
            _context.Add(proiect);
            return Save();
        }

        public ICollection<Angajat> GetAngajatByProiect(int codProiect)
        {
            return _context.AngajatiProiecte.Where(a => a.Proiect.CodProiect == codProiect).Select(p => p.Angajat).ToList();
        }

        public Proiect GetProiect(int codProiect)
        {
            return _context.Proiecte.Where(p => p.CodProiect == codProiect).FirstOrDefault();
        }

        public DateTime GetProiectDataPredare(int codProiect)
        {
            var proiect = _context.Proiecte.FirstOrDefault(p => p.CodProiect == codProiect);
            if (proiect == null)
            {
                throw new ArgumentException("Invalid project code.", nameof(codProiect));
            }
            DateTime dataPredare = proiect.DataPredare;
            return dataPredare;
        }

        public string GetProiectDenumire(int codProiect)
        {
            var proiect = _context.Proiecte.FirstOrDefault(p => p.CodProiect == codProiect);
            if (proiect == null)
            {
                throw new ArgumentException("Invalid project code.", nameof(codProiect));
            }
            var denumireProiect = proiect.Denumire ?? string.Empty;
            return denumireProiect;
        }

        public ICollection<Proiect> GetProiecte()
        {
            return _context.Proiecte.ToList();
        }

        public ICollection<Proiect> GetProiectOfAAngajat(int codAngajat)
        {
            return _context.AngajatiProiecte.Where(p => p.Angajat.Matricula == codAngajat).Select(p => p.Proiect).ToList();
        }

        public bool ProiectExista(int codProiect)
        {
            return _context.Proiecte.Any(p => p.CodProiect == codProiect);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
