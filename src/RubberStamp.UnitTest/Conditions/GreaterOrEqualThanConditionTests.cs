using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Conditions
{
    [TestFixture]
    public class GreaterOrEqualThanConditionTests
    {
        [Test]
        public void RubberStamp_Conditions_GreaterOrEqualThanCondition_String_Equal_Test()
        {
            var condition = new GreaterOrEqualThanCondition<TestClass, string>(p => p.Name, "cde");
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "cde" }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterOrEqualThanCondition_String_Greater_Test()
        {
            var condition = new GreaterOrEqualThanCondition<TestClass, string>(p => p.Name, "cde");
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "cdf" }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterOrEqualThanCondition_String_Lower_Invalid_Test()
        {
            var condition = new GreaterOrEqualThanCondition<TestClass, string>(p => p.Name, "cde");
            Assert.IsFalse(condition.IsValid(new TestClass { Name = "cce" }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterOrEqualThanCondition_Int_Equal_Test()
        {
            var condition = new GreaterOrEqualThanCondition<TestClass, int>(p => p.Index, 3);
            Assert.IsTrue(condition.IsValid(new TestClass { Index = 3 }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterOrEqualThanCondition_Int_Greater_Test()
        {
            var condition = new GreaterOrEqualThanCondition<TestClass, int>(p => p.Index, 3);
            Assert.IsTrue(condition.IsValid(new TestClass { Index = 4 }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterOrEqualThanCondition_Int_Lower_Invalid_Test()
        {
            var condition = new GreaterOrEqualThanCondition<TestClass, int>(p => p.Index, 3);
            Assert.IsFalse(condition.IsValid(new TestClass { Index = 2 }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterOrEqualThanCondition_Int_Message_Test()
        {
            var condition = new GreaterOrEqualThanCondition<TestClass, int>(p => p.Index, 3);
            Assert.AreEqual("The Property Index has to be greater than or equal to 3", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_GreaterOrEqualThanCondition_String_Message_Test()
        {
            var condition = new GreaterOrEqualThanCondition<TestClass, string>(p => p.Name, "TEST");
            Assert.AreEqual("The Property Name has to be greater than or equal to TEST", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_GreaterOrEqualThanCondition_SetMessage_Test()
        {
            var condition = new GreaterOrEqualThanCondition<TestClass, string>(p => p.Name, "TEST")
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
