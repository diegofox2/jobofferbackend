using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace JobOfferBackend.Domain.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class PersonUnitTest
    {
        [TestMethod]
        public void ApplyToJobOffer_CreatesNewJobApplication_OnAJobOffer()
        {
            //Arrange

            var skill = new Skill() { Name = "C#", Id = Guid.NewGuid().ToString() };

            var person = new Person() { FirstName = "Pepe", LastName = "Lopez", IdentityCard = "123" };
            person.SetAbility(new Ability(skill, 5));

            var jobOffer = new JobOffer();
            jobOffer.AddSkillRequired(new SkillRequired(skill, 3, true));

            //Act
            person.ApplyToJobOffer(jobOffer);

            var jobApplication = jobOffer.Applications.Where(a => a.ApplicantId == person.Id).SingleOrDefault();

            //Assert
            Assert.IsTrue(jobApplication.Progress.Count() == 2);
            Assert.IsTrue(jobApplication.Progress.First().State == ApplicationState.Recieved);
            Assert.IsTrue(jobApplication.Progress.Last().State == ApplicationState.Requested);
        }
    }
}
