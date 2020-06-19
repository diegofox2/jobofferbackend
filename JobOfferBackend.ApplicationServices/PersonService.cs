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

        public PersonService(PersonRepository personRepository, JobOfferRepository jobOfferRepository)
        {
            _personRepository = personRepository;
            _jobOfferRepository = jobOfferRepository;
        }

        public virtual async Task<Person> GetPersonAsync(Person person)
        {
            return await _personRepository.GetByIdAsync(person.Id);
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

        public virtual async Task ApplyToJobOffer(Person person, JobOffer jobOffer)
        {
            person.Validate();

            jobOffer.Validate();

            person.ApplyToJobOffer(jobOffer);

            await _jobOfferRepository.UpsertAsync(jobOffer);
        }
    }
}
