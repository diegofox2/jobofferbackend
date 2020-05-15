using JobOffer.Domain.Base;
using JobOffer.Domain.Constants;
using MongoDB.Driver.GeoJsonObjectModel.Serializers;
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

        public void AddJobHistory(Job job)
        {
            if (_jobHistory.Any(item => item.ComparePropertiesTo(job)))
            {
                throw new InvalidOperationException(DomainErrorMessages.JOB_REPEATED);
            }
            else
            {
                _jobHistory.Add(job);
            }
        }

        public void UpdateJobHistory(Job job)
        {
            var jobToUpdate = _jobHistory.Find(j => j == job);
            
            if(jobToUpdate == null)
            {
                throw new InvalidOperationException(DomainErrorMessages.JOB_DOES_NOT_EXISTS);
            }

            _jobHistory.Remove(jobToUpdate);
            _jobHistory.Add(job);
        }

        public void AddStudy(Study study)
        {
            study.Validate();

            if (_studies.Any(item => item == study))
            {
                throw new InvalidOperationException(DomainErrorMessages.STUDY_REPEATED);
            }
            else
            {
                _studies.Add(study); 
            }
        }

        public void AddAbility(Ability ability)
        {
            ability.Validate();

            if (_abilities.Any(item => item == ability))
            {
                throw new InvalidOperationException(DomainErrorMessages.ABILITY_REPEATED);
            }
            else
            {
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
