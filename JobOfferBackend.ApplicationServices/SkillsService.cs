using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices
{
    public class SkillsService
    {
        private readonly SkillRepository _skillRepository;

        public SkillsService(SkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            return await _skillRepository.GetAll();
        }
    }
}
