using HrAppSimple.Models;

namespace HrAppSimple.Interface
{
    public interface IAngajatRepository
    {
        ICollection<Angajat> GetAngajati();
        Angajat GetAngajat(int matricula);
        string GetAngajatNumePrenume(int matricula);
        string GetEmail(int matricula);
        DateTime GetDataNastere(int matricula);
        DateTime GetDataAngajare(int matricula);
        bool AngajatExista(int matricula);
        bool CreateAngajat(int codProiect, Angajat angajat);
        ICollection<Proiect> GetProiectOfAAngajat(int codAngajat);
        ICollection<Angajat> GetAngajatByProiect(int codProiect);
        bool Save();
    }
}
