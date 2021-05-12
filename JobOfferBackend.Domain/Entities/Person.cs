using JobOfferBackend.Domain.Common;
using JobOfferBackend.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobOfferBackend.Domain.Entities
{
    public class Person : BaseEntity<Person>
    {
        private List<Job> _jobHistory = new List<Job>();
        private List<Study> _studies = new List<Study>();
        private List<Ability> _abilities = new List<Ability>();
        private List<string> _myJobApplications = new List<string>();

        public string IdentityCard { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<Job> JobHistory { get => _jobHistory; set => _jobHistory = (List<Job>) value; }

        public IEnumerable<Study> Studies { get => _studies; set => _studies = (List<Study>) value; }

        public IEnumerable<Ability> Abilities { get => _abilities; set => _abilities = (List<Ability>) value; }

        public IEnumerable<string> MyJobApplications { get => _myJobApplications; set => _myJobApplications = (List<string>)value; }

        public void SetPreviousJob(Job job, Job jobToReplace = default)
        {
            Validate();

            job.Validate();

            if (jobToReplace != null && _jobHistory.Exists(s => s == jobToReplace))
            {
                _jobHistory.Remove(jobToReplace);
            }

            _jobHistory.Add(job);
            
        }

        public void SetStudy(Study study, Study studyToReplace = default)
        {
            study.Validate();

            Validate();

            if (studyToReplace != null && _studies.Exists(s => s.GetHashCode() == studyToReplace.GetHashCode()))
            {
                _studies.Remove(studyToReplace);
            }

            _studies.Add(study);
            
        }

        public void SetAbility(Ability ability, Ability abilityToReplace = default)
        {
            ability.Validate();

            Validate();

            if (abilityToReplace != null && _abilities.Exists(a => a == abilityToReplace))
            {
                _abilities.Remove(abilityToReplace);
            }

            _abilities.Add(ability);
            
        }

        public void ApplyToJobOffer(JobOffer jobOffer)
        {
            var mandatorySkills = jobOffer.SkillsRequired.Where(s => s.IsMandatory).ToList();

            if(MeetsSomeRequiredSkillsAndHasSameOrMoreYearsOfExperience(mandatorySkills))
            {
                _myJobApplications.Add(jobOffer.Id);

                jobOffer.AddJobApplicationRequested(this);
            }
            else
            {
                throw new InvalidOperationException(DomainErrorMessages.PERSON_DOES_NOT_HAVE_ALL_MANDATORY_SKILLS);
            }
            
        }

        public void SetJobApplicationAccepted(string jobOfferId)
        {
            _myJobApplications.Add(jobOfferId);
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(IdentityCard))
                _errorLines.AppendLine(DomainErrorMessages.IDENTITY_CARD_REQUIRED);

            if (string.IsNullOrEmpty(FirstName))
                _errorLines.AppendLine(DomainErrorMessages.FIRST_NAME_REQUIRED);

            if (string.IsNullOrEmpty(LastName))
                _errorLines.AppendLine(DomainErrorMessages.LAST_NAME_REQUIRED);

            _jobHistory?.ForEach(item => item.Validate());

            _studies?.ForEach(item => item.Validate());

            _abilities?.ForEach(item => item.Validate());

            CheckJobHistoryDuplications();

            CheckAbilityDuplications();

            CheckStudyDuplications();

            ThrowExceptionIfErrors();
        }

        private bool MeetsSomeRequiredSkillsAndHasSameOrMoreYearsOfExperience(List<SkillRequired> skillsRequired)
        {
            return skillsRequired.Any(skill => _abilities.Any(ability => ability.SkillId == skill.SkillId && ability.Years >= skill.Years));
        }

        private void CheckJobHistoryDuplications()
        {
            if (_jobHistory.GroupBy(item => new { item.CompanyName, item.From }).Any(g => g.Count() > 1))
            {
                _errorLines.AppendLine(DomainErrorMessages.JOB_HISTORY_REPEATED);
            }
        }

        private void CheckAbilityDuplications()
        {
            if (_abilities.GroupBy(item => new { item.SkillId }).Any(g => g.Count() > 1))
            {
                _errorLines.AppendLine(DomainErrorMessages.ABILITY_REPEATED);
            }
        }

        private void CheckStudyDuplications()
        {
            if (_studies.GroupBy(item => new { item.Institution, item.Title }).Any(g => g.Count() > 1))
            {
                _errorLines.AppendLine(DomainErrorMessages.STUDY_REPEATED);
            }
        }
    }
}
