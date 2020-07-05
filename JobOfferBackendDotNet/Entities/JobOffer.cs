using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobOfferBackend.Domain.Entities
{
    public enum JobOfferState
    {
        Created,
        Published,
        Finished
    }

    public class JobOffer : BaseEntity<JobOffer>
    {
        private List<JobApplication> _applications = new List<JobApplication>();
        private List<SkillRequired> _skillsRequired = new List<SkillRequired>();

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<JobApplication> Applications { get => _applications; set => _applications = (List<JobApplication>)value; }

        public Company Company { get; set; }

        public Recruiter Recruiter { get; set; }

        public JobOfferState State { get; set; }

        public string Language { get; set; }

        public string LanguageLevel { get; set; }

        public ContractCondition ContractInformation { get; set; }

        public IEnumerable<SkillRequired> SkillsRequired { get => _skillsRequired; set => _skillsRequired = (List<SkillRequired>)value; }

        public string Zone { get; set; }

        public void AddSkillRequired(SkillRequired skillRequired)
        {
            if(_skillsRequired.Any(s => s.Skill == skillRequired.Skill))
            {
                throw new InvalidOperationException(DomainErrorMessages.SKILL_REQUIRED_ALREADY_EXISTS);
            }

            skillRequired.Validate();

            _skillsRequired.Add(skillRequired);
        }

        public void AddJobApplicationRequested(Person person)
        {
            if (_applications.Any(a => a.PersonId == person.Id && a.Progress.Any(p => p.State == ApplicationState.Requested)))
            {
                throw new InvalidOperationException(DomainErrorMessages.APPLICANT_ALREADY_REQUESTED_JOB_OFFER);
            }

            var application = new JobApplication(person.Id, DateTime.Now.Date);

            application.SetStatusRequested();

            _applications.Add(application);
        }

        public void AddJobApplicationOffered(Person person)
        {
            if (_applications.Any(a => a.PersonId == person.Id && a.Progress.Any(p => p.State == ApplicationState.Offered)))
            {
                throw new InvalidOperationException(DomainErrorMessages.APPLICANT_ALREADY_OFFERED);
            }

            var application = new JobApplication(person.Id, DateTime.Now.Date);

            application.SetStatusOffered();

            _applications.Add(application);
        }

        public void SetJobApplicationAccepted(Person person)
        {
            if (_applications.Any(a => a.PersonId == person.Id && a.Progress.Any(p => p.State == ApplicationState.Accepted)))
            {
                throw new InvalidOperationException(DomainErrorMessages.APPLICANT_ALREADY_ACCEPTED);
            }

            _applications.Where(a => a.PersonId == person.Id).SingleOrDefault().SetStatusAccepted();
        }

        public void Publish()
        {
            State = JobOfferState.Published;
        }

        public override void Validate()
        {
            
        }
    }
}
