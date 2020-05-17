using JobOffer.Domain.Entities;
using MongoDB.Driver;

namespace JobOffer.DataAccess
{
    public class PersonRepository: BaseRepository<Person>
    {
        public PersonRepository(IMongoDatabase database):base(database)
        {
        }
    }
}
