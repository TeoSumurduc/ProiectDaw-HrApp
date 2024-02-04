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
            return _context.Departament.Where(d => d.CodDepartament == codDepartament).FirstOrDefault();
        }
        public ICollection<Departament> GetDepartamente()
        {
            return _context.Departament.ToList();
        }
        public bool DepartamentExista(int codDepartament)
        {
            return _context.Departament.Any(d => d.CodDepartament == codDepartament);
        }

        public bool CreateDepartament(Departament departament)
        { 
            _context.Add(departament);
      
            return Save();

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >0 ? true : false;
        }

        public bool UpdateDepartament(Departament departament)
        {
            _context.Update(departament);
            return Save();
        }

        public bool DeleteDepartament(Departament departament)
        {
            _context.Remove(departament);
            return Save();
        }
    }
}
