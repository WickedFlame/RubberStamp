using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Conditions
{
    [TestFixture]
    public class CustomEntityConditionTests
    {
        [Test]
        public void RubberStamp_Conditions_CustomEntityCondition_String_Valid_Test()
        {
            var condition = new CustomEntityCondition<TestClass>(c => c.Name != null);
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "TEST" }));
        }

        [Test]
        public void RubberStamp_Conditions_CustomEntityCondition_String_Invalid_Test()
        {
            var condition = new CustomEntityCondition<TestClass>(c => c.Name != null);
            Assert.IsFalse(condition.IsValid(new TestClass { Name = null }));
        }

        [Test]
        public void RubberStamp_Conditions_CustomEntityCondition_Int_Valid_Test()
        {
            var condition = new CustomEntityCondition<TestClass>(c => c.Index > 0);
            Assert.IsTrue(condition.IsValid(new TestClass { Index = 1 }));
        }

        [Test]
        public void RubberStamp_Conditions_CustomEntityCondition_Int_Invalid_Test()
        {
            var condition = new CustomEntityCondition<TestClass>(c => c.Index > 0);
            Assert.IsFalse(condition.IsValid(new TestClass { Index = 0 }));
        }
        
        [Test]
        public void RubberStamp_Conditions_CustomEntityCondition_ValidateUnequalType_Valid_Test()
        {
            var condition = new CustomEntityCondition<TestClass>(c => c.Name != null) as IValidationCondition<TestClass>;
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "TEST" }));
        }

        [Test]
        public void RubberStamp_Conditions_CustomEntityCondition_Message_Test()
        {
            var condition = new CustomEntityCondition<TestClass>(c => c.Name != null);
            Assert.AreEqual("The Validation failed for the Entity TestClass", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_CustomEntityCondition_SetMessage_Test()
        {
            var condition = new CustomEntityCondition<TestClass>(c => c.Name != null)
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
