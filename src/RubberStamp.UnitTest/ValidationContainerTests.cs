using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RubberStamp.UnitTest
{
    [TestFixture]
    public class ValidationContainerTests
    {
        [Test]
        public void RubberStamp_ValidationContainer()
        {
            var validator1 = new Validator<Person>();
            validator1.AddRule(p => p.Name, con => con.IsNotNull());
            var validator2 = new Validator<Person>();
            validator2.AddRule(p => p.Lastname, con => con.IsNotNull());

            var container = new ValidationContainer<Person>();
            container.Add(validator1);
            container.Add(validator2);

            var result = container.Validate(new Person { Name = "Name", Lastname = "Lastname" });

            Assert.That(result.IsValid);
        }

        [Test]
        public void RubberStamp_ValidationContainer_FailCondition1()
        {
            var validator1 = new Validator<Person>();
            validator1.AddRule(p => p.Name, con => con.IsNotNull());
            var validator2 = new Validator<Person>();
            validator2.AddRule(p => p.Lastname, con => con.IsNotNull());

            var container = new ValidationContainer<Person>();
            container.Add(validator1);
            container.Add(validator2);

            var result = container.Validate(new Person { Lastname = "Lastname" });

            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void RubberStamp_ValidationContainer_FailCondition2()
        {
            var validator1 = new Validator<Person>();
            validator1.AddRule(p => p.Name, con => con.IsNotNull());
            var validator2 = new Validator<Person>();
            validator2.AddRule(p => p.Lastname, con => con.IsNotNull());

            var container = new ValidationContainer<Person>();
            container.Add(validator1);
            container.Add(validator2);

            var result = container.Validate(new Person { Name = "Name" });

            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void RubberStamp_ValidationContainer_FailMultipleConditions()
        {
            var validator1 = new Validator<Person>();
            validator1.AddRule(p => p.Name, con => con.IsNotNull());
            var validator2 = new Validator<Person>();
            validator2.AddRule(p => p.Lastname, con => con.IsNotNull());

            var container = new ValidationContainer<Person>();
            container.Add(validator1);
            container.Add(validator2);

            var result = container.Validate(new Person());

            Assert.That(result.IsValid, Is.False);
        }

        private class Person
        {
            public string Name { get; set; }

            public string Lastname { get; set; }
        }

    }
}
