using AutoMapper;
using HrAppSimple.Dto;
using HrAppSimple.Models;

namespace HrAppSimple.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Angajat, AngajatDto>();
            CreateMap<Departament, DepartamentDto>();
            CreateMap<DepartamentDto, Departament>();
            CreateMap<Locatie, LocatieDto>();
            CreateMap<LocatieDto, Locatie>();
            CreateMap<AngajatDto, Angajat>();
            CreateMap<LocatieDto, Locatie>();
        }
    }
}
