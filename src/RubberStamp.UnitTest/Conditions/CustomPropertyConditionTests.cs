using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Conditions
{
    [TestFixture]
    public class CustomPropertyConditionTests
    {
        [Test]
        public void RubberStamp_Conditions_CustomPropertyCondition_String_Valid_Test()
        {
            var condition = new CustomPropertyCondition<TestClass, string>(c => c.Name, (c, p) => p != null);
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "TEST" }));
        }

        [Test]
        public void RubberStamp_Conditions_CustomPropertyCondition_String_Invalid_Test()
        {
            var condition = new CustomPropertyCondition<TestClass, string>(c => c.Name, (c, p) => p != null);
            Assert.IsFalse(condition.IsValid(new TestClass { Name = null }));
        }

        [Test]
        public void RubberStamp_Conditions_CustomPropertyCondition_Int_Valid_Test()
        {
            var condition = new CustomPropertyCondition<TestClass, int>(c => c.Index, (c, p) => p > 0);
            Assert.IsTrue(condition.IsValid(new TestClass { Index = 1 }));
        }

        [Test]
        public void RubberStamp_Conditions_CustomPropertyCondition_Int_Invalid_Test()
        {
            var condition = new CustomPropertyCondition<TestClass, int>(c => c.Index, (c, p) => p > 0);
            Assert.IsFalse(condition.IsValid(new TestClass { Index = 0 }));
        }
        
        [Test]
        public void RubberStamp_Conditions_CustomPropertyCondition_ValidateUnequalType_Valid_Test()
        {
            var condition = new CustomPropertyCondition<TestClass, string>(c => c.Name, (c, p) => p != null) as IValidationCondition<TestClass>;
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "TEST" }));
        }

        [Test]
        public void RubberStamp_Conditions_CustomPropertyCondition_Int_Message_Test()
        {
            var condition = new CustomPropertyCondition<TestClass, string>(c => c.Name, (c, p) => p != null);
            Assert.AreEqual("The Property Name caused a validation error", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_CustomPropertyCondition_String_Message_Test()
        {
            var condition = new CustomPropertyCondition<TestClass, int>(c => c.Index, (c, p) => p > 0);
            Assert.AreEqual("The Property Index caused a validation error", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_CustomPropertyCondition_SetMessage_Test()
        {
            var condition = new CustomPropertyCondition<TestClass, int>(c => c.Index, (c, p) => p > 0)
                .SetMessage("Error message");

            Assert.AreEqual("Error message", condition.Message);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int Index { get; set; }
        }
    }
}
