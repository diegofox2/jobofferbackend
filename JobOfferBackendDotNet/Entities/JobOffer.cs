using JobOffer.Domain.Base;
using JobOffer.Domain.Constants;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobOffer.Domain.Entities
{
    public class JobOffer : BaseEntity<JobOffer>
    {
        private List<Application> _applications = new List<Application>();
        private List<SkillRequired> _skillsRequired = new List<SkillRequired>();

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<Application> Applications { get => _applications; set => _applications = (List<Application>)value; }

        public Company Company { get; set; }

        public Recruiter Owner { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<SkillRequired> SkillsRequired { get => _skillsRequired; set => _skillsRequired = (List<SkillRequired>)value; }

        public void AddSkillRequired(SkillRequired skill)
        {
            if(_skillsRequired.Any(s => s == skill))
            {
                throw new InvalidOperationException(DomainErrorMessages.SKILL_REQUIRED_ALREADY_EXISTS);
            }

            skill.Validate();

            _skillsRequired.Add(skill);
        }

        public void RecieveApplicant(Person person)
        {
            if(_applications.Any(a=> a.Applicant == person))
            {
                throw new InvalidOperationException(DomainErrorMessages.APPLICANT_ALREADY_EXISTS);
            }

            var application = new Application(person, DateTime.Now.Date);

            _applications.Add(application);
        }

        public override void Validate()
        {
            
        }
    }
}
