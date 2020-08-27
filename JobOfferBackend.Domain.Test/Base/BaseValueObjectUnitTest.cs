using JobOfferBackend.Domain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace JobOfferBackend.Domain.Test.Base
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class BaseValueObjectUnitTest
    {
        class SomeClass
        {
            public string MyProp { get; set; }
        }

        class SomeValueObject : BaseValueObject
        {

            public string SomeStringProp { get; set; }

            public int SomeIntProp { get; set; }

            public SomeClass SomeClassProperty { get; set; }

            public List<SomeClass> SomeClassList { get; set; }

            public override void Validate()
            {

            }
        }

        class ValueObjectWithAnotherValueObject : BaseValueObject
        {
            public string MyProp { get; set; }

            public SomeValueObject SomeValueObjectProperty { get; set; }

            public override void Validate()
            {
            }
        }


        [TestMethod]
        public void AValueObject_IsEqualToAnotherOne_WhenBothHaveSamePropertyValues()
        {
            //Arrange
            var objectClass = new SomeClass() { MyProp = "Test" };

            var a = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString", SomeClassProperty = objectClass };

            var b = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString", SomeClassProperty = objectClass };

            //Assert
            Assert.IsTrue(a == b);

        }

        [TestMethod]
        public void AValueObject_IsNotEqualToAnotherOne_WhenHaveAnyPropertyWithDifferentValue()
        {
            //Arrange
            var objectClass = new SomeClass() { MyProp = "Test" };

            var a = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString", SomeClassProperty = objectClass };

            var b = new SomeValueObject() { SomeIntProp = 2, SomeStringProp = "MyString", SomeClassProperty = objectClass };

            //Assert
            Assert.IsTrue(a != b);

            //Arrange
            b.SomeIntProp = 1;
            b.SomeStringProp = "OtherValue";

            //Assert
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void AValueObject_IsNotEqualToAnotherOne_WhenHaveAnPropertyByReferenceWithDifferentInstances()
        {
            //Arrange
            var objectClass = new SomeClass() { MyProp = "Test" };
            var objectClass2 = new SomeClass() { MyProp = "Test" };

            var a = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString", SomeClassProperty = objectClass };

            var b = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString", SomeClassProperty = objectClass2 };

            //Assert
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void AValueObject_IsNotEqualToAnotherOne_WhenOneOfThemHasNotValueButTheOtheroneYes()
        {
            //Arrange
            var objectClass = new SomeClass() { MyProp = "Test" };

            var a = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString", SomeClassProperty = objectClass };

            var b = new SomeValueObject() { SomeStringProp = "MyString", SomeClassProperty = objectClass };

            //Assert
            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void AValueObject_IsEqualToAnotherOne_WhenBothHaveNotValueForSameProperties()
        {
            //Arrange
            var a = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString" };

            var b = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString" };

            //Assert
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void AValueObject_IsEqualToAnotherOne_WhenBothAValueObjectPropertyWithSameValues()
        {
            //Arrange
            var a = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString" };

            var b = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString" };

            //Assert
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void AValueObject_IsNotEqualToAnotherOne_WhenBothAValueObjectPropertyButWithDifferentValues()
        {
            //Arrange
            var a = new SomeValueObject() { SomeIntProp = 1, SomeStringProp = "MyString" };

            var b = new SomeValueObject() { SomeIntProp = 2, SomeStringProp = "MyString" };

            //Assert
            Assert.IsTrue(a != b);
        }
    }
}
