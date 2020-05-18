using JobOfferBackend.Domain.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JobOfferBackend.Domain.Test.Base
{
    [TestClass]
    [TestCategory("UnitTest")]
    public class BaseEntityModelTest
    {

        class SomeEntity : BaseEntity<SomeEntity>
        {
            public string SomeProp { get; set; }

            public int SomeIntProp { get; set; }

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

            Assert.IsTrue(a == b);
        }

        [TestMethod]
        public void AnEntity_IsNotEqualToAnotherOne_WhenTheyHaveDifferentId()
        {
            //Arrange
            var a = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 1, SomeProp = "Val1" };
            var b = new SomeEntity() { Id = "aas545sd4dvbc", SomeIntProp = 1, SomeProp = "Val1" };

            Assert.IsFalse(a == b);
        }

        [TestMethod]
        public void GetHashCode_ReturnsSameValue_WhenTwoObjectsHasSameId()
        {
            //Arrange
            var a = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 1, SomeProp = "Val1" };
            var b = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 2, SomeProp = "Val2" };

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_ReturnsDifferentValues_WhenTwoObjectsHasDifferentId()
        {
            //Arrange
            var a = new SomeEntity() { Id = "dasjkl546jh", SomeIntProp = 1, SomeProp = "Val1" };
            var b = new SomeEntity() { Id = "askjdjhgs", SomeIntProp = 2, SomeProp = "Val2" };

            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
