using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Domain.Entities;
using JobOfferBackend.WebAPI.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPost]
        [Route("updatejoboffer")]
        public async Task UpdateJobOffer([FromBody] JobOffer jobOffer)
        {
            await _recruiterService.UpdateJobOffer(jobOffer, HttpContext.User.Claims.FirstOrDefault(c => c.Type == "jta").Value);
        }

        [HttpGet()]
        [Route("GetJobOffers")]
        public async Task<IEnumerable<JobOffer>> GetJobOffers()
        {
            return await _recruiterService.GetAllJobOffersCreatedByAccountAsync(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "jta").Value);
        }
    }
}