using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobOfferBackend.WebAPI.Controllers
{
    public class ApplyToJobOfferRequest
    {
        public string jobOfferId { get; set; }

        public string token { get; set; }

        public string user { get; set; }
    }


    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }


        [HttpPost]
        [Route("ApplyToJobOffer")]
        public async Task ApplyToJobOffer([FromBody] ApplyToJobOfferRequest request)
        {
            await _personService.ApplyToJobOfferAsync(request.token, request.jobOfferId, request.user);
        }

        [HttpPost]
        [Route("CreatePerson")]
        public async Task CreatePerson([FromBody] Person person)
        {
            await _personService.CreatePersonAsync(person);
        }
    }
}
