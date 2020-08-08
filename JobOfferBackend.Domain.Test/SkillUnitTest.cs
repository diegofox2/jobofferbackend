using JobOfferBackend.Domain.Constants;
using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace JobOfferBackend.Domain.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class SkillUnitTest
    {
        [TestMethod]
        public void Validate_ThrowsInvalidOperationException_WhenNameIsNullOrEmpty()
        {
            //Arrange
            var skill = new Skill();

            try
            {
                //Act
                skill.Validate();

                Assert.Fail("A skill must be invalid when name is null or empty");
            }
            catch(InvalidOperationException ex)
            {
                //Assert
                Assert.AreEqual(1, ex.Data.Count);
                Assert.IsTrue(ex.Data.Values.Cast<string>().Contains(DomainErrorMessages.NAME_REQUIRED));
            }

        }

    }
}
