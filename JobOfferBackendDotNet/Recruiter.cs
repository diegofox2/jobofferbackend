using System;
using System.Collections.Generic;
using System.Linq;

namespace JobOffer.Domain
{
    public class Recruiter : Person
    {
        private List<Company> _companies = new List<Company>();

        public IEnumerable<Company> Companies { get => _companies; set => _companies = (List<Company>)value; }

        public void CreateJobOffer()
        {

        }

        public void AddCompany(Company company)
        {
            if (_companies.Any(item => item.Name == company.Name))
                throw new InvalidOperationException("COMPANY_REPEATED");

            _companies.Add(company);
        }
    }
}
