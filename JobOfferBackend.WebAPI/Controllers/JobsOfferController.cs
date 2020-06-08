using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsOfferController : ControllerBase
    {
        private readonly JobOfferService _jobOfferService;
        private readonly RecruiterService _recruiterService;

        public JobsOfferController(JobOfferService jobOfferService, RecruiterService recruiterService)
        {
            _jobOfferService = jobOfferService;
            _recruiterService = recruiterService;
        }

        // GET api/jobsoffer
        [HttpGet]
        public async Task<IEnumerable<JobOffer>> Get()
        {
            return await _jobOfferService.GetJobOffersAsync();
        }

        // GET api/jobsoffer/5as55d4bvbv658aaer897bv
        [HttpGet("{id}")]
        public async Task<JobOffer> Get(string id)
        {
            return await _jobOfferService.GetJobOffer(id);
        }
    }
}
