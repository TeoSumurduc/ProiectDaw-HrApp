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
            return _context.Angajati.Any(a => a.Matricula == matricula);
        }

        public bool CreateAngajat(int codProiect, Angajat angajat)
        {
            var angajatProiectEntity = _context.Proiecte.Where(a => a.CodProiect == codProiect).FirstOrDefault();

            var angajatProiect = new AngajatProiect()
            {
                Proiect = angajatProiectEntity,
                Angajat = angajat,

            };

            _context.Add(angajatProiect);
            _context.Add(angajat);
            return Save();
        }

        public bool DeleteAngajat(Angajat angajat)
        {
            _context.Remove(angajat);
            return Save();
        }

        public Angajat GetAngajat(int matricula)
        {
            return _context.Angajati.Where(p => p.Matricula == matricula).FirstOrDefault();
        }

        public ICollection<Angajat> GetAngajatByProiect(int codProiect)
        {
            return _context.AngajatiProiecte.Where(a => a.Proiect.CodProiect == codProiect).Select(p => p.Angajat).ToList();
        }

        public ICollection<Angajat> GetAngajati()
        {
            return _context.Angajati.OrderBy(P => P.Matricula).ToList();
        }

        public string GetAngajatNumePrenume(int matricula)
        {
            var angajat = _context.DetaliiAngajati.FirstOrDefault(a => a.MatriculaAng == matricula);
            if (angajat==null)
            {
                return null;
            }
            var nume = angajat.Nume ?? string.Empty;
            var prenume = angajat.Prenume ?? string.Empty;
            return nume + " " + prenume;
        }

        public DateTime GetDataAngajare(int matricula)
        {
            var angajat = _context.DetaliiAngajati.FirstOrDefault(a => a.MatriculaAng == matricula);
            if (angajat == null)
            {
                throw new ArgumentException("Invalid project code.", nameof(matricula));
            }
            var dataAngajare = angajat.DataAngajare;
            return dataAngajare;
        }

        public DateTime GetDataNastere(int matricula)
        {
            var angajat = _context.DetaliiAngajati.FirstOrDefault(a => a.MatriculaAng == matricula);
            if (angajat == null)
            {
                throw new ArgumentException("Invalid project code.", nameof(matricula));
            }
            var dataNastere = angajat.DataNastere;
            return dataNastere;
        }

        public string GetEmail(int matricula)
        {
            var angajat = _context.DetaliiAngajati.FirstOrDefault(a => a.MatriculaAng == matricula);
            if (angajat == null)
            {
                return null;
            }
            var email=angajat.Email ?? string.Empty;
            return email;
        }

        public ICollection<Proiect> GetProiectOfAAngajat(int codAngajat)
        {
            return _context.AngajatiProiecte.Where(p => p.Angajat.Matricula == codAngajat).Select(p => p.Proiect).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved>0 ? true : false;
        }

        public bool UpdateAngajat(int codProiect,Angajat angajat)
        {
            _context.Update(angajat);
            return Save();
        }
    }
}
