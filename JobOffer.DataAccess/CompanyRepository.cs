using JobOffer.Domain;
using MongoDB.Driver;

namespace JobOffer.DataAccess
{
    public class CompanyRepository : BaseRepository<Company>
    {
        public CompanyRepository(IMongoDatabase database) : base(database)
        {
        }
    }
}
