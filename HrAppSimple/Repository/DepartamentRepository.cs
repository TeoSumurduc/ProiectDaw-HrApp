using HrAppSimple.Data;
using HrAppSimple.Interface;
using HrAppSimple.Models;

namespace HrAppSimple.Repository
{
    public class DepartamentRepository : IDepartamentRepository
    {
        private readonly DataContext _context;

        //constructor
        public DepartamentRepository(DataContext context)
        {
            _context = context;
        }

        public Departament GetDepartament(int codDepartament)
        {
            return _context.Departamente.Where(d => d.CodDepartament == codDepartament).FirstOrDefault();
        }
        public ICollection<Departament> GetDepartamente()
        {
            return _context.Departamente.ToList();
        }
        public bool DepartamentExista(int codDepartament)
        {
            return _context.Departamente.Any(d => d.CodDepartament == codDepartament);
        }
    }
}
