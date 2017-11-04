using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Integration
{
    [TestFixture]
    public class ValidatorWithListTests
    {
        [Test]
        public void RubberStamp_Validator_ValidatorWithList_Valid()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Addresses, con => con.Condition((p, a) => a == null || a.All(ad => ad.Street != null)));

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

            Assert.That(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_ValidatorWithList_ListIsNull()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Addresses, con => con.Condition((p, a) => a == null || a.All(ad => ad.Street != null)));

            var person = new Person();
            var summary = validator.Validate(person);

            Assert.That(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Validator_ValidatorWithList_Invalid()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Addresses, con => con.Condition((p, a) => a == null || a.All(ad => ad.Street != null)));

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
            var summary = validator.Validate(person);

            Assert.That(summary.IsValid, Is.False);
        }

        private class Person
        {
            public IEnumerable<Address> Addresses { get; set; }
        }

        private class Address
        {
            // ReSharper disable once UnusedMember.Local
            public string Street { get; set; }

            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int Zip { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string City { get; set; }
        }
    }
}
