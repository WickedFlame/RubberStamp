using System;
using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Conditions
{
    [TestFixture]
    public class IsNotNullConditionTests
    {
        [Test]
        public void RubberStamp_Conditions_IsNotNullCondition_String_NotNull_Test()
        {
            var condition = new IsNotNullCondition<TestClass, string>(t => t.Name);
            Assert.IsTrue(condition.IsValid(new TestClass { Name = "cde" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsNotNullCondition_String_Null_Test()
        {
            var condition = new IsNotNullCondition<TestClass, string>(t => t.Name);
            Assert.IsFalse(condition.IsValid(new TestClass()));
        }

        [Test]
        public void RubberStamp_Conditions_IsNotNullCondition_String_Argument_Null_Test()
        {
            var condition = new IsNotNullCondition<TestClass, string>(t => t.Name);
            Assert.Throws<ArgumentNullException>(() => condition.IsValid(null));
        }

        [Test]
        public void RubberStamp_Conditions_IsNotNullCondition_NotNullableType_Null_Test()
        {
            int index = 1;

            var condition = new IsNotNullCondition<TestClass, int>(t => t.Index);
            Assert.IsTrue(condition.IsValid(new TestClass { Index = index }));
        }

        [Test]
        public void RubberStamp_Conditions_IsNotNullCondition_Message_Test()
        {
            var condition = new IsNotNullCondition<TestClass, string>(t => t.Name);
            Assert.AreEqual("The Property Name is not allowed to be null", condition.Message);
        }

        [Test]
        public void RubberStamp_Conditions_IsNotNullCondition_SetMessage_Test()
        {
            var condition = new IsNotNullCondition<TestClass, string>(t => t.Name)
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
