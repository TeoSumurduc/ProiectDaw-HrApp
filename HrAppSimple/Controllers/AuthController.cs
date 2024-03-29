﻿using HrAppSimple.Dto;
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

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if(!utilizator.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if(utilizator.TokenExpires<DateTime.Now)
            {
                return Unauthorized("Token expired");
            }

            string token = CreateToken(utilizator);

            var newRefreshToke = GenerateRefreshToken();

            SetRefreshToken(newRefreshToke);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expire = DateTime.Now.AddDays(7)
                };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expire,

            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            utilizator.RefreshToken = newRefreshToken.Token;
            utilizator.TokenExpires = newRefreshToken.Expire;
            utilizator.TokenCreated = newRefreshToken.Created;
        }

        private string CreateToken(Utilizator utilizator)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,utilizator.Nume),
                new Claim(ClaimTypes.Role,"Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
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
