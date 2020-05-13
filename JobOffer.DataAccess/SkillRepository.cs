using JobOffer.Domain.Entities;
using MongoDB.Driver;

namespace JobOffer.DataAccess
{
    public class SkillRepository : BaseRepository<Skill>
    {
        public SkillRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
