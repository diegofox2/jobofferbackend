﻿using JobOfferBackend.Domain.Entities;
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

        public virtual async Task<List<JobOffer>> GetActiveJobOffersByRecruiterAsync(Recruiter recruiter)
        {
            return await Collection.Find(item => item.State !=  JobOfferState.Finished && item.Recruiter == recruiter).ToListAsync();
        }

        public virtual async Task<IEnumerable<JobOffer>> GetAllJobOffersByRecruiter(Recruiter recruiter)
        {
            return await Collection.Find(item => item.Recruiter == recruiter).ToListAsync();
        }

        public virtual async Task<List<JobOffer>> GetAllPublishedJobOffersAsync()
        {
            return await Collection.Find(item => item.State != JobOfferState.Finished).ToListAsync();
        }
    }
}
