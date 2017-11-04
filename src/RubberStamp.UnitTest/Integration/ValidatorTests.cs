using System;
using System.Linq;
using System.Text;
using RubberStamp.Conditions;
using RubberStamp.Rules;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Integration
{
    [TestFixture]
    public class ValidatorTests
    {
        [Test]
        public void Validation_Integration_Validator_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Name, con => con.IsNotNull());
            
            var summary = validator.Validate(new TestClass());

            Assert.IsTrue(summary.ValidationResults.Any());
            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_MultiCondition_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Index, con => con.GreaterThan(1), con => con.LessThan(10));
            
            var summary = validator.Validate(new TestClass());

            Assert.IsTrue(summary.ValidationResults.Any());
            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_NotNull_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Name, con => con.IsNotNull());
            
            var summary = validator.Validate(new TestClass());

            Assert.IsTrue(summary.ValidationResults.Any());
            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_Add_ValidationRule_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new IsNotNullCondition<TestClass, string>());

            validator.AddRule(rule);
            
            var summary = validator.Validate(new TestClass());

            Assert.IsTrue(summary.ValidationResults.Any());
            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_Add_ValidationRule_WithWrongPropertyType_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, int>(p => p.Index)
                .AddCondition(new GreaterThanCondition<TestClass, string>("ABC"));
            validator.AddRule(rule);
            
            Assert.Throws<NullReferenceException>(() => validator.Validate(new TestClass()));
        }
        
        [Test]
        public void Validation_Integration_Validator_CustomValidationRule_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(tc => tc.Name != null, "The validation failed");
            
            var summary = validator.Validate(new TestClass());

            Assert.IsTrue(summary.ValidationResults.Any());
            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_CustomValidationRule_MethodExpression_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(
                tc =>
                {
                    if (tc.Name != null)
                    {
                        return true;
                    }

                    return false;
                },
                "The validation failed");

            var summary = validator.Validate(new TestClass());

            Assert.IsTrue(summary.ValidationResults.Any());
            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_Add_CustomValidationRule_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new CustomValidationRule<TestClass>("The validation failed")
                .AddCondition(new CustomEntityCondition<TestClass>(tc => tc.Name != null));

            validator.AddRule(rule);
            
            var summary = validator.Validate(new TestClass());

            Assert.IsTrue(summary.ValidationResults.Any());
            Assert.IsFalse(summary.IsValid);
        }
        
        [Test]
        public void Validation_Integration_Validator_Add_CustomValidationRule_WrongRuleType_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new CustomValidationRule<TestClass>("The validation failed")
                .AddCondition(new GreaterThanCondition<TestClass, int>(t => t.Index, 4));

            validator.AddRule(rule);
            var summary = validator.Validate(new TestClass { Index = 5 });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_CustomValidator_ValidateMethod_Test()
        {
            var validator = new TestClassValidator();
            var testCls = new TestClass
            {
                Name = "Test",
                Index = 0
            };
            var summary = validator.Validate(testCls);

            Assert.IsTrue(summary.ValidationResults.Any());
            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_CustomValidator_AddRule_Test()
        {
            var validator = new TestClassValidator();
            validator.AddRule(p => p.Name, con => con.IsEqual("Test"));

            var testCls = new TestClass
            {
                Name = "Test",
                Index = 2
            };
            var summary = validator.Validate(testCls);

            Assert.IsFalse(summary.ValidationResults.Any());
            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_CustomValidator_AddRule_Notvalid_Test()
        {
            var validator = new TestClassValidator();
            validator.AddRule(p => p.Name, con => con.IsEqual("Test"));

            var testCls = new TestClass
            {
                Name = "NotEqual",
                Index = 2
            };
            var summary = validator.Validate(testCls);

            Assert.IsTrue(summary.ValidationResults.Any());
            Assert.IsFalse(summary.IsValid);
        }
        
        [Test]
        public void Validation_Integration_Validator_Message_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Name, con => con.IsNotNull());
            
            var summary = validator.Validate(new TestClass());

            var result = summary.ValidationResults.First();
            var expected = new StringBuilder();
            expected.Append("The Property Name is not allowed to be null");
            
            Assert.AreEqual(expected.ToString(), result.Message);
        }

        [Test]
        public void Validation_Integration_Validator_CustomMessage_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Name, con => con.IsNotNull())
                .SetMessage("This is a custom message");
            
            var summary = validator.Validate(new TestClass());

            var result = summary.ValidationResults.First();
            Assert.AreEqual("This is a custom message", result.Message);
        }
        
        [Test]
        public void Validation_Integration_Validator_WithSubObject_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Address.City, con => con.IsNotNull());

            var person = new Person
            {
                Address = new Address()
            };
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.ValidationResults.Any());
        }

        [Test]
        public void Validation_Integration_Validator_WithSubObject_Valid_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Address.City, con => con.IsNotNull());

            var person = new Person
            {
                Address = new Address
                {
                    City = "New York"
                }
            };
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }
        
        [Test]
        public void Validation_Integration_Validator_WithSubObject_NotDefined_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Address.City, con => con.IsNotNull());

            Assert.Throws<NullReferenceException>(() => validator.Validate(new Person()));
        }

        [Test]
        public void Validation_Integration_Validator_WithSubObject_WithWhen_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Address.City, con => con.IsNotNull()).When(p => p.Address != null);
            
            var summary = validator.Validate(new Person());

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void Validation_Integration_Validator_WithSubObject_WithWhen_Invalid_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Address.City, con => con.IsNotNull()).When(p => p.Address != null);

            var person = new Person
            {
                Address = new Address()
            };
            var summary = validator.Validate(person);

            Assert.IsTrue(summary.ValidationResults.Any());
        }

        [Test]
        public void Validation_Integration_Validator_WithSubObject_WithWhen_Valid_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Address.City, con => con.IsNotNull()).When(p => p.Address != null);

            var person = new Person
            {
                Address = new Address
                {
                    City = "New York"
                }
            };

            var summary = validator.Validate(person);

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_CustomConditionBuilder_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Name, con => con.Condition((bo, p) => p != null && p != bo.Lastname));
            var summary = validator.Validate(new Person
            {
                Name = "Test",
                Lastname = "Succeed"
            });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_CustomConditionBuilder_Invalid_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Name, con => con.Condition((bo, p) => p != null && p != bo.Lastname));

            var summary = validator.Validate(new Person
            {
                Name = "Test",
                Lastname = "Test"
            });

            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_CustomConditionBuilder_Message_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Name, con => con.Condition((bo, p) => p != null && bo.Lastname != null));

            var summary = validator.Validate(new Person());
            
            Assert.AreEqual(@"The Property Name caused a validation error", summary.ValidationResults.First().Message);
        }

        [Test]
        public void RubberStamp_Integration_Validator_GreaterOrEqualThanCondition_Equal_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Index, con => con.GreaterOrEqualThan(5));

            var summary = validator.Validate(new TestClass { Index = 5 });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_GreaterOrEqualThanCondition_Greater_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Index, con => con.GreaterOrEqualThan(5));

            var summary = validator.Validate(new TestClass { Index = 6 });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_GreaterOrEqualThanCondition_Less_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Index, con => con.GreaterOrEqualThan(5));

            var summary = validator.Validate(new TestClass { Index = 4 });

            Assert.IsFalse(summary.IsValid);

            var expected = new StringBuilder();
            expected.Append("The Property Index has to be greater than or equal to 5");

            Assert.AreEqual(expected.ToString(), summary.ValidationMessage);

            Assert.AreEqual("The Property Index has to be greater than or equal to 5", summary.ValidationResults.First().Message);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_IsEqualCondition_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new IsEqualCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "test" });
            
            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_IsEqualCondition_Invalid_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new IsEqualCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "not passed" });

            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_LessThanCondition_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new LessThanCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "atest" });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_LessThanCondition_Invalid_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new IsEqualCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "ztest" });

            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_GreaterThanCondition_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new GreaterThanCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "ztest" });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_GreaterThanCondition_Invalid_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new GreaterThanCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "atest" });

            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_IsNotEqualCondition_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new IsNotEqualCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "ztest" });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_IsNotEqualCondition_Invalid_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new IsNotEqualCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "test" });

            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_IsNullCondition_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new IsNullCondition<TestClass, string>());

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass());

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_IsNullCondition_Invalid_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new IsNullCondition<TestClass, string>());

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "test" });

            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_LessOrEqualThanCondition_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new LessOrEqualThanCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "test" });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_LessOrEqualThanCondition_Invalid_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new LessOrEqualThanCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "ztest" });

            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_GreaterOrEqualThanCondition_Invalid_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new GreaterOrEqualThanCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "atest" });

            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_GreaterOrEqualThanCondition_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new GreaterOrEqualThanCondition<TestClass, string>("test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "test" });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_CustomPropertyCondition_Invalid_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new CustomPropertyCondition<TestClass, string>((t, s) => t.Name == "test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "atest" });

            Assert.IsFalse(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_AddCondition_CustomPropertyCondition_Test()
        {
            var validator = new Validator<TestClass>();
            var rule = new ValidationRule<TestClass, string>(p => p.Name)
                .AddCondition(new CustomPropertyCondition<TestClass, string>((t, s) => t.Name == "test"));

            validator.AddRule(rule);

            var summary = validator.Validate(new TestClass { Name = "test" });

            Assert.IsTrue(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_SetMessageOnCondition_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Name, con => con.IsNotNull().SetMessage("Error message"));

            var summary = validator.Validate(new TestClass());

            Assert.IsTrue(summary.ValidationResults.First().Message.Contains("Error message"));
        }

        [Test]
        public void RubberStamp_Integration_Validator_SetMessageOnCondition_MultiCondition_Test()
        {
            var validator = new Validator<TestClass>();
            validator.AddRule(p => p.Index, con => con.GreaterThan(10).SetMessage("Error message 1"), con => con.LessThan(1).SetMessage("Error message 2"));

            var summary = validator.Validate(new TestClass { Index = 5 });

            Assert.IsTrue(summary.ValidationResults.First().Message.Contains("Error message 1"));
            Assert.IsTrue(summary.ValidationResults.Last().Message.Contains("Error message 2"));
        }

        [Test]
        public void RubberStamp_Integration_Validator_CustomConditionBuilder_NullableProperty_Test()
        {
            var validator = new Validator<TestClass2>();
            validator.AddRule(p => p.ValidFrom, con => con.Condition((c, validFrom) => !c.ValidTo.HasValue || validFrom < c.ValidTo));

            var summary = validator.Validate(new TestClass2());

            Assert.That(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_GreaterThan_NullableProperty_Test()
        {
            DateTime? date = DateTime.MinValue;
            var validator = new Validator<TestClass2>();
            validator.AddRule(p => p.ValidFrom, con => con.GreaterThan(date));

            var summary = validator.Validate(new TestClass2());

            Assert.That(summary.IsValid, Is.False);
        }

        [Test]
        public void RubberStamp_Integration_Validator_GreaterThan_NullableProperty_Valid_Test()
        {
            var validator = new Validator<TestClass2>();
            validator.AddRule(p => p.ValidFrom, con => con.GreaterThan(DateTime.MinValue));

            var summary = validator.Validate(new TestClass2 { ValidFrom = DateTime.MaxValue });

            Assert.That(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_GetPropertyNameInResult_Test()
        {
            var validator = new Validator<TestClass2>();
            validator.AddRule(p => p.ValidFrom, con => con.GreaterThan(DateTime.MinValue));

            var summary = validator.Validate(new TestClass2());

            Assert.That(summary.ValidationResults.First().Property, Is.EqualTo("ValidFrom"));
        }

        [Test]
        public void RubberStamp_Integration_Validator_MethodeExpression_Test()
        {
            var validator = new Validator<TestClass2>();
            validator.AddRule(p => p.Name, con => con.Condition((c, street) => MethodExpression(c)));
            
            var summary = validator.Validate(new TestClass2());

            Assert.That(summary.ValidationResults.First().Property, Is.EqualTo("Name"));
        }

        [Test]
        public void RubberStamp_Integration_Validator_IsValidEmail_Test()
        {
          var validator = new Validator<Person>();
            validator.AddRule(p => p.Email, con => con.IsValidEmailFormat());

            var summary = validator.Validate(new Person { Email = "Test@test.com" });

            Assert.That(summary.IsValid);
        }

        [Test]
        public void RubberStamp_Integration_Validator_IsValidEmail_Null_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Email, con => con.IsValidEmailFormat());

            var summary = validator.Validate(new Person());

            Assert.That(summary.IsValid, Is.False);
        }

        [Test]
        public void RubberStamp_Integration_Validator_IsValidEmail_Invalid_Test()
        {
            var validator = new Validator<Person>();
            validator.AddRule(p => p.Email, con => con.IsValidEmailFormat());

            var summary = validator.Validate(new Person { Email = "Test person@test.com" });

            Assert.That(summary.IsValid, Is.False);
        }


        private bool MethodExpression(TestClass2 cls)
        {
            return cls.ValidFrom > DateTime.MinValue && cls.ValidTo < DateTime.MaxValue && cls.Name != null;
        }

        private class TestClassValidator : Validator<TestClass>
        {
            public TestClassValidator()
            {
                AddRule(p => p.Name, con => con.IsNotNull());
            }
            
            protected override void Validate(IValidationSummary summary, TestClass item)
            {
                if (item.Index < 1)
                {
                    summary.AddResult(new ValidationResult(Severity.Error, "Index is smaller than 1"));
                }

                base.Validate(summary, item);
            }
        }

        private class TestClass
        {
            public string Name { get; set; }

            public int Index { get; set; }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass2
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }

            public DateTime? ValidFrom { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public DateTime? ValidTo { get; set; }
        }

        private class Address
        {
            // ReSharper disable once UnusedMember.Local
            public string Street { get; set; }

            // ReSharper disable once UnusedMember.Local
            public int Zip { get; set; }

            public string City { get; set; }
        }

        private class Person
        {
            public string Name { get; set; }

            public string Lastname { get; set; }

            public Address Address { get; set; }

            public string Email { get; set; }
        }
    }
}
