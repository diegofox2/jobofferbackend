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
using System.Linq;
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
            var recruiter = new Recruiter() { PersonId = "xxxx"};

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
            var recruiter = new Recruiter() { PersonId = "xxx"};

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

            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            var jobOfferList = new List<JobOffer>
            {
                new JobOffer(), new JobOffer()
            };

            _jobOfferRepositoryMock.Setup(mock => mock.GetAllJobOffersByRecruiter(It.IsAny<string>())).ReturnsAsync(jobOfferList);

            //Act
            var result = await _service.GetAllJobOffersCreatedByAccountAsync(It.IsAny<string>());

            //Assert
            _accountRepositoryMock.VerifyAll();
            _recruiterRepositoryMock.VerifyAll();

            Assert.IsTrue(result.ToList().Count == 2);
            Assert.IsTrue(result.ToList().All(item => item.AlreadyApplied == false));
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

            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

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
        public async Task GetNewJobOffer_ReturnsNewJobOffer_WhenRecruiterExists()
        {
            //Arrange
            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            //Act
            var newJobOffer = await _service.GetNewJobOffer("someId");

            //Assert
            Assert.IsTrue(!string.IsNullOrWhiteSpace(newJobOffer.Id), "The jobOfferId should not be empty");
            Assert.AreEqual("someId", newJobOffer.RecruiterId, "The recruiterId should be the same than the recruiter sent by param");
            Assert.AreEqual(JobOfferState.WorkInProgress, newJobOffer.State, $"The initial state should be {JobOfferState.WorkInProgress.ToString()}");
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetNewJobOffer_ThrowsInvalidOperationException_WhenRecruiterDoesNotExists()
        {
            //Arrange
            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            try
            {
                var newJobOffer = await _service.GetNewJobOffer("someId");

                Assert.Fail("It shouldn't create a new job offer if the recruiter does not exists");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(ServicesErrorMessages.RECRUITER_DOES_NOT_EXISTS, ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task Publish_ChangeTheJobOfferState_WhenItIsWorkingProgress()
        {
            //Arrange
            var jobOffer = new JobOffer
            {
                ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" },
                RecruiterId = Guid.NewGuid().ToString(),
                CompanyId = Guid.NewGuid().ToString(),
                State = JobOfferState.WorkInProgress
            };

            _jobOfferRepositoryMock.Setup(mock => mock.UpsertAsync(It.IsAny<JobOffer>())).Returns(Task.CompletedTask);

            //Act
             await _service.PublishJobOffer(jobOffer);

            //Assert
            Assert.AreEqual(JobOfferState.Published, jobOffer.State);
            _jobOfferRepositoryMock.Verify(mock => mock.UpsertAsync(It.IsAny<JobOffer>()), Times.Once);

        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task Publish_ThrowsInvalidOperationException_WhenItIsNotInWorkingProgress()
        {
            //Arrange
            var jobOffer = new JobOffer
            {
                ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" },
                RecruiterId = Guid.NewGuid().ToString(),
                CompanyId = Guid.NewGuid().ToString(),
                State = JobOfferState.Published
            };

            //Act
            try
            {
                await _service.PublishJobOffer(jobOffer);

                Assert.Fail("A job offer shouldn't be published if it isn't in working progress");
            }
            catch(InvalidOperationException ex)
            {
                Assert.AreEqual(DomainErrorMessages.ONLY_WORKINPROGRESS_JOBOFFERS_CAN_BE_PUBLISHED, ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task Publish_ThrowsInvalidOperationException_WhenTheJobOfferHasNotACompanyAssignedTo()
        {
            //Arrange
            var jobOffer = new JobOffer
            {
                ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" },
                RecruiterId = Guid.NewGuid().ToString(),
                State = JobOfferState.WorkInProgress
            };

            //Act
            try
            {
                await _service.PublishJobOffer(jobOffer);

                Assert.Fail("A job offer shouldn't be published when it is invalid");
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Data[0].ToString() == DomainErrorMessages.COMPANY_REQUIRED);
                Assert.AreEqual(JobOfferState.WorkInProgress, jobOffer.State);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task Publish_ThrowsInvalidOperationException_WhenTheJobOfferHasNotARecruiterAssignedTo()
        {
            //Arrange
            var jobOffer = new JobOffer
            {
                ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" },
                CompanyId = Guid.NewGuid().ToString(),
                State = JobOfferState.WorkInProgress
            };

            //Act
            try
            {
                await _service.PublishJobOffer(jobOffer);

                Assert.Fail("A job offer shouldn't be published when it is invalid");
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Data[0].ToString() == DomainErrorMessages.RECRUITER_REQUIRED);
                Assert.AreEqual(JobOfferState.WorkInProgress, jobOffer.State);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task Publish_ThrowsInvalidOperationException_WhenTheJobOfferHasNotContractInformation()
        {
            //Arrange
            var jobOffer = new JobOffer
            {
                CompanyId = Guid.NewGuid().ToString(),
                RecruiterId = Guid.NewGuid().ToString(),
                State = JobOfferState.WorkInProgress
            };

            //Act
            try
            {
                await _service.PublishJobOffer(jobOffer);

                Assert.Fail("A job offer shouldn't be published when it is invalid");
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Data[0].ToString() == DomainErrorMessages.CONTRACT_INFORMATION_EMPTY);
                Assert.AreEqual(JobOfferState.WorkInProgress, jobOffer.State);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task SaveJobOfferAsync_StoreDataInDB_WhenJobOfferIsValid()
        {
            //Arrange

            var jobOffer = new JobOffer
            {
                ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" },
                RecruiterId = Guid.NewGuid().ToString(),
                CompanyId = Guid.NewGuid().ToString()
            };

            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _companyRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _jobOfferRepositoryMock.Setup(mock => mock.JobOfferBelongsToRecruiter(It.IsAny<JobOffer>())).Returns(Task.FromResult<bool>(true));

            _jobOfferRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _jobOfferRepositoryMock.Setup(mock => mock.UpsertAsync(It.IsAny<JobOffer>())).Returns(Task.CompletedTask);

            //Act
            await _service.SaveJobOfferAsync(jobOffer);

            //Assert
            _jobOfferRepositoryMock.VerifyAll();

        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task SaveJobOfferAsync_ThrowsInvalidOperationException_WhenRecruiterDoesNotExists()
        {
            //Arrange
            var jobOffer = new JobOffer
            {
                ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" },
                RecruiterId = Guid.NewGuid().ToString(),
                CompanyId = Guid.NewGuid().ToString()
            };

            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            try
            {
                await _service.SaveJobOfferAsync(jobOffer);

                //Assert
                Assert.Fail("It should throw an exception when the recruiter does not exists");
            }
            catch (InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(DomainErrorMessages.INVALID_RECRUITER, ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task SaveJobOfferAsync_ThrowsInvalidOperationException_WhenCompanyDoesNotExists()
        {
            //Arrange
            var jobOffer = new JobOffer
            {
                ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" },
                RecruiterId = Guid.NewGuid().ToString(),
                CompanyId = Guid.NewGuid().ToString()
            };

            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _companyRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

            //Act
            try
            {
                await _service.SaveJobOfferAsync(jobOffer);

                //Assert
                Assert.Fail("It should throw an exception when the company does not exists");
            }
            catch (InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(DomainErrorMessages.COMPANY_INVALID, ex.Message);
            }
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task SaveJobOfferAsync_ThrowsInvalidOperationException_WhenJobOfferWasNotCreatedByTheOriginalRecruiter()
        {
            //Arrange
            var jobOffer = new JobOffer
            {
                ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" },
                RecruiterId = Guid.NewGuid().ToString(),
                CompanyId = Guid.NewGuid().ToString()
            };

            _recruiterRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _companyRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _jobOfferRepositoryMock.Setup(mock => mock.CheckEntityExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _jobOfferRepositoryMock.Setup(mock => mock.JobOfferBelongsToRecruiter(It.IsAny<JobOffer>())).Returns(Task.FromResult<bool>(false));

            //Act
            try
            {
                await _service.SaveJobOfferAsync(jobOffer);

                //Assert
                Assert.Fail("It should throw an exception when the recruiter that is saving is not the same than the one who created it");
            }
            catch (InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(DomainErrorMessages.RECRUITER_WHO_SAVE_THE_JOBOFFER_SHOULD_BE_THE_SAME_THAN_CREATED_IT, ex.Message);
            }
        }

    }
}
