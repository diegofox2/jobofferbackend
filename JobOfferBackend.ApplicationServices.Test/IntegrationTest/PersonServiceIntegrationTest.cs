using JobOfferBackend.ApplicationServices.Test.Base;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Constants;
using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices.Test.IntegrationTest
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class PersonServiceIntegrationTest : IntegrationTestBase
    {
        private readonly PersonService _service;
        private readonly PersonRepository _personRepository;
        private readonly JobOfferRepository _jobOfferRepository;
        private readonly AccountRepository _accountRepository;

        public PersonServiceIntegrationTest()
        {
            _personRepository = new PersonRepository(_database);
            _jobOfferRepository = new JobOfferRepository(_database);
            _accountRepository = new AccountRepository(_database);

            _service = new PersonService(_personRepository, _jobOfferRepository, _accountRepository);
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

            person.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2015, 5, 1), true));
            person.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2014, 1, 1), false, new DateTime(2015, 4, 30)));

            person.SetStudy(new Study("UBA", "Lic.Relaciones del Trabajo", StudyStatus.Completed));

            //Act
            await _service.CreatePersonAsync(person);

            var savedPerson = await _service.GetPersonByIdAsync(person.Id);

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

            var savedPerson = await _service.GetPersonByIdAsync(person.Id);

            //Act

            var previousJob = savedPerson.JobHistory.Where(j => j.CompanyName == "Accenture" && j.From.Date == new DateTime(2014, 1, 1).Date).Single();

            var newJob = (Job)previousJob.Clone();

            newJob.CompanyName = "Globant";

            person.SetPreviousJob(newJob, previousJob);

            await _service.UpdatePersonAsync(person);

            var updatedPerson = await _service.GetPersonByIdAsync(person.Id);

            //Assert
            Assert.AreEqual("Globant", updatedPerson.JobHistory.Single(j => j == newJob).CompanyName, "Company name of a person was now updated");
        }

        [TestMethod]
        public async Task CreatePerson_ThrowsInvalidOperationException_WhenPersonHasADuplicatedPreviousStudy()
        {
            //Arrange
            var person = new Person()
            {
                FirstName = "Patricia",
                LastName = "Maidana",
                IdentityCard = "28123456"
            };

            var previousStudiesRepeated = new List<Study>()
            {
                new Study("UBA", "Lic.Relaciones del Trabajo", StudyStatus.Completed),
                new Study("UBA", "Lic.Relaciones del Trabajo", StudyStatus.Completed)
            };

            person.Studies = previousStudiesRepeated;


            //Act
            try
            {
                await _service.CreatePersonAsync(person);

                Assert.Fail("Person shouldn't be created when has duplicated previous study ");
            }
            catch(InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Data.Values.Cast<string>().Contains(DomainErrorMessages.STUDY_REPEATED));
            }
        }

        [TestMethod]
        public async Task CreatePerson_ThrowsInvalidOperationException_WhenPersonHasADuplicatedPreviousAbility()
        {
            //Arrange
            var person = new Person()
            {
                FirstName = "Patricia",
                LastName = "Maidana",
                IdentityCard = "28123456"
            };

            var cSharp = new Skill() { Id = "a", Name = "C#" };

            var previousAbilitiesRepeated = new List<Ability>()
            {
                new Ability(cSharp, 1),
                new Ability(cSharp, 1),
            };

            person.Abilities = previousAbilitiesRepeated;

            //Act
            try
            {
                await _service.CreatePersonAsync(person);

                Assert.Fail("Person shouldn't be created when has duplicated previous ability ");
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Data.Values.Cast<string>().Contains(DomainErrorMessages.ABILITY_REPEATED));
            }
        }

        [TestMethod]
        public async Task CreatePerson_ThrowsInvalidOperationException_WhenPersonHasADuplicatedPreviousJobs()
        {
            //Arrange
            var person = new Person()
            {
                FirstName = "Patricia",
                LastName = "Maidana",
                IdentityCard = "28123456"
            };

            var previousJobsRepeated = new List<Job>()
            {
                new Job("Accenture", "Sr.Talent Adquision", new DateTime(2015, 5, 1), true),
                new Job("Accenture", "Sr.Talent Adquision", new DateTime(2015, 5, 1), true)
            };

            person.JobHistory = previousJobsRepeated;

            //Act
            try
            {
                await _service.CreatePersonAsync(person);

                Assert.Fail("Person shouldn't be created when has duplicated previous job ");
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Data.Values.Cast<string>().Contains(DomainErrorMessages.JOB_HISTORY_REPEATED));
            }
        }
    }
}
