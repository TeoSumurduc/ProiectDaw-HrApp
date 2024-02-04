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

        [HttpGet("{codProiect}")]
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
    }
}
