using HrAppSimple.Data;
using HrAppSimple.Interface;
using HrAppSimple.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.CompilerServices;

namespace HrAppSimple.Repository
{
    public class LocatieRepository : ILocatieRepository
    {
        private readonly DataContext _context;
        public LocatieRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateLocatie(Locatie locatie)
        {
            _context.Add(locatie);
            return Save();
        }

        public Locatie GetLocatie(int codLocatie)
        {
            return _context.Locatii.Where(l => l.CodLocatie == codLocatie).FirstOrDefault();
        }

        public Locatie GetLocatieByDepartament(int codDepartament)
        { 
               return _context.Departamente
                   .Where(d => d.CodDepartament == codDepartament)
                   .SelectMany(d => d.Locatie) 
                   .FirstOrDefault();
        }

        public string GetLocatieOras(int codLocatie)
        {
            var locatie = _context.Locatii.FirstOrDefault(l => l.CodLocatie == codLocatie);
            if (locatie == null)
            {
                return null;
            }
            var oras = locatie.Oras ?? string.Empty;
            return oras;
        }

        public string GetLocatieOrasSiTata(int codLocatie)
        {
            if(GetLocatieOras(codLocatie) == null) {
                return null;
            }

            if(GetLocatieTara(codLocatie) == null)
            {
                return null;
            }

            return "Oras: " + GetLocatieOras(codLocatie) + "; Tara: " + GetLocatieTara(codLocatie);
        }

        public string GetLocatieTara(int codLocatie)
        {
            var locatie = _context.Locatii.FirstOrDefault(l => l.CodLocatie == codLocatie);
            if (locatie == null)
            {
                return null;
            }
            var tara = locatie.Tara ?? string.Empty;
            return tara;
        }

        public ICollection<Locatie> GetLocatii()
        {
            return _context.Locatii.ToList();
        }

        public bool LocatieExista(int codLocatie)
        {
            return _context.Locatii.Any(l => l.CodLocatie == codLocatie);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateLocatie(Locatie locatie)
        {
            _context.Update(locatie);
            return Save();
        }
    }
}
