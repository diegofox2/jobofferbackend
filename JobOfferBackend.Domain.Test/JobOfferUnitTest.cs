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

            var skill = new Skill() { Name = "C#", Id = Guid.NewGuid().ToString() };

            var person = new Person() { FirstName = "Pepe", LastName = "Lopez", IdentityCard = "123" };
            person.SetAbility(new Ability(skill, 5));

            var jobOffer = new JobOffer();
            jobOffer.AddSkillRequired(new SkillRequired(skill, 3, true));


            //Act
            jobOffer.AddJobApplicationRequested(person);

            var jobApplication = jobOffer.Applications.Where(a => a.ApplicantId == person.Id).SingleOrDefault();


            //Assert
            Assert.IsNotNull(jobApplication);
        }

        [TestMethod]
        public void RecieveApplicant_ThrowsInvalidOperationException_WhenApplicantAlreadyExists()
        {
            //Arrange

            var skill = new Skill() { Name = "C#", Id = Guid.NewGuid().ToString() };

            var person = new Person() { FirstName = "Pepe", LastName = "Lopez", IdentityCard = "123" };
            person.SetAbility(new Ability(skill, 5));

            var jobOffer = new JobOffer();
            jobOffer.AddSkillRequired(new SkillRequired(skill, 3, true));
            
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

        [TestMethod]
        public void AddSkillRequired_ThrowsException_WhenThereIsASkillRequiredRepeated()
        {
            //Arrange
            var contractCondition = new ContractCondition() { KindOfContract = "a", StartingFrom = "b", WorkingDays = "c" };
            var jobOffer = new JobOffer() { Title = "Some title", ContractInformation = contractCondition };
            var javascript = new Skill() { Id = Guid.NewGuid().ToString(), Name = "Javascript" };
            var skillRequired = new SkillRequired(javascript, 1);

            jobOffer.AddSkillRequired(skillRequired);
            

            //Act
            try
            {
                jobOffer.AddSkillRequired(skillRequired);
            }
            catch(InvalidOperationException ex)
            {
                Assert.AreEqual(ex.Message, DomainErrorMessages.SKILL_REQUIRED_ALREADY_EXISTS);
            }
        }
    }
}
