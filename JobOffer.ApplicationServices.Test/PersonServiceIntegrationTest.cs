using JobOffer.ApplicationServices.Test.Base;
using JobOffer.DataAccess;
using JobOffer.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JobOffer.ApplicationServices.Test
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class PersonServiceIntegrationTest : IntegrationTestBase
    {
        private readonly PersonService _service;
        private readonly PersonRepository _personRepository;

        public PersonServiceIntegrationTest()
        {
            _personRepository = new PersonRepository(_database);
            _service = new PersonService(_personRepository);
        }

        [TestInitialize]
        public void Init()
        {
            SetupIntegrationTest.OneTimeTearDown();
            SetupIntegrationTest.OneTimeSetUp();
        }

        [TestCleanup]
        public void Clean()
        {
            SetupIntegrationTest.OneTimeTearDown();
        }

        
        [TestMethod]
        public async Task CreatePerson_SavePersonSuccessfully_WhenPersonDataIsCorrectAndPersonNotExists()
        {
            //Arrange
            var person = new Person()
            {
                FirstName = "Patricia",
                LastName = "Maidana",
                IdentityCard = "28123456"
            };

            person.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2015,5,1), true));
            person.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2014, 1, 1), false, new DateTime(2015,4,30)));

            person.SetStudy(new Study("UBA", "Lic.Relaciones del Trabajo", StudyStatus.Completed));

            //Act
            await _service.CreatePersonAsync(person);

            var savedPerson = await _service.GetPersonAsync(person);

            //Assert
            Assert.AreEqual(person, savedPerson, "Person was not saved");
        }

        [TestMethod]
        public async Task UpdatePerson_SavePersonSuccessfully_WhenPersonDataIsCorrectAndPersonExists()
        {
            //Arrange
            var person = new Person()
            {
                FirstName = "Patricia",
                LastName = "Maidana",
                IdentityCard = "28123456"
            };

            person.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2015, 5, 1), true));
            person.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2014, 1, 1), false, new DateTime(2015, 4, 30)));

            person.SetStudy(new Study("UBA", "Lic.Relaciones del Trabajo", StudyStatus.Completed));

            await _service.CreatePersonAsync(person);

            var savedPerson = await _service.GetPersonAsync(person);

            //Act

            var previousJob = savedPerson.JobHistory.Where(j => j.CompanyName == "Accenture" && j.From.Date == new DateTime(2014, 1, 1).Date).Single();

            var newJob = (Job)previousJob.Clone();

            newJob.CompanyName = "Globant";

            person.SetPreviousJob(newJob, previousJob);

            await _service.UpdatePersonAsync(person);

            var updatedPerson = await _service.GetPersonAsync(person);

            //Assert
            Assert.AreEqual("Globant", updatedPerson.JobHistory.Single(j => j == newJob).CompanyName, "Company name of a person was now updated");
        }

    }
}
