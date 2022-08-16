using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly DataContextSql _db;

        public AuthenticationController(IConfiguration configuration, SignInManager<User> signInManager, DataContextSql db)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _db = db;
        }

        [HttpPost("api/Authenticate")]
        public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticationRequestBody authenticationRequestBody)
        {

            // Step1: Walidacja danych logowania
            var user = await _db.Users
                .Where(u => u.UserName == authenticationRequestBody.UserName)
                .FirstOrDefaultAsync();

            if (user == null)
                return Unauthorized();

            var validationResult = await _signInManager.CheckPasswordSignInAsync(user, authenticationRequestBody.Password, false);

            if (!validationResult.Succeeded)
            {
                return Forbid();
            }

            // Step2: Tworzenie kluczy tokenu
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            // Step3: Tworzenie Claimów
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Email.ToString()));

            // Step4: Tworzenie tokenu
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return tokenToReturn;
        }
    }
}