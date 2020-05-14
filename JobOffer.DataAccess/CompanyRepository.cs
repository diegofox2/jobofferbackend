using JobOffer.Domain.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace JobOffer.DataAccess
{
    public class CompanyRepository : BaseRepository<Company>
    {
        public CompanyRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<Company> GetCompanyAsync (string companyName, string activity)
        {
            return await Collection.Find(item => item.Name == companyName && item.Activity == activity).SingleOrDefaultAsync();
        }
    }
}
