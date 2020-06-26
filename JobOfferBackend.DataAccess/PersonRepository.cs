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

    }
}
