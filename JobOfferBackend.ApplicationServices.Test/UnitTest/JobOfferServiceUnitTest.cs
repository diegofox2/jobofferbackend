using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices.Test.UnitTest
{
    [TestClass]
    public class JobOfferServiceUnitTest
    {
        private readonly Mock<JobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<PersonRepository> _personRepositoryMock;
        private readonly Mock<IMongoDatabase> _mongoDataBaseMock;
        private readonly JobOfferService _service;

        public JobOfferServiceUnitTest()
        {
            _mongoDataBaseMock = new Mock<IMongoDatabase>(MockBehavior.Loose);
            _jobOfferRepositoryMock = new Mock<JobOfferRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _personRepositoryMock = new Mock<PersonRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _service = new JobOfferService( _jobOfferRepositoryMock.Object, _personRepositoryMock.Object);

        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetJobOffersAsync_ReturnsAListWithJobOffersIndicatingWhichOneAreAlreadyApplied()
        {
            //Arrange

            var jobOffer1 = new JobOffer() { Id = Guid.NewGuid().ToString() };
            var jobOffer2 = new JobOffer() { Id = Guid.NewGuid().ToString() };

            var cSharp = new Skill() { Id = Guid.NewGuid().ToString(), Name = "C#" };

            jobOffer1.AddSkillRequired(new SkillRequired(cSharp, 1, true));

            var jobOffers = new List<JobOffer>() { jobOffer1, jobOffer2 };

            var person = new Person() { Id = Guid.NewGuid().ToString(), IdentityCard = "123", FirstName = "Pepe", LastName = "Lopez" };

            person.SetAbility(new Ability(cSharp, 3));

            jobOffer1.AddJobApplicationOffered(person);

            person.ApplyToJobOffer(jobOffers.First());

            _jobOfferRepositoryMock.Setup(mock => mock.GetAllPublishedJobOffersAsync()).ReturnsAsync(jobOffers);
            _personRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(person);


            //Act
            var result = await _service.GetJobOffersAsync(person.Id);

            //Assert
            Assert.IsTrue(result.Single(r => r.AlreadyApplied).JobOffer == jobOffer1, "JobOffers are not including the application that the person already did");

        }

        
    }
}