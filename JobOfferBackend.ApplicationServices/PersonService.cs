﻿using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices
{
    /// <summary>
    /// Intentionally it doesn't implements an Interface because it could create a fake abstraction
    /// See https://medium.com/@dcamacho31/foreword-224a02be04f8
    /// </summary>
    public class PersonService
    {
        private readonly PersonRepository _personRepository;
        private readonly JobOfferRepository _jobOfferRepository;
        private readonly AccountRepository _accountRepository;

        public PersonService(PersonRepository personRepository, JobOfferRepository jobOfferRepository, AccountRepository accountRepository)
        {
            _personRepository = personRepository;
            _jobOfferRepository = jobOfferRepository;
            _accountRepository = accountRepository;
        }

        public virtual async Task<Person> GetPersonByIdAsync(string personId)
        {
            return await _personRepository.GetByIdAsync(personId);
        }

        public virtual async Task CreatePersonAsync(Person person)
        {
            person.Validate();

            await _personRepository.UpsertAsync(person);
        }

        public virtual async Task UpdatePersonAsync(Person recruiter)
        {
            recruiter.Validate();

            if (await _personRepository.CheckEntityExistsAsync(recruiter.Id))
            {
                await _personRepository.UpsertAsync(recruiter);
            }
            else
            {
                throw new InvalidOperationException(ServicesErrorMessages.PERSON_DOES_NOT_EXISTS);
            }
        }

        public virtual async Task ApplyToJobOfferAsync(string jobOfferId, string accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);

            if (account != null)
            {
                var jobOffer = await _jobOfferRepository.GetByIdAsync(jobOfferId);

                if (jobOffer != null)
                {
                    var person = await _personRepository.GetByIdAsync(account.PersonId);

                    if (person != null)
                    {
                        person.ApplyToJobOffer(jobOffer);

                        await _jobOfferRepository.UpsertAsync(jobOffer);

                        await _personRepository.UpsertAsync(person);
                    }
                    else
                    {
                        throw new InvalidOperationException(ServicesErrorMessages.PERSON_DOES_NOT_EXISTS);
                    }

                }
                else
                {
                    throw new InvalidOperationException(ServicesErrorMessages.INVALID_JOB_OFFER);
                }
                
            }
            else
            {
                throw new InvalidOperationException(ServicesErrorMessages.INVALID_USER_ACCOUNT);
            }
        }
    }
}
