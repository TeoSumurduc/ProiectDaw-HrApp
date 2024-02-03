using HrAppSimple.Models;

namespace HrAppSimple.Interface
{
    public interface IAngajatRepository
    {
        ICollection<Angajat> GetAngajati();
        Angajat GetAngajat(int matricula);
        string GetAngajatNumePrenume(int matricula);
        bool AngajatExista(int matricula);
        bool CreateAngajat(int codProiect, Angajat angajat);
        bool Save();
    }
}
