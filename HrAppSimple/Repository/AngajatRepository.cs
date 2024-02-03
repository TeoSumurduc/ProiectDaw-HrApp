using HrAppSimple.Data;
using HrAppSimple.Interface;
using HrAppSimple.Models;

namespace HrAppSimple.Repository
{
    public class AngajatRepository : IAngajatRepository
    {
        private readonly DataContext _context;
        public AngajatRepository(DataContext context)
        {
            _context = context;
        }

        public bool AngajatExista(int matricula)
        {
            return _context.Angajat.Any(a => a.Matricula == matricula);
        }

        public bool CreateAngajat(int codProiect, Angajat angajat)
        {
            var angajatProiectEntity = _context.Proiect.Where(a => a.CodProiect == codProiect).FirstOrDefault();

            var angajatProiect = new AngajatProiect()
            {
                Proiect = angajatProiectEntity,
                Angajat = angajat,

            };

            _context.Add(angajatProiect);
            _context.Add(angajat);
            return Save();
        }

        public Angajat GetAngajat(int matricula)
        {
            return _context.Angajat.Where(p => p.Matricula == matricula).FirstOrDefault();
        }

        public ICollection<Angajat> GetAngajati()
        {
            return _context.Angajat.OrderBy(P => P.Matricula).ToList();
        }

        public string GetAngajatNumePrenume(int matricula)
        {
            var angajat = _context.DetaliiAngajat.FirstOrDefault(a => a.MatriculaAng == matricula);
            if (angajat==null)
            {
                return null;
            }
            var nume = angajat.Nume ?? string.Empty;
            var prenume = angajat.Prenume ?? string.Empty;
            return nume + " " + prenume;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved>0 ? true : false;
        }
    }
}
