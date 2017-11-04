using System.Linq;
using System.Text;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Integration
{
    [TestFixture]
    public class InheritedValidatorTest
    {
        [Test]
        public void RubberStamp_Validator_InheritedValidator_Valid_Test()
        {
            var validator = new PersonValidator();
            var summary = validator.Validate(new Person("Taylor", "Ted", "New York"));

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_InheritedValidator_Invalid_Test()
        {
            var validator = new PersonValidator();
            var summary = validator.Validate(new Person("Taylor", "Tim", "New York"));

            Assert.IsFalse(summary.IsValid);
            Assert.AreEqual("Tim Taylor is not alowed", summary.ValidationResults.First().Message);
        }

        [Test]
        public void RubberStamp_Validator_InheritedValidator_Extended_Rule_Valid_Test()
        {
            var validator = new PersonValidator();
            validator.AddRule(p => p.City, con => con.IsEqual("New York"));
            var summary = validator.Validate(new Person("Taylor", "Ted", "New York"));

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_InheritedValidator_Extended_Rule_Invalid_Test()
        {
            var validator = new PersonValidator();
            validator.AddRule(p => p.City, con => con.IsEqual("Mexico City"));
            
            var summary = validator.Validate(new Person("Taylor", "Ted", "New York"));

            Assert.IsFalse(summary.IsValid);

            var expected = new StringBuilder();
            expected.Append("The Property City has to be equal to Mexico City");

            Assert.AreEqual(expected.ToString(), summary.ValidationMessage);
        }

        [Test]
        public void RubberStamp_Validator_InheritedValidator_Extended_Rule_CustomMessage_Invalid_Test()
        {
            var validator = new PersonValidator();
            validator.AddRule(p => p.City, con => con.IsEqual("Mexico City")).SetMessage("The City has to be Mexico City");
            var summary = validator.Validate(new Person("Taylor", "Ted", "New York"));

            Assert.IsFalse(summary.IsValid);
            Assert.AreEqual("The City has to be Mexico City", summary.ValidationResults.First().Message);
        }

        [Test]
        public void RubberStamp_Validator_InheritedValidator_Extended_Rule_WithWhen_Valid_Test()
        {
            var validator = new PersonValidator();
            validator.AddRule(p => p.City, con => con.IsEqual("Mexico City")).When(p => p.Firstname != "Ted");
            var summary = validator.Validate(new Person("Taylor", "Ted", "New York"));

            Assert.IsTrue(summary.IsValid);
        }
        
        private class Person
        {
            public Person(string name, string firstname, string city)
            {
                Name = name;
                Firstname = firstname;
                City = city;
            }

            public string Name { get; private set; }

            public string Firstname { get; private set; }

            public string City { get; private set; }
        }

        private class PersonValidator : Validator<Person>
        {
            protected override void Validate(IValidationSummary summary, Person item)
            {
                if (item.Name == "Taylor" && item.Firstname == "Tim")
                {
                    summary.AddResult(new ValidationResult(Severity.Error, "Tim Taylor is not alowed"));
                }

                base.Validate(summary, item);
            }
        }
    }
}
