using JobOfferBackend.Domain.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOfferBackend.DataAccess
{
    public class JobOfferRepository : BaseRepository<JobOffer>
    {
        public JobOfferRepository(IMongoDatabase database) : base(database)
        {
        }

        public virtual async Task<List<JobOffer>> GetActiveJobOffersByRecruiterAsync(string recruiterId)
        {
            return await Collection.Find(item => item.State !=  JobOfferState.Finished && item.RecruiterId == recruiterId).ToListAsync();
        }

        public virtual async Task<IEnumerable<JobOffer>> GetAllJobOffersByRecruiter(string recruiterId)
        {
            return await Collection.Find(item => item.RecruiterId == recruiterId).ToListAsync();
        }

        public virtual async Task<List<JobOffer>> GetAllPublishedJobOffersAsync()
        {
            return await Collection.Find(item => item.State != JobOfferState.Finished).ToListAsync();
        }

        public virtual async Task<bool> JobOfferBelongsToRecruiter(JobOffer jobOffer, string recruiterId)
        {
           return await Collection.CountDocumentsAsync(item => item.RecruiterId == recruiterId && item.Id == jobOffer.Id) == 1;
        }
    }
}
