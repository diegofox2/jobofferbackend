﻿using JobOfferBackend.ApplicationServices;
using JobOfferBackend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOfferController : ControllerBase
    {
        private readonly JobOfferService _jobOfferService;
        private readonly RecruiterService _recruiterService;

        public JobOfferController(JobOfferService jobOfferService, RecruiterService recruiterService)
        {
            _jobOfferService = jobOfferService;
            _recruiterService = recruiterService;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<JobOffer>> Get()
        {
            return await _jobOfferService.GetJobOffersAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
    }
}
