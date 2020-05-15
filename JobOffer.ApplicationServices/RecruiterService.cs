using JobOffer.ApplicationServices.Constants;
using JobOffer.DataAccess;
using JobOffer.Domain.Constants;
using JobOffer.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JobOffer.ApplicationServices
{
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

        public async Task<Recruiter> GetRecruiterAsync(Recruiter recruiter)
        {
            return await _recruiterRepository.GetByIdAsync(recruiter.Id);
        }


        public async Task CreateRecruiterAsync(Recruiter recruiter)
        {
            recruiter.Validate();

            await _recruiterRepository.UpsertAsync(recruiter);
        }

        public async Task UpdateRecruiterAsync(Recruiter recruiter)
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

        public async Task CreateCompanyAsync(Company company)
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

        public async Task CreateJobOffer(JobOffer.Domain.Entities.JobOffer jobOffer, Recruiter recruiter)
        {
            jobOffer.Validate();

            if(jobOffer.HasIdCreated)
            {
                throw new InvalidOperationException(DomainErrorMessages.JOBOFFER_ALREADY_EXISTS);
            }
            
            var jobOffersCreatedByRecruiter = await _jobOfferRepository.GetActiveJobOffer(recruiter);

            if (jobOffersCreatedByRecruiter.Any(j => j.Company == jobOffer.Company && j.Title == jobOffer.Title))
            {
                throw new InvalidOperationException(DomainErrorMessages.JOBOFFER_ALREADY_EXISTS);
            }

            await _jobOfferRepository.UpsertAsync(jobOffer);
        }
    }
}
