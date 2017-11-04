using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Conditions
{
    [TestFixture]
    public class RegexConditionTests
    {
        [Test]
        public void RubberStamp_Conditions_RegexCondition_Regex_Validate_Number_Test()
        {
            var condition = new RegexCondition<TestClass, string>(p => p.Index.ToString(), @"^[0-9]");
            Assert.IsTrue(condition.IsValid(new TestClass { Index = 1234567890 }));
        }

        [Test]
        public void RubberStamp_Conditions_RegexCondition_Regex_Validate_Characters_Test()
        {
            var condition = new RegexCondition<TestClass, string>(p => p.Email, @"^[a-zA-Z]");
            Assert.IsTrue(condition.IsValid(new TestClass { Email = "abcABC" }));
        }

        [Test]
        public void RubberStamp_Conditions_RegexCondition_Regex_InValid_Number_Test()
        {
            var condition = new RegexCondition<TestClass, string>(p => p.Email, @"^[0-9]");
            Assert.IsFalse(condition.IsValid(new TestClass { Email = "test" }));
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Email { get; set; }

            public int Index { get; set; }
        }
    }
}
