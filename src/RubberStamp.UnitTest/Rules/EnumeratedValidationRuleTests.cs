using System;
using System.Collections.Generic;
using RubberStamp.Rules;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Rules
{
    [TestFixture]
    public class EnumeratedValidationRuleTests
    {
        [Test]
        public void RubberStamp_Rules_EnumeratedValidationRule_AddValidatorForSubType()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.GreaterThan(0));
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var rule = new EnumeratedValidationRule<Person, Address>(p => p.Addresses, () => adrValidator);
            var person = new Person
            {
                Addresses = new List<Address>
                {
                    new Address
                    {
                        Street = "test",
                        Zip = 1,
                        City = "test"
                    }
                }
            };
            var result = rule.Validate(person);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void RubberStamp_Rules_EnumeratedValidationRule_AddValidatorForSubType_Invalid()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.GreaterThan(0));
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var rule = new EnumeratedValidationRule<Person, Address>(p => p.Addresses, () => adrValidator);

            var person = new Person
            {
                Addresses = new List<Address>
                {
                    new Address
                    {
                        Zip = 1,
                        City = "test"
                    }
                }
            };
            var result = rule.Validate(person);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void RubberStamp_Rules_EnumeratedValidationRule_AddValidatorForSubType_ThatIsNull()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.IsNotNull());
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var rule = new EnumeratedValidationRule<Person, Address>(p => p.Addresses, () => adrValidator);

            var person = new Person();
            Assert.Throws<ArgumentNullException>(() => rule.Validate(person));
        }

        [Test]
        public void RubberStamp_Rules_EnumeratedValidationRule_AddValidatorForSubType_WithWhenCondition()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.IsNotNull());
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var rule = new EnumeratedValidationRule<Person, Address>(p => p.Addresses, () => adrValidator);

            var person = new Person();
            Assert.Throws<ArgumentNullException>(() => rule.Validate(person));
        }

        [Test]
        public void RubberStamp_Rules_EnumeratedValidationRule_AddValidatorForSubType_WithWhenCondition_InValid()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.IsNotNull());
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var rule = new EnumeratedValidationRule<Person, Address>(p => p.Addresses, () => adrValidator);

            var person = new Person
            {
                Addresses = new List<Address> { new Address() }
            };
            var result = rule.Validate(person);

            Assert.That(result, Is.Not.Null);
        }

        private class Person
        {
            // ReSharper disable once UnusedMember.Local
            public string Name { get; set; }

            // ReSharper disable once UnusedMember.Local
            public string Lastname { get; set; }

            public IEnumerable<Address> Addresses { get; set; }
        }

        private class Address
        {
            // ReSharper disable once UnusedMember.Local
            public string Street { get; set; }

            // ReSharper disable once UnusedMember.Local
            public int Zip { get; set; }

            public string City { get; set; }
        }
    }
}
