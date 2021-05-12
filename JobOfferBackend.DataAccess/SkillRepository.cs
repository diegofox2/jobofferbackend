using JobOfferBackend.Domain.Constants;
using JobOfferBackend.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOfferBackend.DataAccess
{
    public class SkillRepository : BaseRepository<Skill>
    {
        public SkillRepository(IMongoDatabase database) : base(database)
        {
        }

        public virtual async Task<bool> SkillNameDoesNotExistsAsync(Skill skill)
        {
            return await Collection.CountDocumentsAsync(item => item.Name == skill.Name) == 0;
        }

        public override async Task UpsertAsync(Skill entity)
        {
            if (await SkillNameDoesNotExistsAsync(entity))
            {
                await base.UpsertAsync(entity);
            }
            else
            {
                throw new InvalidOperationException(DomainErrorMessages.SKILL_NAME_ALREADY_EXISTS);
            }
        }
    }
}
