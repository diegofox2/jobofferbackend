using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices.Test.UnitTest
{
    [TestClass]
    public class RecruiterServiceUnitTest
    {
        private readonly Mock<CompanyRepository> _companyRepositoryMock;
        private readonly Mock<RecruiterRepository> _recruiterRepositoryMock;
        private readonly Mock<JobOfferRepository> _jobOfferRepositoryMock;
        private readonly Mock<IMongoDatabase> _mongoDataBaseMock;
        private readonly RecruiterService _service;

        public RecruiterServiceUnitTest()
        {
            _mongoDataBaseMock = new Mock<IMongoDatabase>(MockBehavior.Loose);

            _companyRepositoryMock = new Mock<CompanyRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object );
            _recruiterRepositoryMock = new Mock<RecruiterRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);
            _jobOfferRepositoryMock = new Mock<JobOfferRepository>(MockBehavior.Strict, _mongoDataBaseMock.Object);

            _service = new RecruiterService(_companyRepositoryMock.Object, _recruiterRepositoryMock.Object, _jobOfferRepositoryMock.Object);
        }


        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task UpdateRecruiterAsync_InvokesRepostiroy_WhenRecriterIsValidAndExists()
        {
            //Arrange
            var recruiter = new Recruiter() { LastName = "Maidana", FirstName = "Patricia", IdentityCard = "2899999" };

            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _recruiterRepositoryMock.Setup(mock => mock.UpsertAsync(recruiter)).ReturnsAsync((ReplaceOneResult)null);

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
            catch(InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(ServicesErrorMessages.RECRUITER_DOES_NOT_EXISTS, ex.Message);
            }
        }
    }
}
