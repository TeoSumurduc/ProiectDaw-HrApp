using HrAppSimple.Models;

namespace HrAppSimple.Interface
{
    public interface IUtilizatorRepository
    {
        bool Save();
        bool DeleteUtilizator(Utilizator utilizatori);

        bool UpdateUtilizator(Utilizator utilizatori);

        Utilizator GetUtilizator(int id);

        bool CreateUtilizator(Utilizator utilizatori);
        bool UtilizatorExista(int id);

        ICollection<Utilizator> GetUtilizatori();
    }
}
