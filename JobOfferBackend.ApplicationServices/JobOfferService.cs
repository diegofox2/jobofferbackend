using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.ApplicationServices.DTO;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices
{
    public class JobOfferService
    {
        private readonly JobOfferRepository _jobOfferRepository;
        private readonly PersonRepository _personRepository;

        public JobOfferService(JobOfferRepository jobOfferRepository, PersonRepository personRepository)
        {
            _jobOfferRepository = jobOfferRepository;
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<JobOfferListDto>> GetJobOffersAsync(string personId)
        {
            var jobOffers = await _jobOfferRepository.GetAllPublishedJobOffersAsync();

            var jobOfferListDto = new List<JobOfferListDto>();

            if (!string.IsNullOrEmpty(personId))
            {
                var person = await _personRepository.GetByIdAsync(personId);

                if (person == null)
                {
                    throw new InvalidOperationException(ServicesErrorMessages.PERSON_DOES_NOT_EXISTS);
                }

                jobOffers.ForEach(jobOffer =>
                {
                    if (person.MyJobApplications.Contains(jobOffer.Id))
                    {
                        jobOfferListDto.Add(new JobOfferListDto() { JobOffer = jobOffer, AlreadyApplied = true });
                    }
                    else
                    {
                        jobOfferListDto.Add(new JobOfferListDto() { JobOffer = jobOffer});
                    }
                });
            }
            else
            {
                jobOffers.ForEach(jobOffer =>
                {
                   jobOfferListDto.Add(new JobOfferListDto() { JobOffer = jobOffer });
                   
                });
            }

            return jobOfferListDto;
        }

        public async Task<JobOffer> GetJobOfferAsync(string id)
        {
            return await _jobOfferRepository.GetByIdAsync(id);
        }

    }
}
