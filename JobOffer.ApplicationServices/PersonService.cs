using JobOffer.ApplicationServices.Constants;
using JobOffer.DataAccess;
using JobOffer.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace JobOffer.ApplicationServices
{
    public class PersonService
    {
        private readonly PersonRepository _personRepository;

        public PersonService(PersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<Person> GetPersonAsync(Person person)
        {
            return await _personRepository.GetByIdAsync(person.Id);
        }

        public async Task CreatePersonAsync(Person person)
        {
            person.Validate();

            await _personRepository.UpsertAsync(person);
        }

        public async Task UpdatePersonAsync(Person recruiter)
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
    }
}
