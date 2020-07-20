using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace JobOfferBackend.Domain.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class RecruiterUnitTest
    {
        [TestMethod]
        public void AcceptApplicant_CreatesNewApplication_OnAJobOffer()
        {
            //Arrange

            var skillCSharp = new Skill() { Name = "C#", Id = Guid.NewGuid().ToString() };

            var person = new Person() { FirstName = "Pepe", LastName = "Lopez", Id = Guid.NewGuid().ToString() };
            person.SetAbility(new Ability(skillCSharp, 5));

            var jobOffer = new JobOffer() { Id = Guid.NewGuid().ToString() };
            jobOffer.AddSkillRequired(new SkillRequired(skillCSharp, 3, true));

            person.ApplyToJobOffer(jobOffer);

            var recruiter = new Recruiter();



            //Act
            recruiter.AcceptApplicant(person, jobOffer);

            var jobApplication = jobOffer.Applications.Where(a => a.ApplicantId == person.Id).SingleOrDefault();

            //Assert
            Assert.IsTrue(jobApplication.Progress.Count() == 3);
            Assert.IsNotNull(jobApplication.Progress.SingleOrDefault(p => p.State == ApplicationState.Recieved));
            Assert.IsNotNull(jobApplication.Progress.SingleOrDefault(p => p.State == ApplicationState.Requested));
            Assert.IsNotNull(jobApplication.Progress.SingleOrDefault(p => p.State == ApplicationState.Accepted));
        }

        [TestMethod]
        public void OfferEmployment_CreatesANewJobApplication_AndSetItWithStatusOffered_WhenPersonHasTheRequiredSkills()
        {
            //Arrange
            var skillCSharp = new Skill() { Name = "C#", Id = Guid.NewGuid().ToString() };

            var person = new Person() { FirstName = "Pepe", LastName = "Lopez", Id = Guid.NewGuid().ToString() };
            person.SetAbility(new Ability(skillCSharp, 5));

            var jobOffer = new JobOffer() { Id = Guid.NewGuid().ToString() };
            jobOffer.AddSkillRequired(new SkillRequired(skillCSharp, 3, true));

            var recruiter = new Recruiter();

            //Act
            recruiter.OfferEmployment(jobOffer, person); 

            //Assert
            Assert.IsTrue(jobOffer.Applications.First().Progress.Count() == 2);
            Assert.IsTrue(jobOffer.Applications.First().Progress.First().State == ApplicationState.Recieved);
            Assert.IsTrue(jobOffer.Applications.First().Progress.Last().State == ApplicationState.Offered);
        }
    }
}
