using JobOfferBackend.ApplicationServices;
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
    public class SkillController : ControllerBase
    {
        private readonly SkillsService _skillsService;

        public SkillController(SkillsService skillsService)
        {
            _skillsService = skillsService;
        }

        [HttpGet("getall")]
        public async Task<IEnumerable<Skill>> GetSkills()
        {
            return await _skillsService.GetAllSkillsAsync();
        }

        [HttpPost("create")]
        public async Task Create([FromBody] Skill skill)
        {
            await _skillsService.CreateSkill(skill);
        }
    }
}
