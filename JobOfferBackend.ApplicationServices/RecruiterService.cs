using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.ApplicationServices.DTO;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Constants;
using JobOfferBackend.Domain.Entities;
using System;
using System.Collections.Generic;
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
        private readonly PersonRepository _personRepository;
        private readonly AccountRepository _accountRepository;

        public RecruiterService(CompanyRepository companyRepository, RecruiterRepository recruiterRepository, JobOfferRepository jobOfferRepository, PersonRepository personRepository, AccountRepository accountRepository)
        {
            _companyRepository = companyRepository;
            _recruiterRepository = recruiterRepository;
            _jobOfferRepository = jobOfferRepository;
            _personRepository = personRepository;
            _accountRepository = accountRepository;
        }

        public virtual async Task<Recruiter> GetRecruiterAsync(Recruiter recruiter)
        {
            return await _recruiterRepository.GetByIdAsync(recruiter.Id);
        }

        public virtual async Task<IEnumerable<JobOfferListDto>> GetAllJobOffersCreatedByAccountAsync(string accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);

            if (account != null)
            {
                    var recruiterExists = await _recruiterRepository.CheckEntityExistsAsync(account.PersonId);

                    if (recruiterExists)
                    {
                        var jobOfferDtoList = new List<JobOfferListDto>();
                        var jobOffers = await _jobOfferRepository.GetAllJobOffersByRecruiter(account.PersonId);

                        jobOffers.ToList().ForEach(item =>
                        {
                            jobOfferDtoList.Add(new JobOfferListDto() { AlreadyApplied = false, JobOffer = item });
                        });


                        return jobOfferDtoList;
                    }
                    else
                    {
                        throw new InvalidOperationException(DomainErrorMessages.INVALID_RECRUITER);
                    }
             
            }
            else
            {
                throw new InvalidOperationException(DomainErrorMessages.ACCOUNT_DOES_NOT_EXISTS);
            }
        }

        public virtual async Task CreateRecruiterAsync(Recruiter recruiter)
        {
            recruiter.Validate();

            //This should be transactional and integration tests should validate all these repositories 

            await _personRepository.UpsertAsync(recruiter);
            await _recruiterRepository.UpsertAsync(recruiter);
            //
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

        public virtual async Task AddClientAsync(Company company, string recruiterId)
        {
            company.Validate();

            var existingCompany = await _companyRepository.GetCompanyAsync(company.Name, company.Activity);

            //This should be transactional

            if(existingCompany == null)
            {
                await _companyRepository.UpsertAsync(company);
            }

            var recruiter = await _recruiterRepository.GetByIdAsync(recruiterId);

            recruiter.AddClient(company);

            await _recruiterRepository.UpsertAsync(recruiter);

            /////////////////////////////////////////////////////////
        }

        public virtual async Task SaveJobOfferAsync(JobOffer jobOfferToSave, string recruiterId)
        {
            jobOfferToSave.Validate();

            var recruiterExists = await _recruiterRepository.CheckEntityExistsAsync(recruiterId);

            if(!recruiterExists)
            {
                throw new InvalidOperationException(DomainErrorMessages.INVALID_RECRUITER);
            }

            var companyExists = await _companyRepository.CheckEntityExistsAsync(jobOfferToSave.CompanyId);

            if (!companyExists)
            {
                throw new InvalidOperationException(DomainErrorMessages.COMPANY_INVALID);
            }

            if (!jobOfferToSave.HasIdCreated)
            {
                var jobOffersCreatedByRecruiter = await _jobOfferRepository.GetActiveJobOffersByRecruiterAsync(recruiterId);

                if (jobOffersCreatedByRecruiter.Any(j => j.CompanyId == jobOfferToSave.CompanyId && j.Title == jobOfferToSave.Title && j.State != JobOfferState.Finished))
                {
                    throw new InvalidOperationException(DomainErrorMessages.JOBOFFER_ALREADY_EXISTS);
                }

                jobOfferToSave.RecruiterId = recruiterId;

                jobOfferToSave.State = JobOfferState.Created;
            }
            else
            {
                if (!await _jobOfferRepository.JobOfferBelongsToRecruiter(jobOfferToSave, recruiterId))
                {
                    throw new InvalidOperationException(DomainErrorMessages.INVALID_RECRUITER);
                }
            }

            await _jobOfferRepository.UpsertAsync(jobOfferToSave);
        }

        public virtual  async Task PublishJobOffer(JobOffer jobOffer)
        {
            jobOffer.Publish();

            await _jobOfferRepository.UpsertAsync(jobOffer);
        }

        public virtual async Task FinishJobOffer(JobOffer jobOffer)
        {
            jobOffer.Finish();

            await _jobOfferRepository.UpsertAsync(jobOffer);
        }
    }
}
