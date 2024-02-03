
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
    public class LocatieController: Controller
    {
        private readonly ILocatieRepository _locatieRepository;
        private readonly IMapper _mapper;
        private readonly IDepartamentRepository _departamentRepository;

        public LocatieController(ILocatieRepository locatieRepository, IMapper mapper,
            IDepartamentRepository departamentRepository)
        {
            _locatieRepository = locatieRepository;
            _mapper = mapper;
            _departamentRepository = departamentRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Locatie>))]
        public IActionResult GetLocatii()
        {
            var locatii = _mapper.Map<List<LocatieDto>>(_locatieRepository.GetLocatii());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(locatii);
        }

        [HttpGet("{Cod Locatie}")]
        [ProducesResponseType(200, Type = typeof(Locatie))]
        [ProducesResponseType(400)]
        public IActionResult GetLocatie(int codLocatie)
        {
            if (!_locatieRepository.LocatieExista(codLocatie))
                return NotFound();

            var locatie = _mapper.Map<LocatieDto>(_locatieRepository.GetLocatie(codLocatie));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(locatie);
        }

        [HttpGet("{Cod Locatie}/Oras")]
        [ProducesResponseType(200, Type = typeof(Locatie))]
        [ProducesResponseType(400)]
        public IActionResult GetLocatieOras(int codLocatie)
        {
            if (!_locatieRepository.LocatieExista(codLocatie))
                return NotFound();

            var oras = _mapper.Map<LocatieDto>(_locatieRepository.GetLocatieOras(codLocatie));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(oras);
        }

        [HttpGet("{Cod Locatie}/Tara")]
        [ProducesResponseType(200, Type = typeof(Locatie))]
        [ProducesResponseType(400)]
        public IActionResult GetLocatieTara(int codLocatie)
        {
            if (!_locatieRepository.LocatieExista(codLocatie))
                return NotFound();

            var tara = _mapper.Map<LocatieDto>(_locatieRepository.GetLocatieTara(codLocatie));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tara);
        }

        [HttpGet("{Cod Locatie}/Oras+Tara")]
        [ProducesResponseType(200, Type = typeof(Locatie))]
        [ProducesResponseType(400)]
        public IActionResult GetLocatieOrasSiTata(int codLocatie)
        {
            if (!_locatieRepository.LocatieExista(codLocatie))
                return NotFound();

            var oras_tara = _mapper.Map<LocatieDto>(_locatieRepository.GetLocatieOrasSiTata(codLocatie));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(oras_tara);
        }

        [HttpGet("{Cod Departament}/LocatieByDepartament")]
        [ProducesResponseType(200, Type = typeof(Locatie))]
        [ProducesResponseType(400)]
        public IActionResult GetLocatieByDepartament(int codDepartament)
        {
            if (!_locatieRepository.LocatieExista(codDepartament))
                return NotFound();

            var locatie = _mapper.Map<LocatieDto>(_locatieRepository.GetLocatieByDepartament(codDepartament));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(locatie);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateLocatie([FromRoute]int codDepartament,[FromBody] LocatieDto locatieCreate)
        {
            if (locatieCreate == null)
                return BadRequest(ModelState);

            //verificam daca exista dubluri
            var locatii = _locatieRepository.GetLocatii()
                .Where(c => c.CodLocatie == locatieCreate.CodLocatie)
                .FirstOrDefault();

            if (locatii != null)
            {
                ModelState.AddModelError("", "Locatia exista deja!");
                return StatusCode(422, ModelState);
            }

            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var locatieMap = _mapper.Map<Locatie>(locatieCreate);

            locatieMap.Departament = _departamentRepository.GetDepartament(codDepartament);

            if (!_locatieRepository.CreateLocatie(locatieMap))
            {
                ModelState.AddModelError("", "A aparut o eroare in timp ce salvam!");
                return StatusCode(500, ModelState);
            }

            return Ok("Avem o locatie noua!");
        }

        [HttpPut("{Cod Locatie}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateLoctie(int codLocatie, [FromBody] LocatieDto updateLocatie)
        {
            if(updateLocatie == null)
                return BadRequest(ModelState);

            if(codLocatie!=updateLocatie.CodLocatie)
                return BadRequest(ModelState);

            if (!_locatieRepository.LocatieExista(codLocatie))
                return NotFound();  

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var locatieMap = _mapper.Map<Locatie>(updateLocatie);

            if(!_locatieRepository.UpdateLocatie(locatieMap))
            {
                ModelState.AddModelError("", "A aparut o eroare la update");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
