using JobOffer.Domain.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOffer.DataAccess
{
    public class SkillRepository : BaseRepository<Skill>
    {
        public SkillRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
