using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.ApplicationServices.Test.Base;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Constants;
using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices.Test.IntegrationTest
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class RecruiterServiceIntegrationTest : IntegrationTestBase
    {
        private readonly RecruiterService _recruiterService;
        private readonly RecruiterRepository _recruiterRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly JobOfferRepository _jobOfferRepository;
        private readonly SkillRepository _skillRepository;
        private readonly PersonRepository _personRepository;
        private readonly AccountRepository _accountRepository;
        private readonly PersonService _personService;

        public RecruiterServiceIntegrationTest()
        {
            _recruiterRepository = new RecruiterRepository(_database);
            _companyRepository = new CompanyRepository(_database);
            _jobOfferRepository = new JobOfferRepository(_database);
            _skillRepository = new SkillRepository(_database);
            _personRepository = new PersonRepository(_database);
            _accountRepository = new AccountRepository(_database);

            _recruiterService = new RecruiterService(_companyRepository, _recruiterRepository, _jobOfferRepository, _personRepository,_accountRepository);
            _personService = new PersonService(_personRepository, _jobOfferRepository, _accountRepository);
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
        
        public async Task AddClientAsync_SaveCompanySuccessfullyAndAddsCompanyToRecruiterClients_WhenCompanyDataIsCorrectAndCompanyDoesNotExists()
        {

            //Arrange
            var person = new Person()
            {
                FirstName = "Patricia",
                LastName = "Maidana",
                IdentityCard = "28123456"
            };

            var recruiter = await _recruiterService.CreateRecruiterAsync(person);

            var company = new Company("Acme", "Software");

            //Act
            await _recruiterService.AddClientAsync(company, recruiter.Id);

            var savedCompany = await _companyRepository.GetByIdAsync(company.Id);
            recruiter = await _recruiterRepository.GetByIdAsync(recruiter.Id);

            //Assert
            Assert.AreEqual(company, savedCompany, "Company was not saved");
            Assert.IsTrue(recruiter.ClientCompaniesId.Any(item => item == company.Id), "Company was not added to client list");

        }

        [TestMethod]
        public async Task AddClientAsync_OnlyAddsAClientToRecruiter_WhenCompanyAlreadyExists()
        {
            //Arrage

            var person1 = new Person()
            {
                FirstName = "Patricia",
                LastName = "Maidana",
                IdentityCard = "28123456"
            };

            var person2 = new Person()
            {
                FirstName = "Carolina",
                LastName = "Leanza",
                IdentityCard = "28987561"
            };

            var recruiter1 = await _recruiterService.CreateRecruiterAsync(person1);
            var recruiter2 = await _recruiterService.CreateRecruiterAsync(person2);

            var company = new Company("Acme", "Software");
            var otherCompany = new Company("Mulesoft", "Software");

            await _recruiterService.AddClientAsync(company, recruiter1.Id);
            await _recruiterService.AddClientAsync(otherCompany, recruiter1.Id);

            //Act
            await _recruiterService.AddClientAsync(otherCompany, recruiter2.Id);
            recruiter1 = await _recruiterRepository.GetByIdAsync(recruiter1.Id);
            recruiter2 = await _recruiterRepository.GetByIdAsync(recruiter2.Id);

            //Assert
            Assert.IsTrue(await _companyRepository.CheckEntityExistsAsync(company.Id), "Company 1 was not recorded");
            Assert.IsTrue(await _companyRepository.CheckEntityExistsAsync(otherCompany.Id), "Company 2 was not recorded");
            Assert.AreEqual(2, recruiter1.ClientCompaniesId.Count(), "Recruiter1 does not have all the clientes linked" );
            Assert.AreEqual(1, recruiter2.ClientCompaniesId.Count(), "Recruiter2 does not have all the clientes linked");
            Assert.AreEqual(otherCompany.Id, recruiter2.ClientCompaniesId.First(), "Recruiter2 does not have linked the existing company");
        }

        [TestMethod]
        public async Task SaveJobOffer_CreatesSuccessfullyANewJobOffer_WhenJobOfferDataIsCorrect()
        {
            //Arrange
            
            var skill1 = new Skill() { Name = "C#" };
            var skill2 = new Skill() { Name = "Javascript" };
            var skill3 = new Skill() { Name = "React" };
            await _skillRepository.UpsertAsync(skill1);
            await _skillRepository.UpsertAsync(skill2);
            await _skillRepository.UpsertAsync(skill3);

            var person = new Person() { FirstName = "Maidana", LastName = "Patricia", IdentityCard = "28123456" };

            var recruiter = await _recruiterService.CreateRecruiterAsync(person);

            var company = new Company("Acme", "Software");

            await _recruiterService.AddClientAsync(company, recruiter.Id);

            var jobOffer = await _recruiterService.GetNewJobOffer(recruiter.Id);
            jobOffer.Title = "Analista Funcional";
            jobOffer.Description = "Se necesita analista funcional con bla bla bla";
            jobOffer.RecruiterId = recruiter.Id;
            jobOffer.Date = DateTime.Now.Date;

            jobOffer.ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" };

            jobOffer.AddSkillRequired(new SkillRequired(skill1, 5,true));
            jobOffer.AddSkillRequired(new SkillRequired(skill2, 4, false));
            jobOffer.AddSkillRequired(new SkillRequired(skill3, 2, false));
            jobOffer.CompanyId = company.Id;

            //Act
            await _recruiterService.SaveJobOfferAsync(jobOffer);

            var jobOfferSaved = await _jobOfferRepository.GetByIdAsync(jobOffer.Id);
            var companySaved = await _companyRepository.GetCompanyAsync(company.Name, company.Activity);

            //Assert
            Assert.AreEqual(companySaved.Id, jobOfferSaved.CompanyId, "The company created is different than the one assigned to the job offer");
            Assert.AreEqual(jobOffer, jobOfferSaved, "Job offer was not saved" );
            Assert.AreEqual(JobOfferState.WorkInProgress, jobOfferSaved.State, "Job offer created has a wrong state");
            Assert.AreEqual(recruiter.Id, jobOfferSaved.RecruiterId);

        }

        [TestMethod]
        public async Task SaveJobOffer_UpdatesAJobOfferSuccessfuly_WhenJobOfferExistsAndHasValidInformation()
        {
            //Arrange
            var person = new Person() { FirstName = "Maidana", LastName = "Patricia", IdentityCard = "28123456" };            
            var recruiter = await _recruiterService.CreateRecruiterAsync(person);

            var company = new Company("Acme", "Software");
            await _recruiterService.AddClientAsync(company, recruiter.Id);

            var skill1 = new Skill() { Name = "C#" };
            var skill2 = new Skill() { Name = "Javascript" };
            var skill3 = new Skill() { Name = "React" };

            await _skillRepository.UpsertAsync(skill1);
            await _skillRepository.UpsertAsync(skill2);
            await _skillRepository.UpsertAsync(skill3);

            var jobOffer = await _recruiterService.GetNewJobOffer(recruiter.Id);
            jobOffer.Title = "Analista Funcional";
            jobOffer.Description = "Se necesita analista funcional con bla bla bla";
            jobOffer.RecruiterId = recruiter.Id;
            jobOffer.Date = DateTime.Now.Date;

            jobOffer.ContractInformation = new ContractCondition() { KindOfContract = "FullTime", StartingFrom = "As soon as possible", WorkingDays = "Montay to Friday" };

            jobOffer.AddSkillRequired(new SkillRequired(skill1, 5, true));
            jobOffer.AddSkillRequired(new SkillRequired(skill2, 4, false));
            jobOffer.AddSkillRequired(new SkillRequired(skill3, 2, false));
            jobOffer.CompanyId = company.Id;

            await _recruiterService.SaveJobOfferAsync(jobOffer);

            var jobOfferSaved = await _jobOfferRepository.GetByIdAsync(jobOffer.Id);

            const string newTitle = "New title";

            //Act

            jobOfferSaved.Title = newTitle;

            await _recruiterService.SaveJobOfferAsync(jobOfferSaved);

            var jobOfferUpdated = await _jobOfferRepository.GetByIdAsync(jobOfferSaved.Id);

            //Assert
            Assert.AreEqual(newTitle, jobOfferSaved.Title);
            
        }
    }
}
