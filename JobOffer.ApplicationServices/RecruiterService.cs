using JobOffer.ApplicationServices.Constants;
using JobOffer.DataAccess;
using JobOffer.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace JobOffer.ApplicationServices
{
    public class RecruiterService
    {
        private readonly CompanyRepository _companyRepository;
        private readonly RecruiterRepository _recruiterRepository;

        public RecruiterService(CompanyRepository companyRepository, RecruiterRepository recruiterRepository)
        {
            _companyRepository = companyRepository;
            _recruiterRepository = recruiterRepository;
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
    }
}
