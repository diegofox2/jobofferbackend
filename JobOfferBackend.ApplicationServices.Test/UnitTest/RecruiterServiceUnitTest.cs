using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Constants;
using JobOfferBackend.Domain.Entities;
using JobOfferBackend.Doman.Security.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices.Test.UnitTest
{
    [TestClass]
    public class RecruiterServiceUnitTest
    {
        private readonly Mock<CompanyRepository> _companyRepositoryMock;
        private readonly Mock<RecruiterRepository> _recruiterRepositoryMock;
        private readonly Mock<JobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<PersonRepository> _personRepositoryMock;
        private readonly Mock<AccountRepository> _accountRepositoryMock;
        private readonly Mock<IMongoDatabase> _mongoDataBaseMock;
        private readonly RecruiterService _service;

        public RecruiterServiceUnitTest()
        {
            _mongoDataBaseMock = new Mock<IMongoDatabase>(MockBehavior.Loose);

            _companyRepositoryMock = new Mock<CompanyRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _recruiterRepositoryMock = new Mock<RecruiterRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _jobOfferRepositoryMock = new Mock<JobOfferRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _personRepositoryMock = new Mock<PersonRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _accountRepositoryMock = new Mock<AccountRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);

            _service = new RecruiterService(_companyRepositoryMock.Object, _recruiterRepositoryMock.Object, _jobOfferRepositoryMock.Object, _personRepositoryMock.Object, _accountRepositoryMock.Object);
        }


        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task UpdateRecruiterAsync_InvokesRepostiroy_WhenRecriterIsValidAndExists()
        {
            //Arrange
            var recruiter = new Recruiter() { LastName = "Maidana", FirstName = "Patricia", IdentityCard = "2899999" };

            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _recruiterRepositoryMock.Setup(mock => mock.UpsertAsync(recruiter)).Returns(Task.CompletedTask);

            //Act
            await _service.UpdateRecruiterAsync(recruiter);

            //Assert
            _recruiterRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task UpdateRecruiterAsync_ThrowsInvalidOperationException_WhenRecriterDoesNotExists()
        {
            //Arrange
            var recruiter = new Recruiter() { LastName = "Maidana", FirstName = "Patricia", IdentityCard = "2899999" };

            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            try
            {
                await _service.UpdateRecruiterAsync(recruiter);

                //Assert
                Assert.Fail("It shouldn't allow updateing a recruiter when it doesn't exist");
            }
            catch (InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(ServicesErrorMessages.RECRUITER_DOES_NOT_EXISTS, ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllJobOffersAsync_ReturnsJobOffers_WhenBothAccountAndRecruiterExists()
        {
            //Arrange
            _accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Account());

            _recruiterRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Recruiter());

            _jobOfferRepositoryMock.Setup(mock => mock.GetAllJobOffersByRecruiter(It.IsAny<Recruiter>())).ReturnsAsync(new List<JobOffer>());

            //Act
            await _service.GetAllJobOffersCreatedByAccountAsync(It.IsAny<string>());

            //Assert
            _accountRepositoryMock.VerifyAll();
            _recruiterRepositoryMock.VerifyAll();
            _jobOfferRepositoryMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllJobOffersAsync_ThrowsInvalidOperationException_WhenAccountDoesNotExists()
        {
            //Arrange
            _accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Account)null);

            //Act
            try
            {
                await _service.GetAllJobOffersCreatedByAccountAsync(It.IsAny<string>());

                //Assert
                Assert.Fail("It should throw an exception when account does not exists");
            }
            catch (InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(DomainErrorMessages.ACCOUNT_DOES_NOT_EXISTS, ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllJobOffersAsync_ThrowsInvalidOperationException_WhenRecruiterDoesNotExists()
        {
            //Arrange
            _accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Account());

            _recruiterRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Recruiter)null);

            //Act
            try
            {
                await _service.GetAllJobOffersCreatedByAccountAsync(It.IsAny<string>());

                //Assert
                Assert.Fail("It should throw an exception when recruiter does not exists");
            }
            catch (InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(DomainErrorMessages.INVALID_RECRUITER, ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateJobOfferAsync_CreatesANewJobOffer_WhenJobOfferIsValidAndItDoesNotExistsPreviously()
        {
            //Arrange

            var jobOffer = new JobOffer();
            jobOffer.ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" };

            _recruiterRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Recruiter());

            _jobOfferRepositoryMock.Setup(mock => mock.GetActiveJobOffersByRecruiterAsync(It.IsAny<Recruiter>())).ReturnsAsync(new List<JobOffer>());

            _jobOfferRepositoryMock.Setup(mock => mock.UpsertAsync(It.IsAny<JobOffer>())).Returns(Task.CompletedTask);

            //Act
            await _service.CreateJobOfferAsync(jobOffer, It.IsAny<string>());

            //Assert
            _recruiterRepositoryMock.VerifyAll();
            _jobOfferRepositoryMock.VerifyAll();

        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateJobOfferAsync_ThrowsInvalidOperationException_WhenJobOfferAlreadyExists()
        {
            //Arrange
            var jobOffer = new JobOffer() { Id = Guid.NewGuid().ToString() };
            jobOffer.ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" };
            //Act
            try
            {
                await _service.CreateJobOfferAsync(jobOffer, It.IsAny<string>());

                //Assert
                Assert.Fail("It should throw an exception when job offer already exists");
            }
            catch (InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(DomainErrorMessages.JOBOFFER_ALREADY_EXISTS, ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateJobOfferAsync_ThrowsInvalidOperationException_WhenJobOfferWithSameTitleAndCompanyAlreadyExistsAndItIsNotFinished()
        {
            //Arrange

            var company = new Company( "Accenture", "IT"){ Id = Guid.NewGuid().ToString() };

            var jobOffer = new JobOffer();
            jobOffer.Title = "Some Job";
            jobOffer.Company = company;
            jobOffer.ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" };

            _recruiterRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Recruiter());

            _jobOfferRepositoryMock.Setup(mock => mock.GetActiveJobOffersByRecruiterAsync(It.IsAny<Recruiter>())).ReturnsAsync(new List<JobOffer>() { jobOffer });

            //Act
            try
            {
                await _service.CreateJobOfferAsync(jobOffer, It.IsAny<string>());

                //Assert
                Assert.Fail("It should throw an exception when a job offer with the same title, for the same company, already exists and the offer is not finished");
            }
            catch (InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(DomainErrorMessages.JOBOFFER_ALREADY_EXISTS, ex.Message);
            }
        }

    }
}
