using JobOfferBackend.Domain.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOfferBackend.DataAccess
{
    public class SkillRepository : BaseRepository<Skill>
    {
        public SkillRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
