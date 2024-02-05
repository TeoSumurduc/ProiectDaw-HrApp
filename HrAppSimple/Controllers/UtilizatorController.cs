using AutoMapper;
using HrAppSimple.Dto;
using HrAppSimple.Interface;
using HrAppSimple.Models;
using HrAppSimple.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HrAppSimple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizatorController : ControllerBase
    {
        private readonly IConfiguration _congif;
        private readonly IUtilizatorRepository _utilizatorInterface;
        private readonly IMapper _mapper;


        public UtilizatorController(IUtilizatorRepository utilizatorInterface, IMapper mapper, IConfiguration congif)
        {
            _congif = congif;
            mapper = mapper;
            _utilizatorInterface = utilizatorInterface;
        }

        [HttpGet]
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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public IActionResult Createuser([FromBody] UtilizatorDto userCreate)
        {

            if (userCreate == null)
                return BadRequest(ModelState);

            var user = _utilizatorInterface.GetUtilizatori().Where(c => c.Nume.ToLower() == userCreate.Nume.TrimEnd().ToLower())
                .FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "E deja un user asa");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            user = new Utilizator();
            CreatePasswordHash(userCreate.Parola, out byte[] passwarodhash, out byte[] passwordsalt);
            user.Nume = userCreate.Nume;
            user.PasswordHash = passwarodhash;
            user.PasswordSalt = passwordsalt;
            user.IsAdmin = false;
            if (!_utilizatorInterface.CreateUtilizator(user))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Succes");
        }

        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult Updateuser(int id)
        {




            if (!_utilizatorInterface.UtilizatorExista(id))
            {
                return NotFound();
            }

            Utilizator user = _utilizatorInterface.GetUtilizator(id);
            user.IsAdmin = true;
            if (!_utilizatorInterface.UpdateUtilizator(user))
            {
                ModelState.AddModelError("", "IDK CEVA LA UPDATE");
                return StatusCode(500, ModelState);
            }

            return Ok("Suces");
        }

        [HttpDelete("{userid}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult Deleteuser(int id)
        {
            if (!_utilizatorInterface.UtilizatorExista(id)) { return NotFound(); }

            var usertodel = _utilizatorInterface.GetUtilizator(id);



            if (!_utilizatorInterface.DeleteUtilizator(usertodel))
            {
                ModelState.AddModelError("", "Nu am mers deletul frate");
                return StatusCode(500, ModelState);
            }

            return Ok("Succes");


        }
        [HttpPut("ToAdmin"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateUtilizatorAdmin(int id)
        {




            if (!_utilizatorInterface.UtilizatorExista(id))
            {
                return NotFound();
            }

            Utilizator user = _utilizatorInterface.GetUtilizator(id);
            user.IsAdmin = true;
            if (!_utilizatorInterface.UpdateUtilizator(user))
            {
                ModelState.AddModelError("", "Eroare update!");
                return StatusCode(500, ModelState);
            }

            return Ok("Suces");
        }

        [HttpPost("login")]
        public async Task<ActionResult<String>> Login(UtilizatorDto rq)
        {
            Utilizator user = _utilizatorInterface.GetUtilizator(rq.Id);

            if (user == null)
                return BadRequest("No user");
            if (user.Nume != rq.Nume)
                return BadRequest("UserNotFound");
            if (!Verify(rq.Parola, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Incorect Password");

            string token = string.Empty;
            if (user.IsAdmin)
                token = CreateTokenAdmin(rq);
            else
                token = CreateTokenUser(rq);
            return Ok(token);
        }


        private string CreateTokenAdmin(UtilizatorDto user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nume),
                new Claim(ClaimTypes.Role, "Admin")
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

        private string CreateTokenUser(UtilizatorDto user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nume),
                new Claim(ClaimTypes.Role, "User")
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