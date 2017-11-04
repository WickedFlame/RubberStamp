using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Conditions
{
    [TestFixture]
    public class IsNullConditionTests
    {
        [Test]
        public void RubberStamp_Conditions_IsNullCondition_String_NotNull_Test()
        {
            var condition = new IsNullCondition<TestClass, string>(p => p.Name);
            Assert.IsFalse(condition.IsValid(new TestClass { Name = "cde" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsNullCondition_String_Null_Test()
        {
            var condition = new IsNullCondition<TestClass, string>(p => p.Name);
            Assert.IsTrue(condition.IsValid(new TestClass { Name = null }));
        }

        [Test]
        public void RubberStamp_Conditions_IsNullCondition_NotNullableType_Null_Test()
        {
            int index = 1;

            var condition = new IsNullCondition<TestClass, int>(p => p.Index);
            Assert.IsFalse(condition.IsValid(new TestClass { Index = index }));
        }

        [Test]
        public void RubberStamp_Conditions_IsNullCondition_Message_Test()
        {
            var condition = new IsNullCondition<TestClass, string>(p => p.Name);
            Assert.AreEqual("The Property Name has to be null", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_IsNullCondition_SetMessage_Test()
        {
            var condition = new IsNullCondition<TestClass, string>(p => p.Name)
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
