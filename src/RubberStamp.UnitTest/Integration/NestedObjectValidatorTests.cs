using System;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Integration
{
    [TestFixture]
    public class NestedObjectValidatorTests
    {
        [Test]
        public void RubberStamp_Validator_NestedValidation_AddValidatorForSubType()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.GreaterThan(0));
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.For(p => p.Address, () => adrValidator);

            var person = new Person
            {
                Address = new Address
                {
                    Street = "test",
                    Zip = 1,
                    City = "test"
                }
            };
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_NestedValidation_AddValidatorForSubType_Invalid()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.GreaterThan(0));
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.For(p => p.Address, () => adrValidator);

            var person = new Person
            {
                Address = new Address
                {
                    Zip = 1,
                    City = "test"
                }
            };
            var summary = validator.Validate(person);

            Assert.That(summary.IsValid, Is.False);
        }

        [Test]
        public void RubberStamp_Validator_NestedValidation_AddValidatorForSubType_ThatIsNull()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.IsNotNull());
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.For(p => p.Address, () => adrValidator);

            var person = new Person();
            Assert.Throws<ArgumentNullException>(() => validator.Validate(person));
        }

        [Test]
        public void RubberStamp_Validator_NestedValidation_AddValidatorForSubType_WithWhenCondition()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.IsNotNull());
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.For(p => p.Address, () => adrValidator).When(p => p.Address != null);

            var person = new Person();
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_NestedValidation_AddValidatorForSubType_WithWhenCondition_InValid()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.IsNotNull());
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.For(p => p.Address, () => adrValidator).When(p => p.Address != null);

            var person = new Person
            {
                Address = new Address()
            };
            var summary = validator.Validate(person);

            Assert.That(summary.IsValid, Is.False);
        }

        [Test]
        public void RubberStamp_Validator_NestedGenericValidation_AddValidatorForSubType()
        {
            var validator = new Validator<Person>();
            validator.For<Address, AddressValidator>(p => p.Address);

            var person = new Person
            {
                Address = new Address
                {
                    Street = "test",
                    Zip = 1,
                    City = "test"
                }
            };
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_NestedGenericValidation_AddValidatorForSubType_Invalid()
        {
            var validator = new Validator<Person>();
            validator.For<Address, AddressValidator>(p => p.Address);

            var person = new Person
            {
                Address = new Address
                {
                    Zip = 1,
                    City = "test"
                }
            };
            var summary = validator.Validate(person);

            Assert.That(summary.IsValid, Is.False);
        }

        [Test]
        public void RubberStamp_Validator_NestedGenericValidation_AddValidatorForSubType_ThatIsNull()
        {
            var validator = new Validator<Person>();
            validator.For<Address, AddressValidator>(p => p.Address);

            var person = new Person();
            Assert.Throws<ArgumentNullException>(() => validator.Validate(person));
        }

        [Test]
        public void RubberStamp_Validator_NestedGenericValidation_AddValidatorForSubType_WithWhenCondition()
        {
            var validator = new Validator<Person>();
            validator.For<Address, AddressValidator>(p => p.Address).When(p => p.Address != null);

            var person = new Person();
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_NestedGenericValidation_AddValidatorForSubType_WithWhenCondition_InValid()
        {
            var validator = new Validator<Person>();
            validator.For<Address, AddressValidator>(p => p.Address).When(p => p.Address != null);

            var person = new Person
            {
                Address = new Address()
            };
            var summary = validator.Validate(person);

            Assert.That(summary.IsValid, Is.False);
        }

        private class AddressValidator : Validator<Address>
        {
            public AddressValidator()
            {
                AddRule(a => a.Street, con => con.IsNotNull());
                AddRule(a => a.Zip, con => con.IsNotNull());
                AddRule(a => a.City, con => con.IsNotNull());
            }
        }

        private class Person
        {
            // ReSharper disable once UnusedMember.Local
            public string Name { get; set; }

            // ReSharper disable once UnusedMember.Local
            public string Lastname { get; set; }

            public Address Address { get; set; }
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
