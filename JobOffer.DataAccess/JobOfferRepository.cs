using JobOffer.Domain.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOffer.DataAccess
{
    public class JobOfferRepository : BaseRepository<Domain.Entities.JobOffer>
    {
        public JobOfferRepository(IMongoDatabase database) : base(database)
        {
        }

        public async Task<List<Domain.Entities.JobOffer>> GetActiveJobOffer(Recruiter recruiter)
        {
            return await Collection.Find(item => item.IsActive == true && item.Owner == recruiter).ToListAsync();
        }

        public override async Task<ReplaceOneResult> UpsertAsync(Domain.Entities.JobOffer entity)
        {
            entity.Applications.ToList().ForEach(app =>
            {
                if(!app.HasIdCreated)
                {
                    app.Id = CreateId();
                }
            });

            return await base.UpsertAsync(entity);
        }
    }
}
