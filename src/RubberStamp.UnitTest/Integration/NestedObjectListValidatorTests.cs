using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Integration
{
    [TestFixture]
    public class NestedObjectListValidatorTests
    {
        [Test]
        public void RubberStamp_Validator_ChainedListValidation_AddValidatorForList()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.GreaterThan(0));
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.ForEach(p => p.Addresses, () => adrValidator);

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

            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_ChainedListValidation_AddValidatorForSubType_Invalid()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.GreaterThan(0));
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.ForEach(p => p.Addresses, () => adrValidator);

            var person = new Person
            {
                Addresses = new List<Address>
                {
                    new Address
                    {
                        Street = "test",
                        Zip = 1,
                        City = "test"
                    },
                    new Address
                    {
                        Zip = 1,
                        City = "test"
                    }
                }
            };
            var summary = validator.Validate(person);

            Assert.That(summary.IsValid, Is.False);
        }

        [Test]
        public void RubberStamp_Validator_ChainedListValidation_AddValidatorForSubType_ThatIsNull()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.IsNotNull());
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.ForEach(p => p.Addresses, () => adrValidator);

            var person = new Person();
            Assert.Throws<ArgumentNullException>(() => validator.Validate(person));
        }

        [Test]
        public void RubberStamp_Validator_ChainedListValidation_AddValidatorForSubType_WithWhenCondition()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.IsNotNull());
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.ForEach(p => p.Addresses, () => adrValidator).When(p => p.Addresses != null);

            var person = new Person();
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_ChainedListValidation_AddValidatorForSubType_WithWhenCondition_InValid()
        {
            var adrValidator = new Validator<Address>();
            adrValidator.AddRule(a => a.Street, con => con.IsNotNull());
            adrValidator.AddRule(a => a.Zip, con => con.IsNotNull());
            adrValidator.AddRule(a => a.City, con => con.IsNotNull());

            var validator = new Validator<Person>();
            validator.ForEach(p => p.Addresses, () => adrValidator).When(p => p.Addresses != null);

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
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_ChainedListGenericValidation_AddValidatorForList()
        {
            var validator = new Validator<Person>();
            validator.ForEach<Address, AddressValidator>(p => p.Addresses);

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

            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_ChainedListGenericValidation_AddValidatorForSubType_Invalid()
        {
            var validator = new Validator<Person>();
            validator.ForEach<Address, AddressValidator>(p => p.Addresses);

            var person = new Person
            {
                Addresses = new List<Address>
                {
                    new Address
                    {
                        Street = "test",
                        Zip = 1,
                        City = "test"
                    },
                    new Address
                    {
                        Zip = 1,
                        City = "test"
                    }
                }
            };
            var summary = validator.Validate(person);

            Assert.That(summary.IsValid, Is.False);
        }

        [Test]
        public void RubberStamp_Validator_ChainedListGenericValidation_AddValidatorForSubType_ThatIsNull()
        {
            var validator = new Validator<Person>();
            validator.ForEach<Address, AddressValidator>(p => p.Addresses);

            var person = new Person();
            Assert.Throws<ArgumentNullException>(() => validator.Validate(person));
        }

        [Test]
        public void RubberStamp_Validator_ChainedListGenericValidation_AddValidatorForSubType_WithWhenCondition()
        {
            var validator = new Validator<Person>();
            validator.ForEach<Address, AddressValidator>(p => p.Addresses).When(p => p.Addresses != null);

            var person = new Person();
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_ChainedListGenericValidation_AddValidatorForSubType_WithWhenCondition_InValid()
        {
            var validator = new Validator<Person>();
            validator.ForEach<Address, AddressValidator>(p => p.Addresses).When(p => p.Addresses != null);

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
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
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
