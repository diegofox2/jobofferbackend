using JobOfferBackend.ApplicationServices;
using JobOfferBackend.ApplicationServices.DTO;
using JobOfferBackend.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOfferBackend.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class JobsOfferController : ControllerBase
    {
        private readonly JobOfferService _jobOfferService;
        private readonly SkillsService _skillsService;

        public JobsOfferController(JobOfferService jobOfferService, SkillsService skillsService)
        {
            _jobOfferService = jobOfferService;
            _skillsService = skillsService;
        }

        // GET api/jobsoffer
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IEnumerable<JobOfferListDto>> Get(string accountId)
        {
            return await _jobOfferService.GetJobOffersAsync(accountId);
        }

        // GET api/jobsoffer/5as55d4bvbv658aaer897bv
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<JobOffer> GetById(string id)
        {
            return await _jobOfferService.GetJobOffer(id);
        }

        [HttpGet("getskills")]
        public async Task<IEnumerable<Skill>> GetSkills()
        {
            return await _skillsService.GetAllSkillsAsync();
        }

    }
}
