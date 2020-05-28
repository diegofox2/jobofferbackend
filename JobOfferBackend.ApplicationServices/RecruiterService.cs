using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Constants;
using JobOfferBackend.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices
{
    /// <summary>
    /// Intentionally it doesn't implements an Interface because it could create a fake abstraction
    /// See https://medium.com/@dcamacho31/foreword-224a02be04f8
    /// </summary>
    public class RecruiterService
    {
        private readonly CompanyRepository _companyRepository;
        private readonly RecruiterRepository _recruiterRepository;
        private readonly JobOfferRepository _jobOfferRepository;

        public RecruiterService(CompanyRepository companyRepository, RecruiterRepository recruiterRepository, JobOfferRepository jobOfferRepository)
        {
            _companyRepository = companyRepository;
            _recruiterRepository = recruiterRepository;
            _jobOfferRepository = jobOfferRepository;
        }

        public virtual async Task<Recruiter> GetRecruiterAsync(Recruiter recruiter)
        {
            return await _recruiterRepository.GetByIdAsync(recruiter.Id);
        }


        public virtual async Task CreateRecruiterAsync(Recruiter recruiter)
        {
            recruiter.Validate();

            recruiter.ClientCompanies.ToList().ForEach(async company =>
            {
                if (!company.HasIdCreated)
                {
                    await _companyRepository.UpsertAsync(company);
                }
            });

            await _recruiterRepository.UpsertAsync(recruiter);
        }

        public virtual async Task UpdateRecruiterAsync(Recruiter recruiter)
        {
            recruiter.Validate();

            if (await _recruiterRepository.CheckEntityExistsAsync(recruiter.Id))
            {
                await _recruiterRepository.UpsertAsync(recruiter);
            }
            else
            {
                throw new InvalidOperationException(ServicesErrorMessages.RECRUITER_DOES_NOT_EXISTS);
            }
        }

        public virtual async Task CreateCompanyAsync(Company company)
        {
            company.Validate();

            var existingCompany = await _companyRepository.GetCompanyAsync(company.Name, company.Activity);

            if (existingCompany != null)
            {
                throw new InvalidOperationException(ServicesErrorMessages.COMPANY_ALREADY_EXISTS);
            }
            else
            {
                await _companyRepository.UpsertAsync(company);
            }
        }

        public virtual async Task CreateJobOfferAsync(JobOffer jobOffer, string recruiterId)
        {
            
            
            jobOffer.Validate();

            if(jobOffer.HasIdCreated)
            {
                throw new InvalidOperationException(DomainErrorMessages.JOBOFFER_ALREADY_EXISTS);
            }

            var recruiter = await _recruiterRepository.GetByIdAsync(recruiterId);

            var jobOffersCreatedByRecruiter = await _jobOfferRepository.GetActiveJobOffers(recruiter);

            if (jobOffersCreatedByRecruiter.Any(j => j.Company == jobOffer.Company && j.Title == jobOffer.Title))
            {
                throw new InvalidOperationException(DomainErrorMessages.JOBOFFER_ALREADY_EXISTS);
            }

            jobOffer.Owner = recruiter;

            jobOffer.IsActive = true;

            await _jobOfferRepository.UpsertAsync(jobOffer);
        }
    }
}
