using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobOfferBackend.Domain.Entities
{
    public class Recruiter : Person, IIdentity<Recruiter>
    {
        private List<Company> _clientCompanies = new List<Company>();

        public IEnumerable<Company> ClientCompanies { get => _clientCompanies; set => _clientCompanies = (List<Company>)value; }


        public void AddClient(Company company)
        {
            if (_clientCompanies.Any(item => item.Name == company.Name))
            {
                throw new InvalidOperationException(DomainErrorMessages.COMPANY_REPEATED);
            }

            _clientCompanies.Add(company);
        }

        public void OfferEmployment(JobOffer jobOffer, Person person)
        {
            CheckPersonSkills(person, jobOffer);

            jobOffer.AddJobApplicationOffered(person);
        }

        public void AcceptApplicant(Person person, JobOffer jobOffer)
        {
            CheckPersonSkills(person, jobOffer);

            jobOffer.SetJobApplicationAccepted(person);
            
        }

        private void CheckPersonSkills(Person person, JobOffer jobOffer)
        {
            var mandatorySkills = jobOffer.SkillsRequired.Where(s => s.IsMandatory).Select(s => s.Skill);

            var personSkills = person.Abilities.Select(h => h.Skill);

            if (!mandatorySkills.All(ms => personSkills.Any(ps => ps.Id == ms.Id)))
            {
                throw new InvalidOperationException(DomainErrorMessages.PERSON_DOES_NOT_HAVE_ALL_MANDATORY_SKILLS);
            }
        }

    }
}
