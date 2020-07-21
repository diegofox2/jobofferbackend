using JobOfferBackend.Domain.Security.Constants;
using JobOfferBackend.Doman.Security.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace JobOfferBackend.Domain.Security.Test
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class AccountUnitTest
    {
        [TestMethod]
        public void Validate_ThrowsInvalidOperationExeption_WhenEmailIsEmpty()
        {
            //Arrange
            var account = new Account() { Password = "test" };

            try
            {
                //Act
                account.Validate();

                Assert.Fail("Account should be invalid when email is empty");

            }
            catch(Exception ex)
            {
                //Assert
                Assert.AreEqual(1, ex.Data.Count);
                Assert.IsTrue(ex.Data.Contains(DomainErrorMessages.EMAIL_CANT_BE_EMPTY));
            }
        }

        [TestMethod]
        public void Validate_ThrowsInvalidOperationExeption_WhenPasswordIsEmpty()
        {
            //Arrange
            var account = new Account() { Email ="a@b.com" };

            try
            {
                //Act
                account.Validate();

                Assert.Fail("Account should be invalid when password is empty");

            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual(1, ex.Data.Count);
                Assert.IsTrue(ex.Data.Contains(DomainErrorMessages.PASSWORD_CANT_BE_EMPTY));
            }
        }

        [TestMethod]
        public void Validate_ThrowsInvalidOperationExeption_WhitTwoErrorMessages_WhenPasswordIsEmpty()
        {
            //Arrange
            var account = new Account();

            try
            {
                //Act
                account.Validate();

                Assert.Fail("Account should be invalid when password and email are empty");

            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual(2, ex.Data.Count);
                Assert.IsTrue(ex.Data.Contains(DomainErrorMessages.EMAIL_CANT_BE_EMPTY));
                Assert.IsTrue(ex.Data.Contains(DomainErrorMessages.PASSWORD_CANT_BE_EMPTY));
            }
        }
    }
}
