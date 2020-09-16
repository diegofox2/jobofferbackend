using System;
using System.Threading.Tasks;
using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.ApplicationServices.Test.Base;
using JobOfferBackend.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobOfferBackend.ApplicationServices.Test.IntegrationTest
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class AccountServiceIntegrationTest: IntegrationTestBase
    {
        private readonly AccountRepository _accountRepository;
        private readonly AccountService _accountService;

        public AccountServiceIntegrationTest()
        {
            _accountRepository = new AccountRepository(_database);
            _accountService = new AccountService(_accountRepository);
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
        public async Task SignUpAsync_CreatesANewAccount_WhenPasswordMatches()
        {
            //Act
            await _accountService.SignUpAsync("a@b.com", "test", "test", true);


            //Assert
            var account = await _accountRepository.GetAccountAsync("a@b.com", "test");
            
            Assert.IsNotNull(account);
        }

        [TestMethod]
        public async Task SignUpAsync_ThrowsInvalidOperationException_WhenPasswordDoesNotMatch()
        {
            try
            {
                //Act
                await _accountService.SignUpAsync("a@b.com", "test", "dsada", false);

                Assert.Fail("Creating a new account should fail when password does not match");
            }
            catch(InvalidOperationException ex)
            {
                Assert.AreEqual(ServicesErrorMessages.PASSWORD_CONFIRMATION_DOES_NOT_MATCH_WITH_PASSWORD, ex.Message);
            }
            
        }

        [TestMethod]
        public async Task SignUpAsync_ThrowsInvalidOperationException_WhenEmailAccountAlreadyExists()
        {
            //Arrange

            await _accountService.SignUpAsync("a@b.com", "test", "test", false);

            try
            {
                //Act
                await _accountService.SignUpAsync("a@b.com", "rere", "rere", false);

                Assert.Fail("Creating a new account should fail when email account already exists");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(ServicesErrorMessages.INVALID_USER_ACCOUNT, ex.Message);
            }

        }

    }
}
