using System.Linq;
using RubberStamp.Rules;
using NUnit.Framework;
using RubberStamp.Conditions;

namespace RubberStamp.UnitTest
{
    [TestFixture]
    public class ValidatorTests
    {
        [Test]
        public void RubberStamp_Validator_AddRule_Test()
        {
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new IsNotNullCondition<TestClass, string>(p => p.Name));

            IValidator<TestClass> validator = new Validator<TestClass>()
                .AddRule(rule);

            Assert.AreSame(rule, validator.Rules.Single());
            Assert.That(validator.Rules.Single().Conditions.Single(), Is.Not.Null);
        }

        [Test]
        public void RubberStamp_Validator_AddRuleByObjectExpression_Test()
        {
            IValidator<TestClass> validator = new Validator<TestClass>();
            validator.AddRule(p => p.Name == string.Empty, "This is a error message");

            var rule = validator.Rules.Single();
            Assert.That(rule, Is.Not.Null);
            Assert.That(rule.Conditions.Single(), Is.Not.Null);
        }

        [Test]
        public void RubberStamp_Validator_AddRuleByExpressionAndCondition_Test()
        {
            IValidator<TestClass> validator = new Validator<TestClass>();
            validator.AddRule(p => p.Name, con => con.IsNotNull());

            var rule = validator.Rules.Single();
            Assert.That(rule, Is.Not.Null);
            Assert.That(rule.Conditions.Single(), Is.Not.Null);
        }

        [Test]
        public void RubberStamp_Validator_AddRuleByExpressionAndConditions_Test()
        {
            IValidator<TestClass> validator = new Validator<TestClass>();
            validator.AddRule(p => p.Name, con => con.Conditions(c=> c.IsNotNull(), c => c.IsEqual("Test")));

            var rule = validator.Rules.Single();
            Assert.That(rule, Is.Not.Null);
            Assert.That(rule.Conditions.Count(), Is.EqualTo(2));
        }

        private class TestClass
        {
            public string Name { get; set; }
        }
    }
}
