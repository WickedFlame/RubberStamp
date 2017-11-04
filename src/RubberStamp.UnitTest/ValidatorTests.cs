using System.Linq;
using RubberStamp.Rules;
using NUnit.Framework;

namespace RubberStamp.UnitTest
{
    [TestFixture]
    public class ValidatorTests
    {
        [Test]
        public void RubberStamp_Validator_AddRule_Test()
        {
            IValidator<TestClass> validator = new Validator<TestClass>();
            var rule = validator.AddRule(new ValidationRule<TestClass, string>(p => p.Name));

            Assert.AreSame(rule, validator.Rules.Single());
        }

        [Test]
        public void RubberStamp_Validator_AddRuleByObjectExpression_Test()
        {
            IValidator<TestClass> validator = new Validator<TestClass>();
            var rule = validator.AddRule(p => p.Name == string.Empty, "This is a error message");

            Assert.AreSame(rule, validator.Rules.Single());
        }

        [Test]
        public void RubberStamp_Validator_AddRuleByExpressionAndCondition_Test()
        {
            IValidator<TestClass> validator = new Validator<TestClass>();
            var rule = validator.AddRule(p => p.Name, con => con.IsNotNull());

            Assert.AreSame(rule, validator.Rules.Single());
        }

        [Test]
        public void RubberStamp_Validator_AddRuleByExpressionAndConditions_Test()
        {
            IValidator<TestClass> validator = new Validator<TestClass>();
            var rule = validator.AddRule(p => p.Name, con => con.IsNotNull(), con => con.IsEqual("Test"));

            Assert.AreSame(rule, validator.Rules.Single());
        }
        
        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }
        }
    }
}
