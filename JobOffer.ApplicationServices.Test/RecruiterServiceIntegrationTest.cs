using JobOffer.ApplicationServices.Test.Base;
using JobOffer.DataAccess;
using JobOffer.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace JobOffer.ApplicationServices.Test
{
    [TestClass]
    public class RecruiterServiceIntegrationTest : IntegrationTestBase
    {
        private readonly RecruiterService _service;
        private readonly RecruiterRepository _recruiterRepository;
        private readonly CompanyRepository _companyRepository;

        public RecruiterServiceIntegrationTest()
        {
            _recruiterRepository = new RecruiterRepository(_database);
            _companyRepository = new CompanyRepository(_database);
            _service = new RecruiterService(_companyRepository, _recruiterRepository);
        }

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            SetupIntegrationTest.OneTimeSetUp();
        }

        [ClassCleanup]
        public static void Clean()
        {
            SetupIntegrationTest.OneTimeTearDown();
        }

        [TestMethod]
        public async Task CreateCompany_SaveCompanySuccessfully_WhenCompanyDataIsSuccessAndCompanyDoesNotExists()
        {

            //Arrage
            var company = new Company("Acme", "Software");

            //Act
            await _service.CreateCompanyAsync(company);
            var savedCompany = await _companyRepository.GetCompanyAsync(company.Name, company.Activity);

            //Assert
            Assert.IsNotNull(savedCompany);
        }


    }
}
