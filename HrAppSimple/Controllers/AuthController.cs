using HrAppSimple.Dto;
using HrAppSimple.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HrAppSimple.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static Utilizator  utilizator= new Utilizator();
        private IConfiguration _configuration;

        public AuthController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Utilizator>> Register(UtilizatorDto request)
        {
            CreatePasswordHash(request.Parola, out byte[] passwordHash, out byte[] passordSalt);
            utilizator.Nume = request.Nume;
            utilizator.PasswordHash = passwordHash;
            utilizator.PasswordSalt = passordSalt;

            return Ok(utilizator);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UtilizatorDto request)
        {
            if(utilizator.Nume != request.Nume) 
            {
                return BadRequest("Utilizator not found!");
            }

            if(!VerifyPasswordHash(request.Parola, utilizator.PasswordHash, utilizator.PasswordSalt))
            {
                return BadRequest("Parola gresita!");
            }

            string token = CreateToken(utilizator);
            return Ok(token);
        }

        private string CreateToken(Utilizator utilizator)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,utilizator.Nume)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
               
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[]passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac= new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
