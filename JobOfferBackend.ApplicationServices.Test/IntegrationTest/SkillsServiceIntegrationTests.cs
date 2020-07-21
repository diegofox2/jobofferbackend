using JobOfferBackend.ApplicationServices.Test.Base;
using JobOfferBackend.DataAccess;
using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOfferBackend.ApplicationServices.Test.IntegrationTest
{
    [TestClass]
    [TestCategory("IntegrationTest")]
    public class SkillsServiceIntegrationTests : IntegrationTestBase
    {
        private readonly SkillRepository _skillRepository;
        private readonly SkillsService _skillsService;

        public SkillsServiceIntegrationTests()
        {
            _skillRepository = new SkillRepository(_database);
            _skillsService = new SkillsService(_skillRepository);
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
        public async Task GetAllSkills_ReturnsAllSkills_Always()
        {
            //Arrange

            var cSharp = new Skill() { Id = Guid.NewGuid().ToString(), Name = "C#" };
            var javascript = new Skill() { Id = Guid.NewGuid().ToString(), Name = "javascript" };

            await _skillRepository.UpsertAsync(cSharp);
            await _skillRepository.UpsertAsync(javascript);

            //Act
            var skills = await _skillsService.GetAllSkillsAsync();

            //Assert
            Assert.AreEqual(2, skills.Count());
            Assert.AreEqual(cSharp, skills.First());
            Assert.AreEqual(javascript, skills.Last());
        }
    }
}
