using JobOfferBackend.ApplicationServices;
using JobOfferBackend.ApplicationServices.DTO;
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

        public JobsOfferController(JobOfferService jobOfferService)
        {
            _jobOfferService = jobOfferService;
        }

        // GET api/jobsoffer
        [HttpGet()]
        public async Task<IEnumerable<JobOfferListDto>> Get(string accountId)
        {
            return await _jobOfferService.GetJobOffersAsync(accountId);
        }

        // GET api/jobsoffer/5as55d4bvbv658aaer897bv
        [HttpGet("{id}")]
        public async Task<JobOffer> GetById(string id)
        {
            return await _jobOfferService.GetJobOffer(id);
        }
    }
}
