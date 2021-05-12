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
        Updated,
        Published,
        Finished
    }

    public enum LanguageLevel
    {
        Nothing,
        Basic,
        Intermediate,
        Advance
    }

    public class JobOffer : BaseEntity<JobOffer>
    {
        private List<JobApplication> _applications = new List<JobApplication>();
        private List<SkillRequired> _skillsRequired = new List<SkillRequired>();

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<JobApplication> Applications { get => _applications; set => _applications = (List<JobApplication>)value; }

        public string CompanyId { get; set; }

        public string RecruiterId { get; set; }

        public JobOfferState State { get; set; }

        public string Language { get; set; }

        public LanguageLevel LanguageLevel { get; set; }

        public bool IsLanguageMandatory { get; set; }

        public ContractCondition ContractInformation { get; set; }

        public IEnumerable<SkillRequired> SkillsRequired { get => _skillsRequired; set => _skillsRequired = (List<SkillRequired>)value; }

        public string Zone { get; set; }

        public void AddSkillRequired(SkillRequired skillRequired)
        {
            if (_skillsRequired.Any(s => s.SkillId == skillRequired.SkillId))
            {
                throw new InvalidOperationException(DomainErrorMessages.SKILL_REQUIRED_ALREADY_EXISTS);
            }

            skillRequired.Validate();

            _skillsRequired.Add(skillRequired);
        }

        public void AddJobApplicationRequested(Person person)
        {
            if (_applications.Any(a => a.ApplicantId == person.Id && a.Progress.Any(p => p.State == ApplicationState.Requested)))
            {
                throw new InvalidOperationException(DomainErrorMessages.APPLICANT_ALREADY_REQUESTED_JOB_OFFER);
            }

            var application = new JobApplication(person.Id, DateTime.Now.Date);

            application.SetStatusRequested();

            _applications.Add(application);
        }

        public void AddJobApplicationOffered(Person person)
        {
            if (_applications.Any(a => a.ApplicantId == person.Id && a.Progress.Any(p => p.State == ApplicationState.Offered)))
            {
                throw new InvalidOperationException(DomainErrorMessages.APPLICANT_ALREADY_OFFERED);
            }

            var application = new JobApplication(person.Id, DateTime.Now.Date);

            application.SetStatusOffered();

            _applications.Add(application);
        }

        public void SetJobApplicationAccepted(Person person)
        {
            if (_applications.Any(a => a.ApplicantId == person.Id && a.Progress.Any(p => p.State == ApplicationState.Accepted)))
            {
                throw new InvalidOperationException(DomainErrorMessages.APPLICANT_ALREADY_ACCEPTED);
            }

            _applications.Where(a => a.ApplicantId == person.Id).SingleOrDefault().SetStatusAccepted();
        }

        public void Publish()
        {
            State = JobOfferState.Published;
        }

        public void Finish()
        {
            State = JobOfferState.Finished;
        }

        public override void Validate()
        {
            if (ContractInformation == null)
                _errorLines.AppendLine(DomainErrorMessages.CONTRACT_INFORMATION_EMPTY);

            if (string.IsNullOrEmpty(CompanyId))
                _errorLines.AppendLine(DomainErrorMessages.COMPANY_REQUIRED);

            CheckNoDuplicatedSkillsRequired();

            CheckNoSkillsRequiredInvalid();

            CheckNoInvalidApplications();

            ThrowExceptionIfErrors();
        }

        private void CheckNoSkillsRequiredInvalid()
        {
            foreach(var skillRequired in SkillsRequired)
            {
                skillRequired.Validate();
            }
        }

        private void CheckNoDuplicatedSkillsRequired()
        {
            if (_skillsRequired.GroupBy(item => item.SkillId).Any(item=> item.Count() > 1))
            {
                _errorLines.AppendLine(DomainErrorMessages.SKILL_REQUIRED_ALREADY_EXISTS);
            }
        }

        private void CheckNoInvalidApplications()
        {
            _applications.ForEach(application => application.Validate());
        }
    }
}
