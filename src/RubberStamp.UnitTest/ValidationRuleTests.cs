using RubberStamp.Conditions;
using RubberStamp.Rules;
using NUnit.Framework;

namespace RubberStamp.UnitTest
{
    [TestFixture]
    public class ValidationRuleTests
    {
        [Test]
        public void RubberStamp_ValidationRule_SetSeverity_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .SetSeverity(Severity.Info);

            Assert.AreEqual(Severity.Info, rule.Severity);
        }

        [Test]
        public void RubberStamp_ValidationRule_SetSeverity_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .SetSeverity(Severity.Info)
                .AddCondition(new IsNotNullCondition<TestClass, string>());

            var result = rule.Validate(new TestClass());

            Assert.AreEqual(Severity.Info, result.Severity);
        }

        [Test]
        public void RubberStamp_ValidationRule_SetSeverity_AfterCondition_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .AddCondition(new IsNotNullCondition<TestClass, string>())
                .SetSeverity(Severity.Info);

            var result = rule.Validate(new TestClass());

            Assert.AreEqual(Severity.Info, result.Severity);
        }

        [Test]
        public void RubberStamp_ValidationRule_SetSeverity_OnBase_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .AddCondition(new GreaterThanCondition<TestClass, int>(r => r.Index, 5))
                .SetSeverity(Severity.Info);

            var result = rule.Validate(new TestClass());

            Assert.AreEqual(Severity.Info, result.Severity);
        }

        [Test]
        public void RubberStamp_ValidationRule_DefaultSeverity_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .AddCondition(new IsNotNullCondition<TestClass, string>());

            var result = rule.Validate(new TestClass());

            Assert.AreEqual(Severity.Error, result.Severity);
        }

        [Test]
        public void RubberStamp_ValidationRule_DifferentCondition_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .AddCondition(new GreaterThanCondition<TestClass, int>(i => i.Index, 10));

            var result = rule.Validate(new TestClass());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void RubberStamp_ValidationRule_DifferentCondition_Valid_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .AddCondition(new IsEqualCondition<TestClass, string>(i => i.Firstname, "Test"));

            var result = rule.Validate(new TestClass { Firstname = "Test" });

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RubberStamp_ValidationRule_When_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .When(r => r.Firstname != null)
                .AddCondition(new IsEqualCondition<TestClass, string>(i => i.Firstname, "Tester"));

            var result = rule.Validate(new TestClass { Firstname = "Test" });

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void RubberStamp_ValidationRule_When_Valid_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .When(r => r.Firstname != null)
                .AddCondition(new IsEqualCondition<TestClass, string>(i => i.Firstname, "Tester"));

            var result = rule.Validate(new TestClass());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RubberStamp_ValidationRule_When_AfterCondition_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .AddCondition(new IsEqualCondition<TestClass, string>(i => i.Firstname, "Tester"))
                .When(r => r.Firstname != null)
                .AddCondition(new IsEqualCondition<TestClass, string>(i => i.Firstname, "Tester"));

            var result = rule.Validate(new TestClass { Firstname = "Test" });

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void RubberStamp_ValidationRule_When_Valid_AfterCondition_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .AddCondition(new IsEqualCondition<TestClass, string>(i => i.Firstname, "Tester"))
                .When(r => r.Firstname != null);

            var result = rule.Validate(new TestClass());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RubberStamp_ValidationRule_When_Valid_OnBase_WithValidation_Test()
        {
            var rule = new ValidationRule<TestClass, string>(r => r.Name)
                .AddCondition(new IsEqualCondition<TestClass, int>(i => i.Index, 5))
                .When(r => r.Firstname != null);

            var result = rule.Validate(new TestClass { Firstname = "test" });

            Assert.That(result, Is.Not.Null);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Firstname { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int Index { get; set; }
        }
    }
}
