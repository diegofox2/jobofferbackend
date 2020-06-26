using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruiterController : ControllerBase
    {
        private readonly RecruiterService _recruiterService;

        public RecruiterController(RecruiterService  recruiterService)
        {
            _recruiterService = recruiterService;
        }

        [HttpPost]
        [Route("AddJobOffer")]
        public async Task CreateJobOffer([FromBody] JobOffer jobOffer, string recruiterId)
        {
            await _recruiterService.CreateJobOfferAsync(jobOffer, recruiterId);
        }

        [HttpPost]
        [Route("CreateRecruiter")]
        public async Task CreateRecruiter([FromBody] Recruiter recruiter)
        {
            await _recruiterService.CreateRecruiterAsync(recruiter);
        }
    }
}