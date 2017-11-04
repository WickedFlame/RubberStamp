using RubberStamp.Conditions;
using RubberStamp.Rules;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Integration
{
    [TestFixture]
    public class CustomValidationRuleTests
    {
        [Test]
        public void Validation_Integration_CustomValidationRule_CustomEntityCondition_Test()
        {
            var rule = new CustomValidationRule<TestClass>("The validation failed")
                .AddCondition(new CustomEntityCondition<TestClass>(inst => inst.Index > 10));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.IsNotNull(result);
        }
        
        [Test]
        public void Validation_Integration_CustomValidationRule_CustomEntityCondition_Valid_Test()
        {
            var rule = new CustomValidationRule<TestClass>("The validation failed")
                .AddCondition(new CustomEntityCondition<TestClass>(inst => inst.Index < 10));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.IsNull(result);
        }

        [Test]
        public void Validation_Integration_CustomValidationRule_Message_CustomEntityCondition_Test()
        {
            var rule = new CustomValidationRule<TestClass>("The validation failed")
                .AddCondition(new CustomEntityCondition<TestClass>(inst => inst.Index > 10));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.AreEqual("The validation failed", result.Message);
        }
        
        [Test]
        public void Validation_Integration_CustomValidationRule_CustomPropertyCondition_Test()
        {
            var rule = new CustomValidationRule<TestClass>()
                .AddCondition(new CustomPropertyCondition<TestClass, int>(inst => inst.Index, (t, p) => p > 10));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.IsNotNull(result);
        }
        
        [Test]
        public void Validation_Integration_CustomValidationRule_CustomPropertyCondition_Valid_Test()
        {
            var rule = new CustomValidationRule<TestClass>()
                .AddCondition(new CustomPropertyCondition<TestClass, int>(inst => inst.Index, (t, p) => p < 10));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.IsNull(result);
        }

        [Test]
        public void Validation_Integration_CustomValidationRule_Message_CustomPrpertCondition_Test()
        {
            var rule = new CustomValidationRule<TestClass>()
                .AddCondition(new CustomPropertyCondition<TestClass, int>(inst => inst.Index, (t, p) => p > 10));

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.AreEqual("The Property Index caused a validation error", result.Message);
        }

        [Test]
        public void Validation_Integration_CustomValidationRule_CustomMessage_CustomPrpertCondition_Test()
        {
            var rule = new CustomValidationRule<TestClass>()
                .AddCondition(new CustomPropertyCondition<TestClass, int>(inst => inst.Index, (t, p) => p > 10))
                .SetMessage("The validation failed");

            var result = rule.Validate(new TestClass { Index = 5 });

            Assert.AreEqual("The validation failed", result.Message);
        }

        [Test]
        public void Validation_Integration_CustomValidationRule_CustomMessage_CustomPrpertCondition_WithExternalValue_Test()
        {
            const string Name = "test";
            var rule = new CustomValidationRule<TestClass>()
                .AddCondition(new CustomPropertyCondition<TestClass, string>(inst => inst.Name, (t, p) => p == Name))
                .SetMessage("The validation failed");

            var result = rule.Validate(new TestClass { Index = 5, Name = "test 2" });

            Assert.AreEqual("The validation failed", result.Message);
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
