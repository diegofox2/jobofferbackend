using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Doman.Security.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(AccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        // GET api/account/signin
        [HttpPost()]
        [Route("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] Account account)
        {
            var accountId = await _accountService.SignInAsync(account.Email, account.Password);

            if(!string.IsNullOrEmpty(accountId))
            {
                return Ok(new { token = GenerarTokenJWT(account.Email, accountId) });
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET api/account/signup
        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public async Task SignUp( string email, string password, string confirmationPassword)
        {
            await _accountService.SignUpAsync(email, password, confirmationPassword);
        }

        private string GenerarTokenJWT(string email, string accountId)
        {
            
            var symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                );
            var signingCredentials = new SigningCredentials(
                    symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("jta", accountId),
            };

            var token = new JwtSecurityToken(_configuration["JWT:Issuer"], _configuration["JWT:Audience"], claims, DateTime.Now, DateTime.UtcNow.AddMinutes(30), signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}