
using AutoMapper;
using HrAppSimple.Dto;
using HrAppSimple.Interface;
using HrAppSimple.Models;
using HrAppSimple.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HrAppSimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentController : Controller
    {
        private readonly IDepartamentRepository _departamentRepository;
        private readonly IMapper _mapper;
        public DepartamentController(IDepartamentRepository departamentRepository, IMapper mapper) 
        {
            _departamentRepository = departamentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Departament>))]
        public IActionResult GetDepartamente()
        {
            var departamente = _mapper.Map<List<DepartamentDto>>(_departamentRepository.GetDepartamente());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(departamente);
        }

        [HttpGet("{Cod Departament}")]
        [ProducesResponseType(200, Type = typeof(Departament))]
        [ProducesResponseType(400)]
        public IActionResult GetDepartament(int codDepartament)
        {
            if (!_departamentRepository.DepartamentExista(codDepartament))
                return NotFound();

            var departament = _mapper.Map<DepartamentDto>(_departamentRepository.GetDepartament(codDepartament));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(departament);
        }

    }
}
