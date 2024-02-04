using HrAppSimple.Models;

namespace HrAppSimple.Interface
{
    public interface IProiectRepository
    {
        ICollection<Proiect> GetProiecte();
        Proiect GetProiect(int codProiect);
        string GetProiectDenumire(int codProiect);
        DateTime GetProiectDataPredare(int codProiect);
        bool ProiectExista(int codProiect);

        ICollection<Proiect> GetProiectOfAAngajat(int codAngajat);
        ICollection<Angajat> GetAngajatByProiect(int codProiect);
        bool CreateProiect(Proiect proiect);
        bool UpdateProiect(Proiect proiect);
        bool DeleteProiect(Proiect proiect);
        bool Save();
    }
}
