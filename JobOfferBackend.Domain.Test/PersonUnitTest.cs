using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace JobOfferBackend.Domain.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class PersonUnitTest
    {
        [TestMethod]
        public void ApplyToJobOffer_CreatesNewApplication_OnAJobOffer()
        {
            //Arrange
            var person = new Person() { FirstName = "Pepe", LastName = "Lopez" };
            person.SetAbility(new Ability(new Skill() { Name = "C#" }, 5));

            var jobOffer = new JobOffer();
            jobOffer.AddSkillRequired(new SkillRequired(new Skill() { Name = "C#" }, 3, true));

            //Act
            person.ApplyToJobOffer(jobOffer);

            var jobApplication = jobOffer.Applications.Where(a => a.Applicant == person).SingleOrDefault();

            //Assert
            Assert.IsTrue(jobApplication.Progress.Count() == 1);
            Assert.IsTrue(jobApplication.Progress.First().State == ApplicationState.Requested);
        }
    }
}
