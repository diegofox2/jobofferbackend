using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Doman.Security.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            var currentAccount = await _accountService.SignInAsync(account.Email, account.Password);

            if(currentAccount != null && !string.IsNullOrEmpty(currentAccount.Id))
            {
                return Ok(new { token = GenerarTokenJWT(currentAccount.Email, currentAccount.Id), isRecruiter = currentAccount.IsRecruiter });
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
        public async Task SignUp( string email, string password, string confirmationPassword, bool isRecruiter)
        {
            await _accountService.SignUpAsync(email, password, confirmationPassword, isRecruiter);
        }

        [HttpPost]
        [Route("validatetoken")]
        public async Task<IActionResult> ValidateToken()
        {
            var isRecruiter = await _accountService.UserIsRecruiter(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "jta").Value);
            //This method only can be executed if the token was previously accepted by the security framework
            return Ok(new { isRecruiter });
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

            var token = new JwtSecurityToken(_configuration["JWT:Issuer"], _configuration["JWT:Audience"], claims, DateTime.Now, DateTime.UtcNow.AddHours(3), signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}