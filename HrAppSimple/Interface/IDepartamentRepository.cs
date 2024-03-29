﻿using HrAppSimple.Models;

namespace HrAppSimple.Interface
{
    public interface IDepartamentRepository
    {
        ICollection<Departament> GetDepartamente();
        Departament GetDepartament(int codDepartament);
        bool DepartamentExista(int codDepartament);

        bool CreateDepartament(Departament departament);
        bool UpdateDepartament(Departament departament);
        bool DeleteDepartament(Departament departament);
        bool Save();
    }
}
