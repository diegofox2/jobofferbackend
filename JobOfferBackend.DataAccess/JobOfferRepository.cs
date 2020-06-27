using JobOfferBackend.Domain.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.DataAccess
{
    public class JobOfferRepository : BaseRepository<JobOffer>
    {
        public JobOfferRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<List<JobOffer>> GetActiveJobOffers(Recruiter recruiter)
        {
            return await Collection.Find(item => item.IsActive == true && item.Recruiter == recruiter).ToListAsync();
        }

        public async Task<List<JobOffer>> GetActiveJobOffersAsync()
        {
            return await Collection.Find(item => item.IsActive == true).ToListAsync();
        }


        public override async Task<ReplaceOneResult> UpsertAsync(JobOffer entity)
        {
            return await base.UpsertAsync(entity);
        }
    }
}
