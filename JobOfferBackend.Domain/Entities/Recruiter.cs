using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobOfferBackend.Domain.Entities
{
    public class Recruiter : Person, IIdentity<Recruiter>
    {
        private List<string> _clientCompaniesId = new List<string>();

        public IEnumerable<string> ClientCompanies { get => _clientCompaniesId; set => _clientCompaniesId = (List<string>)value; }

        public void AddClient(Company company)
        {
            if (_clientCompaniesId.Any(item => item == company.Id))
            {
                throw new InvalidOperationException(DomainErrorMessages.COMPANY_REPEATED);
            }

            _clientCompaniesId.Add(company.Id);
        }

        public void OfferEmployment(JobOffer jobOffer, Person person)
        {
            CheckPersonSkillsAndYearsOfExperience(person, jobOffer);

            jobOffer.AddJobApplicationOffered(person);
        }

        public void AcceptApplicant(Person person, JobOffer jobOffer)
        {
            CheckPersonSkillsAndYearsOfExperience(person, jobOffer);

            jobOffer.SetJobApplicationAccepted(person);
            
        }

        private void CheckPersonSkillsAndYearsOfExperience(Person person, JobOffer jobOffer)
        {
            var mandatorySkills = jobOffer.SkillsRequired.Where(s => s.IsMandatory);

            if (!mandatorySkills.All(mandatorySkill => person.Abilities.Any(ability => ability.SkillId == mandatorySkill.SkillId && ability.Years >= mandatorySkill.Years )))
            {
                throw new InvalidOperationException(DomainErrorMessages.PERSON_DOES_NOT_HAVE_ALL_MANDATORY_SKILLS);
            }
        }

    }
}
