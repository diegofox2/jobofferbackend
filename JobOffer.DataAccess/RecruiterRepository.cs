using JobOffer.Domain.Entities;
using MongoDB.Driver;

namespace JobOffer.DataAccess
{
    public class RecruiterRepository : BaseRepository<Recruiter>
    {
        public RecruiterRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
