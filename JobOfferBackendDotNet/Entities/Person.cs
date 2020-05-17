using JobOffer.Domain.Base;
using JobOffer.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JobOffer.Domain.Entities
{
    public class Person : BaseEntity<Person>
    {
        private List<Job> _jobHistory = new List<Job>();
        private List<Study> _studies = new List<Study>();
        private List<Ability> _abilities = new List<Ability>();

        public string IdentityCard { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<Job> JobHistory { get => _jobHistory; set => _jobHistory = (List<Job>) value; }

        public IEnumerable<Study> Studies { get => _studies; set => _studies = (List<Study>) value; }

        public IEnumerable<Ability> Abilities { get => _abilities; set => _abilities = (List<Ability>) value; }

        public void SetPreviousJob(Job job, Job jobToReplace = default)
        {
            if (_jobHistory.Any(item => item == job))
            {
                throw new InvalidOperationException(DomainErrorMessages.JOB_REPEATED);
            }
            else
            {
                if (jobToReplace != null && _jobHistory.Exists(s => s.GetHashCode() == jobToReplace.GetHashCode()))
                {
                    _jobHistory.Remove(jobToReplace);
                }

                _jobHistory.Add(job);
            }
        }


        public void SetStudy(Study study, Study studyToReplace = default)
        {
            study.Validate();

            if (_studies.Any(item => item == study))
            {
                throw new InvalidOperationException(DomainErrorMessages.STUDY_REPEATED);
            }
            else
            {
                if (studyToReplace != null && _studies.Exists(s => s.GetHashCode() == studyToReplace.GetHashCode()))
                {
                    _studies.Remove(studyToReplace);
                }

                _studies.Add(study); 
            }
        }

        public void SetAbility(Ability ability, Ability abilityToReplace = default)
        {
            ability.Validate();

            if (_abilities.Any(item => item == ability))
            {
                throw new InvalidOperationException(DomainErrorMessages.ABILITY_REPEATED);
            }
            else
            {
                if(abilityToReplace != null && _abilities.Exists(a => a.GetHashCode() == abilityToReplace.GetHashCode()))
                {
                    _abilities.Remove(abilityToReplace);
                }

                _abilities.Add(ability);
            }
        }

        public void ApplyToJobOffer(JobOffer jobOffer)
        {
            var mandatorySkills = jobOffer.SkillsRequired.Where(s => s.IsMandatory).Select(s => s.Skill );

            if(HasAnyMandatorySkills(mandatorySkills))
            {
                jobOffer.RecieveApplicant(this);
            }
            else
            {
                throw new InvalidOperationException(DomainErrorMessages.PERSON_DOES_NOT_HAVE_ALL_MANDATORY_SKILLS);
            }
            
        }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(IdentityCard))
                _errors.AppendLine(DomainErrorMessages.IDENTITY_CARD_REQUIRED);

            if (string.IsNullOrEmpty(FirstName))
                _errors.AppendLine(DomainErrorMessages.FIRST_NAME_REQUIRED);

            if (string.IsNullOrEmpty(LastName))
                _errors.AppendLine(DomainErrorMessages.LAST_NAME_REQUIRED);

            _jobHistory?.ForEach(item => item.Validate());

            _studies?.ForEach(item => item.Validate());

            _abilities?.ForEach(item => item.Validate());

            ThrowExceptionIfErrors();
        }

        private bool HasAnyMandatorySkills(IEnumerable<Skill> skillsRequired)
        {
            var result = false;

            for (int s = 0; s < skillsRequired.Count(); s++)
            {
                if (_abilities.Any(a => a.Skill == skillsRequired.ElementAt(s)))
                {
                    return true;
                }
            }

            return result;
        }
    }
}
