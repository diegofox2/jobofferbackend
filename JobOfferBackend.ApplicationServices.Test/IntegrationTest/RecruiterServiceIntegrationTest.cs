using JobOfferBackend.ApplicationServices.Constants;
using JobOfferBackend.ApplicationServices.Test.Base;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices.Test.IntegrationTest
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

            recruiter.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2015,5,1), true));
            recruiter.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2014, 1, 1), false, new DateTime(2015,4,30)));

            recruiter.SetStudy(new Study("UBA", "Lic.Relaciones del Trabajo", StudyStatus.Completed));

            var cSharp = new Skill() { Name = "C#" };
            var javascript = new Skill() { Name = "Javascript" };
            var react = new Skill() { Name = "React" };

            await _skillRepository.UpsertAsync(cSharp);
            await _skillRepository.UpsertAsync(javascript);
            await _skillRepository.UpsertAsync(react);

            recruiter.SetAbility(new Ability(cSharp, 10));
            recruiter.SetAbility(new Ability(javascript, 8));
            recruiter.SetAbility(new Ability(react, 7));

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

            recruiter.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2015, 5, 1), true));
            recruiter.SetPreviousJob(new Job("Accenture", "Sr.Talent Adquision", new DateTime(2014, 1, 1), false, new DateTime(2015, 4, 30)));

            recruiter.SetStudy(new Study("UBA", "Lic.Relaciones del Trabajo", StudyStatus.Completed));

            var cSharp = new Skill() { Name = "C#" };
            var javascript = new Skill() { Name = "Javascript" };
            var react = new Skill() { Name = "React" };

            await _skillRepository.UpsertAsync(cSharp);
            await _skillRepository.UpsertAsync(javascript);
            await _skillRepository.UpsertAsync(react);

            var cSharpAbility = new Ability(cSharp, 10);
            var javascriptAbility = new Ability(javascript, 8);

            recruiter.SetAbility(cSharpAbility);
            recruiter.SetAbility(javascriptAbility);

            await _service.CreateRecruiterAsync(recruiter);

            //Act

            var savedRecruiter = await _service.GetRecruiterAsync(recruiter);

            var previousJob = savedRecruiter.JobHistory.Where(j => j.CompanyName == "Accenture" && j.From.Date == new DateTime(2014, 1, 1).Date).Single();

            var newJob = (Job)previousJob.Clone();

            newJob.CompanyName = "Globant";

            savedRecruiter.SetPreviousJob(newJob, previousJob);

            var reactAbility = new Ability(react, 5);

            savedRecruiter.SetAbility(reactAbility, cSharpAbility);

            await _service.UpdateRecruiterAsync(savedRecruiter);

            var updatedRecruiter = await _service.GetRecruiterAsync(savedRecruiter);

            //Assert
            Assert.AreEqual("Globant", updatedRecruiter.JobHistory.Single(j => j == newJob).CompanyName, "Company name of a recruiter was now updated");
            Assert.IsTrue(updatedRecruiter.Abilities.Count() == 2);
            Assert.IsNotNull(updatedRecruiter.Abilities.SingleOrDefault(a => a.Skill == react));
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
            
            var jobOffer = new JobOffer() 
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
            await _service.CreateJobOfferAsync(jobOffer, recruiter.Id);

            var jobOfferCreated = await _jobOfferRepository.GetByIdAsync(jobOffer.Id);


            //Assert
            Assert.AreEqual(jobOffer, jobOfferCreated, "Job offer was not saved" ); 

        }

    }
}
