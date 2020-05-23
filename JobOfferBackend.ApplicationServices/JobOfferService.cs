using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices
{
    public class JobOfferService
    {
        private readonly JobOfferRepository _jobOfferRepository;

        public JobOfferService(JobOfferRepository jobOfferRepository)
        {
            _jobOfferRepository = jobOfferRepository;
        }

        public async Task<IEnumerable<JobOffer>> GetJobOffersAsync()
        {
            return await _jobOfferRepository.GetActiveJobOffersAsync();
        }

    }
}
