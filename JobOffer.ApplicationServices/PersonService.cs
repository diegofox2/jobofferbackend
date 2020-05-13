using JobOffer.DataAccess;
using JobOffer.Domain.Entities;
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
    }
}
