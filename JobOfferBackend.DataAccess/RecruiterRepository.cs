using JobOfferBackend.Domain.Entities;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.DataAccess
{
    public class RecruiterRepository : BaseRepository<Recruiter>
    {
        public RecruiterRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<Person> GetByIdentityCardAsync(string identityCard)
        {
            return await Collection.Find(p => p.IdentityCard == identityCard).SingleOrDefaultAsync();
        }

    }
}
