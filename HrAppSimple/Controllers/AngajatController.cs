
using AutoMapper;
using HrAppSimple.Dto;
using HrAppSimple.Interface;
using HrAppSimple.Models;
using HrAppSimple.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HrAppSimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AngajatController : Controller
    {
        private readonly IAngajatRepository _angajatRepository;
        private readonly IMapper _mapper;
        private readonly IProiectRepository _proiecteRepository;

        public AngajatController(IAngajatRepository angajatRepository, IMapper mapper)
        {
            _angajatRepository = angajatRepository;
            _mapper = mapper;
        }

        [HttpGet, Authorize]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Angajat>))]
        public IActionResult GetAngajati()
        {
            var angajati = _mapper.Map<List<AngajatDto>>(_angajatRepository.GetAngajati());

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(angajati);
        }

        [HttpGet("{matricula}")]
        [ProducesResponseType(200,Type = typeof(Angajat))]
        [ProducesResponseType(400)]
        public IActionResult GetAngajat(int matricula)
        {
            if(!_angajatRepository.AngajatExista(matricula))
                return NotFound();

            var angajat = _mapper.Map<AngajatDto>(_angajatRepository.GetAngajat(matricula));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(angajat);
        }

        [HttpGet("{matricula}/nume+prenume")]
        [ProducesResponseType(200, Type = typeof(Angajat))]
        [ProducesResponseType(400)]
        public IActionResult GetAngajatNumePrenume(int matricula)
        {
            if (!_angajatRepository.AngajatExista(matricula))
                return NotFound();

            var nume = _angajatRepository.GetAngajatNumePrenume(matricula);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(nume);
        }

        [HttpGet("{matricula}/email")]
        [ProducesResponseType(200, Type = typeof(Angajat))]
        [ProducesResponseType(400)]
        public IActionResult GetEmail(int matricula)
        {
            if (!_angajatRepository.AngajatExista(matricula))
                return NotFound();

            var email = _angajatRepository.GetEmail(matricula);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(email);
        }

        [HttpGet("{matricula}/data angajare")]
        [ProducesResponseType(200, Type = typeof(Angajat))]
        [ProducesResponseType(400)]
        public IActionResult GetDataAngajare(int matricula)
        {
            if (!_angajatRepository.AngajatExista(matricula))
                return NotFound();

            var dataAngajare = _angajatRepository.GetDataAngajare(matricula);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(dataAngajare);
        }

        [HttpGet("{matricula}/data nastere")]
        [ProducesResponseType(200, Type = typeof(Angajat))]
        [ProducesResponseType(400)]
        public IActionResult GetDataNastere(int matricula)
        {
            if (!_angajatRepository.AngajatExista(matricula))
                return NotFound();

            var dataNastere = _angajatRepository.GetDataNastere(matricula);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(dataNastere);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAngajat([FromQuery] int codProiect, [FromBody] AngajatDto angajatCreate)
        {
            if(angajatCreate == null)
                return BadRequest(ModelState);

            var angajati = _angajatRepository.GetAngajati()
                .Where(c => c.Matricula == angajatCreate.Matricula)
                .FirstOrDefault();

            if (angajati != null)
            {
                ModelState.AddModelError("", "Angajatul exista deja!");
                return StatusCode(422, ModelState);
            }

            if(ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var angajatMap = _mapper.Map<Angajat>(angajatCreate);

            if(!_angajatRepository.CreateAngajat(codProiect, angajatMap))
            {
                ModelState.AddModelError("", "A aparut o eroare in timp ce salvam!");
                return StatusCode(500,ModelState);
            }

            return Ok("Avem un angajat nou!");
        }

        [HttpGet("{Matricula}/proiect")]
        [ProducesResponseType(200, Type = typeof(Angajat))]
        [ProducesResponseType(400)]
        public IActionResult GetProiectOfAAngajat(int matricula)
        {
            if (!_angajatRepository.AngajatExista(matricula))
            {
                return NotFound();
            }

            var angajat = _mapper.Map<ProiectDto>(
                _angajatRepository.GetProiectOfAAngajat(matricula));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(angajat);
        }

        [HttpPut("{Matricula}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAngajat(int matricula,
            [FromQuery] int codProiect, 
            [FromBody] AngajatDto updatedAngajat)
        {
            if (updatedAngajat == null)
                return BadRequest(ModelState);

            if (matricula != updatedAngajat.Matricula)
                return BadRequest(ModelState);

            if (!_angajatRepository.AngajatExista(matricula))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var angajatMap = _mapper.Map<Angajat>(updatedAngajat);

            if (!_angajatRepository.UpdateAngajat(codProiect, angajatMap))
            {
                ModelState.AddModelError("", "A aparut o eroare la update!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{Matricula}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAngajat(int matricula)
        {
            if (!_angajatRepository.AngajatExista(matricula))
            {
                return NotFound();
            }

            var proiecteToDelete = _proiecteRepository.GetProiectOfAAngajat(matricula);
            var angajatToDelete = _angajatRepository.GetAngajat(matricula);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_proiecteRepository.DeleteProiect((Proiect)proiecteToDelete))
            {
                ModelState.AddModelError("", "A aparut o eroare la delete proiecte!");
            }

            if (!_angajatRepository.DeleteAngajat(angajatToDelete))
            {
                ModelState.AddModelError("", "A aparut o eroare la delete angajat!");
            }

            return NoContent();
        }
    }
}
