using JobOffer.ApplicationServices.Constants;
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
    public class RecruiterServiceIntegrationTest : IntegrationTestBase
    {
        private readonly RecruiterService _service;
        private readonly RecruiterRepository _recruiterRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly JobOfferRepository _jobOfferRepository;
        private readonly SkillRepository _skillRepository;

        public RecruiterServiceIntegrationTest()
        {
            _recruiterRepository = new RecruiterRepository(_database);
            _companyRepository = new CompanyRepository(_database);
            _jobOfferRepository = new JobOfferRepository(_database);
            _skillRepository = new SkillRepository(_database);

            _service = new RecruiterService(_companyRepository, _recruiterRepository, _jobOfferRepository);
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
        
        public async Task CreateCompany_SaveCompanySuccessfully_WhenCompanyDataIsCorrectAndCompanyDoesNotExists()
        {

            //Arrage
            var company = new Company("Acme", "Software");

            //Act
            await _service.CreateCompanyAsync(company);

            var savedCompany = await _companyRepository.GetCompanyAsync(company.Name, company.Activity);

            //Assert
            Assert.AreEqual(company, savedCompany, "Company was not saved");

        }

        [TestMethod]
        public async Task CreateCompany_ThrowsError_WhenCompanyAlreadyExists()
        {
            //Arrage
            var company = new Company("Acme", "Software");

            var otherCompany = new Company("Acme", "Software");

            //Act
            await _service.CreateCompanyAsync(company);


            try
            {
                await _service.CreateCompanyAsync(otherCompany);

                //Assert
                Assert.Fail("It should not allow creating a company that already exists");
            }
            catch(InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(ServicesErrorMessages.COMPANY_ALREADY_EXISTS, ex.Message);
            }

        }

        [TestMethod]
        public async Task CreateRecruiter_SaveCruiterSuccessfully_WhenRecruiterDataIsCorrectAndRecruiterNotExists()
        {
            //Arrange
            var recruiter = new Recruiter()
            {
                FirstName = "Patricia",
                LastName = "Maidana",
                IdentityCard = "28123456"
            };

            recruiter.AddClientCompany(new Company("Acme", "Software"));

            recruiter.AddJobHistory(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2015,5,1), true));
            recruiter.AddJobHistory(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2014, 1, 1), false, new DateTime(2015,4,30)));

            recruiter.AddStudy(new Study("UBA", "Lic.Relaciones del Trabajo", StudyStatus.Completed));

            //Act
            await _service.CreateRecruiterAsync(recruiter);

            var savedRecruiter = await _service.GetRecruiterAsync(recruiter);

            //Assert
            Assert.AreEqual(recruiter, savedRecruiter, "Recruiter was not saved");
        }

        [TestMethod]
        public async Task UpdateRecruiter_SaveCruiterSuccessfully_WhenRecruiterDataIsCorrectAndRecruiterExists()
        {
            //Arrange
            var recruiter = new Recruiter()
            {
                FirstName = "Patricia",
                LastName = "Maidana",
                IdentityCard = "28123456"
            };

            recruiter.AddClientCompany(new Company("Acme", "Software"));

            recruiter.AddJobHistory(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2015, 5, 1), true));
            recruiter.AddJobHistory(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2014, 1, 1), false, new DateTime(2015, 4, 30)));

            recruiter.AddStudy(new Study("UBA", "Lic.Relaciones del Trabajo", StudyStatus.Completed));

            await _service.CreateRecruiterAsync(recruiter);

            var savedRecruiter = await _service.GetRecruiterAsync(recruiter);

            //Act

            var jobToModify = savedRecruiter.JobHistory.Where(j => j.CompanyName == "Accenture" && j.From.Date == new DateTime(2014, 1, 1).Date).Single();

            jobToModify.CompanyName = "Globant";

            recruiter.UpdateJobHistory(jobToModify);

            await _service.UpdateRecruiterAsync(recruiter);

            var updatedRecruiter = await _service.GetRecruiterAsync(recruiter);

            //Assert
            Assert.AreEqual("Globant", updatedRecruiter.JobHistory.Single(j => j == jobToModify).CompanyName, "Company name of a recruiter was now updated");
        }

        [TestMethod]
        public async Task CreateJobOffer_SaveSuccessfully_WhenJobOfferDataIsCorrect()
        {
            //Arrange
            var recruiter = new Recruiter() { FirstName = "Maidana", LastName = "Patricia", IdentityCard = "28123456" };

            await _recruiterRepository.UpsertAsync(recruiter);

            var company = new Company("Acme", "Software");

            recruiter.AddClientCompany(company);
            
            var skill1 = new Skill() { Name = "C#" };
            var skill2 = new Skill() { Name = "Javascript" };
            var skill3 = new Skill() { Name = "React" };

            await _skillRepository.UpsertAsync(skill1);
            await _skillRepository.UpsertAsync(skill2);
            await _skillRepository.UpsertAsync(skill3);
            
            var jobOffer = new Domain.Entities.JobOffer() 
            { 
                Title = "Analista Funcional", 
                Description = "Se necesita analista funcional con bla bla bla",
                Owner = recruiter,
                Date = DateTime.Now.Date
            };
            
            jobOffer.AddSkillRequired(new SkillRequired(skill1, 5,true));
            jobOffer.AddSkillRequired(new SkillRequired(skill2, 4, false));
            jobOffer.AddSkillRequired(new SkillRequired(skill3, 2, false));
            

            //Act
            await _service.CreateJobOffer(jobOffer, recruiter);

            var jobOfferCreated = await _jobOfferRepository.GetByIdAsync(jobOffer.Id);


            //Assert
            Assert.AreEqual(jobOffer, jobOfferCreated, "Job offer was not saved" ); 

        }

    }
}
