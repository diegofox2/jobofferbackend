using JobOffer.DataAccess;
using JobOffer.Domain.Entities;
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

        public async Task<Recruiter> GetRecruiterAsync(Person person)
        {
            return await _recruiterRepository.GetByIdAsync(person.Id);
        }


        public async Task CreateRecruiter(Recruiter recruiter)
        {
            recruiter.Validate();

            await _recruiterRepository.UpsertAsync(recruiter);
        }

        public async Task CreateCompany(Company company)
        {
            company.Validate();

            await _companyRepository.UpsertAsync(company);
        }
    }
}
