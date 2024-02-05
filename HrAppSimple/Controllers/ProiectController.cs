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
    public class ProiectController: Controller
    {
        private readonly IProiectRepository _proiectRepository;
        private readonly IMapper _mapper;

        public ProiectController(IProiectRepository proiectRepository, IMapper mapper)
        {
            _proiectRepository = proiectRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Proiect>))]
        public IActionResult GetProiecte()
        {
            var proiecte = _mapper.Map<List<ProiectDto>>(_proiectRepository.GetProiecte());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(proiecte);
        }

        [HttpGet("{codProiect}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(Proiect))]
        [ProducesResponseType(400)]
        public IActionResult GetProiect(int codProiect)
        {
            if (!_proiectRepository.ProiectExista(codProiect))
                return NotFound();

            var proiect = _mapper.Map<ProiectDto>(_proiectRepository.GetProiect(codProiect));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(proiect);
        }

        [HttpGet("{codProiect}/angajat")]
        [ProducesResponseType(200, Type = typeof(Proiect))]
        [ProducesResponseType(400)]
        public IActionResult GetAngajatByProiect(int codProiect)
        {
            if (!_proiectRepository.ProiectExista(codProiect))
            {
                return NotFound();
            }

            var proiect = _mapper.Map<List<AngajatDto>>(
                _proiectRepository.GetAngajatByProiect(codProiect));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(proiect);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProiect([FromBody] ProiectDto proiectCreate)
        {
            if (proiectCreate == null)
                return BadRequest(ModelState);

            var proiect = _proiectRepository.GetProiecte()
                .Where(c => c.CodProiect == proiectCreate.CodProiect)
                .FirstOrDefault();

            if (proiect != null)
            {
                ModelState.AddModelError("", "Proiectul deja exista!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proiectMap = _mapper.Map<Proiect>(proiectCreate);

            if (!_proiectRepository.CreateProiect(proiectMap))
            {
                ModelState.AddModelError("", "A aparut o eroare!");
                return StatusCode(500, ModelState);
            }

            return Ok("Avem un departament nou!");
        }
        [HttpDelete("{codProiect}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProiect(int codProiect)
        {
            if (!_proiectRepository.ProiectExista(codProiect))
            {
                return NotFound();
            }

            var proiectToDelete = _proiectRepository.GetProiect(codProiect);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_proiectRepository.DeleteProiect(proiectToDelete))
            {
                ModelState.AddModelError("", "A aparut o eroare in timp ce stergeam proiectul!");
            }

            return NoContent();
        }

        [HttpPut("{codProiect}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProiect(int codProiect, [FromBody] ProiectDto updatedProiect)
        {
            if (updatedProiect == null)
                return BadRequest(ModelState);

            if (codProiect != updatedProiect.CodProiect)
                return BadRequest(ModelState);

            if (!_proiectRepository.ProiectExista(codProiect))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var categoryMap = _mapper.Map<Proiect>(updatedProiect);

            if (!_proiectRepository.UpdateProiect(categoryMap))
            {
                ModelState.AddModelError("", "A aparut o eroare in timp ce actualizam proiectul!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
