using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices
{
    public class CompaniesService
    {
        private readonly CompanyRepository _companyRepository;

        public CompaniesService(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await _companyRepository.GetAllAsync();
        }
    }
}
