
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDepartament([FromBody] DepartamentDto departamentCreate)
        {
            if(departamentCreate == null)
                return BadRequest(ModelState);

            var department = _departamentRepository.GetDepartamente()
                .Where(c => c.CodDepartament == departamentCreate.CodDepartament)
                .FirstOrDefault();

            if(department != null)
            {
                ModelState.AddModelError("", "Departamentul deja exista!");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var departamentMap = _mapper.Map<Departament>(departamentCreate);

            if(!_departamentRepository.CreateDepartament(departamentMap))
            {
                ModelState.AddModelError("", "A aparut o eroare!");
                return StatusCode(500, ModelState);
            }

            return Ok("Avem un departament nou!");
        }
        [HttpPut("{codDepartament}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDepartament(int codDepartament, [FromBody] DepartamentDto updatedDepartament)
        {
            if (updatedDepartament == null)
                return BadRequest(ModelState);

            if (codDepartament != updatedDepartament.CodDepartament)
                return BadRequest(ModelState);

            if (!_departamentRepository.DepartamentExista(codDepartament))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var departamentMap = _mapper.Map<Departament>(updatedDepartament);

            if (!_departamentRepository.UpdateDepartament(departamentMap))
            {
                ModelState.AddModelError("", "A aparut ceva la update!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{codDepartament}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDepartament(int matricula)
        {
            if (!_departamentRepository.DepartamentExista(matricula))
            {
                return NotFound();
            }

            var departamentToDelete = _departamentRepository.GetDepartament(matricula);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_departamentRepository.DeleteDepartament(departamentToDelete))
            {
                ModelState.AddModelError("", "A aparut ceva la delete!");
            }

            return NoContent();
        }

    }
}
