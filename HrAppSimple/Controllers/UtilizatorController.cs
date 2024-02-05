using AutoMapper;
using HrAppSimple.Dto;
using HrAppSimple.Interface;
using HrAppSimple.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HrAppSimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizatorController : ControllerBase
    {
        public static Utilizator user = new Utilizator();
        private readonly IConfiguration _congif;
        private readonly IUtilizatorRepository _utilizatorInterface;
        private readonly IMapper _mapper;


        public UtilizatorController(IUtilizatorRepository utilizatorInterface, IMapper mapper, IConfiguration congif)
        {
            _congif = congif;
            mapper = mapper;
            _utilizatorInterface = utilizatorInterface;
        }

        [HttpGet, Authorize(Roles = "noob")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Utilizator>))]

        public IActionResult Getusers()
        {


            var users = _utilizatorInterface.GetUtilizatori();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Utilizator>> Register(UtilizatorDto rq)
        {
            CreatePasswordHash(rq.Parola, out byte[] passwarodhash, out byte[] passwordsalt);
            user.Nume = rq.Nume;
            user.PasswordHash = passwarodhash;
            user.PasswordSalt = passwordsalt;

            return Ok(user);

        }
        [HttpPost("login")]
        public async Task<ActionResult<String>> Login(UtilizatorDto rq)
        {
            if (user.Nume != rq.Nume)
            {
                return BadRequest("UserNotFound");
            }
            if (!Verify(rq.Parola, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Incorect Password");

            string token = CreateToken(rq);
            return Ok(token);
        }


        private string CreateToken(UtilizatorDto user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nume),
                new Claim(ClaimTypes.Role, "noob")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_congif.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private void CreatePasswordHash(string password, out byte[] passwarodhash, out byte[] passwordsalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwarodhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool Verify(string password, byte[] passwarodhash, byte[] passwordsalt)
        {
            using (var hmac = new HMACSHA512(passwordsalt))
            {
                var cmpHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return cmpHash.SequenceEqual(passwarodhash);
            }
        }
    }
}
