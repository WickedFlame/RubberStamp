using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Conditions
{
    [TestFixture]
    public class LengthLessOrEqualThanConditionTests
    {
        [Test]
        public void RubberStamp_Conditions_LengthLessOrEqualThanCondition_String_Equal_Test()
        {
            var condition = new LengthLessOrEqualThanCondition<TestClass, string>(p => p.Name, 3);
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "cde" }));
        }

        [Test]
        public void RubberStamp_Conditions_LengthLessOrEqualThanCondition_String_Greater_Test()
        {
            var condition = new LengthLessOrEqualThanCondition<TestClass, string>(p => p.Name, 2);
            Assert.IsFalse(condition.IsValid(new TestClass { Name = "cdf" }));
        }

        [Test]
        public void RubberStamp_Conditions_LengthLessOrEqualThanCondition_String_Lower_Invalid_Test()
        {
            var condition = new LengthLessOrEqualThanCondition<TestClass, string>(p => p.Name, 4);
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "cce" }));
        }

        [Test]
        public void RubberStamp_Conditions_LengthLessOrEqualThanCondition_String_Message_Test()
        {
            var condition = new LengthLessOrEqualThanCondition<TestClass, string>(p => p.Name, 5);
            Assert.AreEqual("The Property Name max length should be 5", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_LengthLessOrEqualThanCondition_SetMessage_Test()
        {
            var condition = new LengthLessOrEqualThanCondition<TestClass, string>(p => p.Name, 5)
                .SetMessage("Error message");

            Assert.AreEqual("Error message", condition.Message);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }
        }
    }
}
