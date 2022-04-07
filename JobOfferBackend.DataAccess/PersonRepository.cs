using JobOfferBackend.Domain.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace JobOfferBackend.DataAccess
{
    public class PersonRepository: BaseRepository<Person>
    {
        public PersonRepository(IMongoDatabase database):base(database)
        {
        }

        public virtual async Task<Person> GetByIdentityCardAsync(string identityCard)
        {
            return await Collection.Find(p => p.IdentityCard == identityCard).SingleOrDefaultAsync();
        }

    }
}
