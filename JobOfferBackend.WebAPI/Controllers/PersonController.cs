using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : ControllerBase
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }


        [HttpPost]
        [Route("ApplyToJobOffer/{jobOfferId}")]
        public async Task ApplyToJobOffer(string jobOfferId)
        {
            await _personService.ApplyToJobOfferAsync(jobOfferId, HttpContext.User.Claims.FirstOrDefault(c=> c.Type == "jta").Value);
        }

        [HttpPost]
        [Route("CreatePerson")]
        public async Task CreatePerson([FromBody] Person person)
        {
            await _personService.CreatePersonAsync(person);
        }
    }
}
