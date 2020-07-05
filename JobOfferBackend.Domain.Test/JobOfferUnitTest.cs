using JobOfferBackend.Domain.Constants;
using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace JobOfferBackend.Domain.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class JobOfferUnitTest
    {
        [TestMethod]
        public void RecieveApplicant_CreatesJobApplication_WhenItIsTheFirstAttemptToApply()
        {
            //Arrange
            var person = new Person() { FirstName = "Pepe", LastName = "Lopez" };
            person.SetAbility(new Ability(new Skill() { Name = "C#" }, 5));

            var jobOffer = new JobOffer();
            jobOffer.AddSkillRequired(new SkillRequired(new Skill() { Name = "C#" }, 3, true));

            //Act
            jobOffer.AddJobApplicationRequested(person);

            var jobApplication = jobOffer.Applications.Where(a => a.PersonId == person.Id).SingleOrDefault();


            //Assert
            Assert.IsNotNull(jobApplication);
        }

        [TestMethod]
        public void RecieveApplicant_ThrowsInvalidOperationException_WhenApplicantAlreadyExists()
        {
            //Arrange
            var person = new Person() { FirstName = "Pepe", LastName = "Lopez" };
            person.SetAbility(new Ability(new Skill() { Name = "C#" }, 5));

            var jobOffer = new JobOffer();
            jobOffer.AddSkillRequired(new SkillRequired(new Skill() { Name = "C#" }, 3, true));
            
            jobOffer.AddJobApplicationRequested(person);

            //Act
            try
            {
                jobOffer.AddJobApplicationRequested(person);
                
                Assert.Fail("Job offer shoud throw exeption when a person tries to apply more than one time");
            }
            catch(InvalidOperationException ex)
            {
                Assert.AreEqual(ex.Message, DomainErrorMessages.APPLICANT_ALREADY_REQUESTED_JOB_OFFER);
            }
        }

        [TestMethod]
        public void AddSkillRequired_ThrowsInvalidOperationException_WhenJobSkillAlreadyExists()
        {
            //Arrange
            var jobOffer = new JobOffer();
            var skill = new Skill() { Name = "C#", Id = Guid.NewGuid().ToString() };

            jobOffer.AddSkillRequired(new SkillRequired(skill, 3, true));

            //Act
            try
            {
                jobOffer.AddSkillRequired(new SkillRequired(skill, 5, false));

                Assert.Fail("It shouldn't allow to add a repeated skill");
            }
            catch(InvalidOperationException ex)
            {
                Assert.AreEqual(ex.Message, DomainErrorMessages.SKILL_REQUIRED_ALREADY_EXISTS);
            }
        }

        [TestMethod]
        public void AddSkillRequired_AcceptsSkill_WhenJobSkillDoesNotExists()
        {
            //Arrange
            var jobOffer = new JobOffer();
            var cSharp = new Skill() { Name = "C#", Id = Guid.NewGuid().ToString() };
            var javascript = new Skill() { Name = "Javascript", Id = Guid.NewGuid().ToString() };

            jobOffer.AddSkillRequired(new SkillRequired(cSharp, 3, true));

            //Act
            jobOffer.AddSkillRequired(new SkillRequired(javascript, 5, false));

            Assert.AreEqual(2, jobOffer.SkillsRequired.Count());
        }
    }
}
