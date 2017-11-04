using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Conditions
{
    [TestFixture]
    public class GreaterThanConditionTests
    {
        [Test]
        public void RubberStamp_Conditions_GreaterThanCondition_String_Equal_Test()
        {
            var condition = new GreaterThanCondition<TestClass, string>(t => t.Name, "cde");
            Assert.IsFalse(condition.IsValid(new TestClass { Name = "cde" }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterThanCondition_String_Greater_Test()
        {
            var condition = new GreaterThanCondition<TestClass, string>(t => t.Name, "cde");
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "cdf" }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterThanCondition_String_Lower_Invalid_Test()
        {
            var condition = new GreaterThanCondition<TestClass, string>(t => t.Name, "cde");
            Assert.IsFalse(condition.IsValid(new TestClass { Name = "cce" }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterThanCondition_Int_Equal_Test()
        {
            var condition = new GreaterThanCondition<TestClass, int>(t => t.Index, 3);
            Assert.IsFalse(condition.IsValid(new TestClass { Index = 3 }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterThanCondition_Int_Greater_Test()
        {
            var condition = new GreaterThanCondition<TestClass, int>(t => t.Index, 3);
            Assert.IsTrue(condition.IsValid(new TestClass { Index = 4 }));
        }

        [Test]
        public void RubberStamp_Conditions_GreaterThanCondition_Int_Lower_Invalid_Test()
        {
            var condition = new GreaterThanCondition<TestClass, int>(t => t.Index, 3);
            Assert.IsFalse(condition.IsValid(new TestClass { Index = 2 }));
        }
        
        [Test]
        public void RubberStamp_Conditions_GreaterThanCondition_Int_Message_Test()
        {
            var condition = new GreaterThanCondition<TestClass, int>(t => t.Index, 3);
            Assert.AreEqual("The Property Index has to be greater than 3", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_GreaterThanCondition_String_Message_Test()
        {
            var condition = new GreaterThanCondition<TestClass, string>(t => t.Name, "TEST");
            Assert.AreEqual("The Property Name has to be greater than TEST", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_GreaterThanCondition_SetMessage_Test()
        {
            var condition = new GreaterThanCondition<TestClass, string>(t => t.Name, "TEST")
                .SetMessage("Error message");

            Assert.AreEqual("Error message", condition.Message);
        }

        private class TestClass
        {
            public string Name { get; set; }

            public int Index { get; set; }
        }
    }
}
