using JobOfferBackend.Domain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobOfferBackend.Domain.Test.Base
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class BaseEntityModelTest
    {

        class SomeValueObject : BaseValueObject
        {
            public string AStringProp { get; set; }

            public override void Validate()
            {
                throw new System.NotImplementedException();
            }
        }

        class SomeEntity : BaseEntity<SomeEntity>
        {
            public string SomeProp { get; set; }

            public int SomeIntProp { get; set; }

            public SomeEntity EntityProp { get; set; }

            public SomeValueObject ValueObjectProp { get; set; }

            public override void Validate()
            {

            }
        }


        [TestMethod]
        public void AnEntity_IsEqualToAnotherOne_WhenBothHaveSameId()
        {
            //Arrange
            var a = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 1, SomeProp = "Val1" };
            var b = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 2, SomeProp = "Val2" };

            //Act Assert
            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void AnEntity_IsNotEqualToAnotherOne_WhenTheyHaveDifferentId()
        {
            //Arrange
            var a = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 1, SomeProp = "Val1" };
            var b = new SomeEntity() { Id = "aas545sd4dvbc", SomeIntProp = 1, SomeProp = "Val1" };

            //Act Assert
            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void GetHashCode_ReturnsSameValue_WhenTwoObjectsHasSameId()
        {
            //Arrange
            var a = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 1, SomeProp = "Val1" };
            var b = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 2, SomeProp = "Val2" };

            //Act Assert
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_ReturnsDifferentValues_WhenTwoObjectsHasDifferentId()
        {
            //Arrange
            var a = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 1, SomeProp = "Val1" };
            var b = new SomeEntity() { Id = "askjdjhgs", SomeIntProp = 2, SomeProp = "Val2" };

            //Act Assert
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }

        [TestMethod]
        public void HasSamePropertyValuesThan_ReturnsTrue_WhenAllPropertiesAreEqual()
        {
            //Arrange
            var a = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 1, SomeProp = "Val1" };
            var b = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 1, SomeProp = "Val1" };
            var c = new SomeEntity() { Id = "98778", SomeIntProp = 9, SomeProp = "val" };
            var d = new SomeValueObject() { AStringProp = "MyString" };
            var e = new SomeValueObject() { AStringProp = "MyString" };

            a.EntityProp = c;
            a.ValueObjectProp = d;
            b.EntityProp = c;
            b.ValueObjectProp = e;


            //Act Assert
            Assert.IsTrue(a.HasSamePropertyValuesThan(b), "");
        }
    }
}
