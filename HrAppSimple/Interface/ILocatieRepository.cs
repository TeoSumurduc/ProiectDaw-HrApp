using HrAppSimple.Models;

namespace HrAppSimple.Interface
{
    public interface ILocatieRepository
    {
        ICollection<Locatie> GetLocatii();
        Locatie GetLocatie(int codLocatie);
        Locatie GetLocatieByDepartament(int codDepartament);
        string GetLocatieOras(int codLocatie);
        string GetLocatieTara(int codLocatie);
        string GetLocatieOrasSiTata(int codLocatie);

        bool LocatieExista(int codLocatie);

        bool CreateLocatie(Locatie locatie);

        bool UpdateLocatie(Locatie locatie);
        bool Save();
    }
}
