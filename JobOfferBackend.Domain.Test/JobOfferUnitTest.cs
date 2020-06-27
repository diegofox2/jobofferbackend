﻿using JobOfferBackend.Domain.Constants;
using JobOfferBackend.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace JobOfferBackend.Domain.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class JobOfferUnitTest
    {
        [TestMethod]
        public void RecieveApplicant_CreatesJobApplication_WhenItIsTheFirstAttemptToApply()
        {
            //Arrange
            var person = new Person() { FirstName = "Pepe", LastName = "Lopez" };
            person.SetAbility(new Ability(new Skill() { Name = "C#" }, 5));

            var jobOffer = new JobOffer();
            jobOffer.AddSkillRequired(new SkillRequired(new Skill() { Name = "C#" }, 3, true));

            //Act
            jobOffer.RecieveApplicant(person);

            var jobApplication = jobOffer.Applications.Where(a => a.Applicant == person).SingleOrDefault();


            //Assert
            Assert.IsNotNull(jobApplication);
        }

        [TestMethod]
        public void RecieveApplicant_ThrowsInvalidOperationException_WhenApplicantAlreadyExists()
        {
            //Arrange
            var person = new Person() { FirstName = "Pepe", LastName = "Lopez" };
            person.SetAbility(new Ability(new Skill() { Name = "C#" }, 5));

            var jobOffer = new JobOffer();
            jobOffer.AddSkillRequired(new SkillRequired(new Skill() { Name = "C#" }, 3, true));
            
            jobOffer.RecieveApplicant(person);

            //Act
            try
            {
                jobOffer.RecieveApplicant(person);
                
                Assert.Fail("Job offer shoud throw exeption when a person tries to apply more than one time");
            }
            catch(InvalidOperationException ex)
            {
                Assert.AreEqual(ex.Message, DomainErrorMessages.APPLICANT_ALREADY_EXISTS);
            }
        }
    }
}
