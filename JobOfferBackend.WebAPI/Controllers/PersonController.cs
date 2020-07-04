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
    public class ApplyToJobOfferRequest
    {
        public string JobOfferId { get; set; }

        public string User { get; set; }
    }


    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonController : Controller
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }


        [HttpPost]
        [Route("ApplyToJobOffer")]
        public async Task ApplyToJobOffer([FromBody] ApplyToJobOfferRequest request )
        {
            await _personService.ApplyToJobOfferAsync(request.JobOfferId, HttpContext.User.Claims.FirstOrDefault(c=> c.Type == "AccountId").Value, request.User);
        }

        [HttpPost]
        [Route("CreatePerson")]
        public async Task CreatePerson([FromBody] Person person)
        {
            await _personService.CreatePersonAsync(person);
        }
    }
}
