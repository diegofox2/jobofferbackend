using JobOfferBackend.ApplicationServices;
using JobOfferBackend.ApplicationServices.DTO;
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

        [HttpGet]
        [Route("GetNewJobOffer")]
        public async Task<JobOffer> GetNewJobOffer(string recruiterId)
        {
            return await _recruiterService.GetNewJobOffer(recruiterId);
        }


        [HttpPost]
        [Route("SaveJobOffer")]
        public async Task SaveJobOffer([FromBody] JobOffer jobOffer)
        {
            await _recruiterService.SaveJobOfferAsync(jobOffer);
        }

        [HttpPost]
        [Route("PublishJobOffer")]
        public async Task PublishJobOffer([FromBody] JobOffer jobOffer)
        {
            await _recruiterService.PublishJobOffer(jobOffer);
        }

        [HttpPost]
        [Route("CreateRecruiter")]
        public async Task CreateRecruiter([FromBody] Person person, IEnumerable<string> companies = null)
        {
            await _recruiterService.CreateRecruiterAsync(person, companies);
        }

        [HttpGet()]
        [Route("GetJobOffers")]
        public async Task<IEnumerable<JobOfferListDto>> GetJobOffers()
        {
            return await _recruiterService.GetAllJobOffersCreatedByAccountAsync(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "jta").Value);
        }
    }
}