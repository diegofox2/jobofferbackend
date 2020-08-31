using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using JobOfferBackend.Doman.Security.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices.Test.UnitTest
{
    [TestClass]
    public class PersonServiceUnitTest
    {
        private readonly Mock<AccountRepository> _accountRepositoryMock;
        private readonly Mock<JobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<PersonRepository> _personRepositoryMock;
        private readonly Mock<IMongoDatabase> _mongoDataBaseMock;
        private readonly PersonService _personService;

        public PersonServiceUnitTest()
        {
            _mongoDataBaseMock = new Mock<IMongoDatabase>(MockBehavior.Loose);

            _accountRepositoryMock = new Mock<AccountRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _jobOfferRepositoryMock = new Mock<JobOfferRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _personRepositoryMock = new Mock<PersonRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _personService = new PersonService(_personRepositoryMock.Object, _jobOfferRepositoryMock.Object, _accountRepositoryMock.Object);

        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ApplyToJobOffer_CreateNewJobApplication_AndLinkItToThePerson_WhenPersonHasSkillsRequired()
        {
            //Arrange
            _accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Account());

            var jobOffer = new JobOffer();
            var cSharp = new Skill() { Id = Guid.NewGuid().ToString(), Name = "C#" };
            var mongoDB = new Skill() { Id = Guid.NewGuid().ToString(), Name = "MongoDb" };
            jobOffer.AddSkillRequired(new SkillRequired(cSharp, 3, true));
            jobOffer.AddSkillRequired(new SkillRequired(mongoDB, 2));

            _jobOfferRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(jobOffer);

            var person = new Person() { IdentityCard = "123", Id = Guid.NewGuid().ToString(), FirstName = "Pepe", LastName = "Lopez" };
            person.SetAbility(new Ability(cSharp, 5));
            person.SetAbility(new Ability(mongoDB, 1));

            _personRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(person);

            _jobOfferRepositoryMock.Setup(mock => mock.UpsertAsync(It.IsAny<JobOffer>())).Returns(Task.CompletedTask);

            _personRepositoryMock.Setup(mock => mock.UpsertAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

            //Act
            await _personService.ApplyToJobOfferAsync(It.IsAny<string>(), It.IsAny<string>());

            //Accert
            _jobOfferRepositoryMock.VerifyAll();
            _personRepositoryMock.VerifyAll();

            Assert.AreEqual(person.Id, jobOffer.Applications.First().ApplicantId, "The new Job Application should have as applicant the same ID than the person who is applying");


        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ApplyToJobOffer_ThrowsInvalidOperationException_WhenPersonDoesNotExists()
        {
            //Arrange
            _accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Account());
            _jobOfferRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new JobOffer());
            _personRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Person)null);

            //Act
            try
            {
                await _personService.ApplyToJobOfferAsync(It.IsAny<string>(), It.IsAny<string>());

                Assert.Fail("It should throw an exception when person does not exists");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ServicesErrorMessages.PERSON_DOES_NOT_EXISTS, exception.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ApplyToJobOffer_ThrowsInvalidOperationException_WhenJobOfferDoesNotExists()
        {
            //Arrange
            _accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Account());
            _jobOfferRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((JobOffer)null);

            //Act
            try
            {
                await _personService.ApplyToJobOfferAsync(It.IsAny<string>(), It.IsAny<string>());

                Assert.Fail("It should throw an exception when job offer does not exists");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ServicesErrorMessages.INVALID_JOB_OFFER, exception.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ApplyToJobOffer_ThrowsInvalidOperationException_WhenAccountDoesNotExists()
        {
            //Arrange
            _accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Account)null);

            //Act
            try
            {
                await _personService.ApplyToJobOfferAsync(It.IsAny<string>(), It.IsAny<string>());

                Assert.Fail("It should throw an exception when account does not exists");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ServicesErrorMessages.INVALID_USER_ACCOUNT, exception.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task UpdatePerson_ThrowsInvalidOperationException_WhenPersonDoesNotExists()
        {
            //Arrange
            _personRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            try
            {
                await _personService.UpdatePersonAsync(new Person());

                Assert.Fail("It should throw an exception when person does not exists");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ServicesErrorMessages.PERSON_DOES_NOT_EXISTS, exception.Message);
            }
        }
    }
}