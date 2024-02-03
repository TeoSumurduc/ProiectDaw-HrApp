using HrAppSimple.Data;
using HrAppSimple.Interface;
using HrAppSimple.Models;

namespace HrAppSimple.Repository
{
    public class DepartamentRepository : IDepartamentRepository
    {
        private readonly DataContext _context;
        public DepartamentRepository(DataContext context)
        {
            _context = context;
        }
        public bool DepartamentExista(int codDepartament)
        {
            return _context.Departament.Any(d => d.CodDepartament == codDepartament);
        }

        public Departament GetDepartament(int codDepartament)
        {
            return _context.Departament.Where(d => d.CodDepartament == codDepartament).FirstOrDefault();
        }

        public ICollection<Departament> GetDepartamente()
        {
            return _context.Departament.ToList();
        }
    }
}
