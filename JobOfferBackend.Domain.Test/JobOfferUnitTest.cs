using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace JobOfferBackend.Domain.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class JobOfferUnitTest
    {
        [TestMethod]
        public void AcceptApplicant_CreatesANewProgress_OnJobApplication()
        {
            //Arrange
            var person = new Person() { FirstName = "Pepe", LastName = "Lopez" };
            person.SetAbility(new Ability(new Skill() { Name = "C#" }, 5));

            var jobOffer = new JobOffer();
            jobOffer.AddSkillRequired(new SkillRequired(new Skill() { Name = "C#" }, 3, true));

            person.ApplyToJobOffer(jobOffer);

            //Act
            jobOffer.AcceptApplicant(person);

            var jobApplication = jobOffer.Applications.Where(a => a.Applicant == person).SingleOrDefault();


            //Assert
            Assert.IsTrue(jobApplication.Progress.Count() == 2);
            Assert.IsTrue(jobApplication.Progress.Last().State == ApplicationState.Accepted);
        }
    }
}
