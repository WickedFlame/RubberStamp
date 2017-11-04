using System;
using System.Text;
using RubberStamp.Conditions;
using RubberStamp.Rules;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Integration
{
    [TestFixture]
    public class ValidationRuleTests
    {
        [Test]
        public void Validation_Integration_ValidationRule_Null_Test()
        {
            var rule = new ValidationRule<TestClass, string>(i => i.Name)
                .AddCondition(new IsNotNullCondition<TestClass, string>());

            var result = rule.Validate(new TestClass());

            Assert.IsNotNull(result);
        }

        [Test]
        public void Validation_Integration_ValidationRule_Null_Valid_Test()
        {
            var rule = new ValidationRule<TestClass, string>(i => i.Name)
                .AddCondition(new IsNotNullCondition<TestClass, string>());

            var result = rule.Validate(new TestClass { Name = "Test" });

            Assert.IsNull(result);
        }

        [Test]
        public void Validation_Integration_ValidationRule_Message_Null_Test()
        {
            var rule = new ValidationRule<TestClass, string>(i => i.Name)
                .AddCondition(new IsNotNullCondition<TestClass, string>());

            var result = rule.Validate(new TestClass());

            var expected = new StringBuilder();
            expected.Append("The Property Name is not allowed to be null");

            Assert.AreEqual(expected.ToString(), result.Message);
        }

        [Test]
        public void Validation_Integration_ValidationRule_GreaterThan_Test()
        {
            var rule = new ValidationRule<TestClass, int>(i => i.Index)
                .AddCondition(new GreaterThanCondition<TestClass, int>(t => t.Index, 1));

            var result = rule.Validate(new TestClass());

            Assert.IsNotNull(result);
        }

        [Test]
        public void Validation_Integration_ValidationRule_GreaterThan_Valid_Test()
        {
            var rule = new ValidationRule<TestClass, int>(i => i.Index)
                .AddCondition(new GreaterThanCondition<TestClass, int>(t => t.Index, 1));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.IsNull(result);
        }

        [Test]
        public void Validation_Integration_ValidationRule_Message_GreaterThan_Test()
        {
            var rule = new ValidationRule<TestClass, int>(i => i.Index)
                .AddCondition(new GreaterThanCondition<TestClass, int>(t => t.Index, 1));

            var result = rule.Validate(new TestClass());

            var expected = new StringBuilder();
            expected.Append("The Property Index has to be greater than 1");

            Assert.AreEqual(expected.ToString(), result.Message);
        }

        [Test]
        public void Validation_Integration_ValidationRule_SmallerThan_Test()
        {
            var rule = new ValidationRule<TestClass, int>(i => i.Index)
                .AddCondition(new LessThanCondition<TestClass, int>(1));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.IsNotNull(result);
        }

        [Test]
        public void Validation_Integration_ValidationRule_SmallerThan_Valid_Test()
        {
            var rule = new ValidationRule<TestClass, int>(i => i.Index)
                .AddCondition(new LessThanCondition<TestClass, int>(1));

            var result = rule.Validate(new TestClass());

            Assert.IsNull(result);
        }

        [Test]
        public void Validation_Integration_ValidationRule_Message_SmallerThan_Test()
        {
            var rule = new ValidationRule<TestClass, int>(i => i.Index)
                .AddCondition(new LessThanCondition<TestClass, int>(1));

            var result = rule.Validate(new TestClass { Index = 5 });

            var expected = new StringBuilder();
            expected.Append("The Property Index has to be less than 1");

            Assert.AreEqual(expected.ToString(), result.Message);
        }

        [Test]
        public void Validation_Integration_ValidationRule_Equals_Test()
        {
            var rule = new ValidationRule<TestClass, int>(i => i.Index)
                .AddCondition(new IsEqualCondition<TestClass, int>(1));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.IsNotNull(result);
        }

        [Test]
        public void Validation_Integration_ValidationRule_Equals_Valid_Test()
        {
            var rule = new ValidationRule<TestClass, int>(i => i.Index)
                .AddCondition(new IsEqualCondition<TestClass, int>(5));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.IsNull(result);
        }

        [Test]
        public void Validation_Integration_ValidationRule_Message_Equals_Test()
        {
            var rule = new ValidationRule<TestClass, int>(i => i.Index)
                .AddCondition(new IsEqualCondition<TestClass, int>(1));

            var result = rule.Validate(new TestClass { Index = 5 });

            var expected = new StringBuilder();
            expected.Append("The Property Index has to be equal to 1");

            Assert.AreEqual(expected.ToString(), result.Message);
        }

        [Test]
        public void Validation_Integration_ValidationRule_Message_Multiple_Conditions_Test()
        {
            var rule =
                new ValidationRule<TestClass, int>(i => i.Index).AddCondition(
                    new GreaterThanCondition<TestClass, int>(1)).AddCondition(new LessThanCondition<TestClass, int>(-1));

            var result = rule.Validate(new TestClass());

            var expected = new StringBuilder();
            expected.AppendLine("The Property Index has to be greater than 1");
            expected.Append("The Property Index has to be less than -1");

            Assert.AreEqual(expected.ToString(), result.Message);
        }

        [Test]
        public void Validation_Integration_ValidationRule_Message_CustomMessage_Test()
        {
            var rule =
                new ValidationRule<TestClass, int>(i => i.Index).AddCondition(
                    new GreaterThanCondition<TestClass, int>(t => t.Index, 1))
                    .SetMessage("This is a custom message")
                    .SetMessageHeader("This is message header\\r\\n");

            var result = rule.Validate(new TestClass());

            Assert.AreEqual(result.Message, "This is message header\\r\\nThis is a custom message");
        }

        [Test]
        public void Validation_Integration_ValidationRule_UnequalTypeInCondition_Test()
        {
            var rule = new ValidationRule<TestClass, string>(i => i.Name)
                .AddCondition(new IsNotNullCondition<TestClass, int>());

            Assert.Throws<NullReferenceException>(() => rule.Validate(new TestClass()));
        }

        private class TestClass
        {
            public string Name { get; set; }

            public int Index { get; set; }
        }
    }
}
