using AutoMapper;
using HrAppSimple.Data;
using HrAppSimple.Interface;
using HrAppSimple.Models;

namespace HrAppSimple.Repository
{
    public class UtilizatorRepository : IUtilizatorRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UtilizatorRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateUtilizator(Utilizator user)
        {
            _context.Add(user);
            return Save();
        }

        public bool DeleteUtilizator(Utilizator user)
        {
            _context.Remove(user);
            return Save();
        }

        public Utilizator GetUtilizator(int id)
        {
            return _context.Utilizatori.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Utilizator> GetUtilizatori()
        {
            return _context.Utilizatori.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }


        public bool UpdateUtilizator(Utilizator user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
