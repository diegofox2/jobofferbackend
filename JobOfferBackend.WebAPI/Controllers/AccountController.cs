using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Doman.Security.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        // GET api/account/signin
        [HttpPost()]
        [Route("signin")]
        public async Task<string> SignIn([FromBody] Account account)
        {
            return await _accountService.SignInAsync(account.Email, account.Password);
        }

        // GET api/account/signup
        [HttpPost]
        [Route("signup")]
        public async Task SignUp( string email, string password, string confirmationPassword)
        {
            await _accountService.SignUpAsync(email, password, confirmationPassword);
        }
    }
}