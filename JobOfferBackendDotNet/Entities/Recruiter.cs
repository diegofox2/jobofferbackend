using JobOffer.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobOffer.Domain.Entities
{
    public class Recruiter : Person, IIdentity<Recruiter>
    {
        private List<Company> _clientCompanies = new List<Company>();

        public IEnumerable<Company> ClientCompanies { get => _clientCompanies; set => _clientCompanies = (List<Company>)value; }


        public void AddClientCompany(Company company)
        {
            if (_clientCompanies.Any(item => item.Name == company.Name))
            {
                throw new InvalidOperationException(DomainErrorMessages.COMPANY_REPEATED);
            }

            _clientCompanies.Add(company);
        }
    }
}
