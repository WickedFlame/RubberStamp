using System.Text;
using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest.Conditions
{
    [TestFixture]
    public class IsValidEmailFormatConditionTests
    {
        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Valid_Email_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsTrue(condition.IsValid(new TestClass { Email = "JoeSmith@example.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Valid_Email_With_Dot_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsTrue(condition.IsValid(new TestClass { Email = "firstname.lastname@example.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Valid_Email_With_MUlti_Dot_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsTrue(condition.IsValid(new TestClass { Email = "firstname....................lastname@example.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Valid_Email_With_Special_Character_In_Name_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsTrue(condition.IsValid(new TestClass { Email = "firstname-._lastname@example.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Valid_Email_With_Number_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsTrue(condition.IsValid(new TestClass { Email = "1234567890@12345.123" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Valid_Email_With_Long_Name_Test()
        {
            var email = new StringBuilder();
            while (email.Length <= 255)
            {
                email.Append('x');
            }

            email.Append("@example.com");

            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsTrue(condition.IsValid(new TestClass { Email = email.ToString() }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Valid_Email_With_Short_Name_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsTrue(condition.IsValid(new TestClass { Email = "x@example.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Valid_Email_With_Dot_Before_Name_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsTrue(condition.IsValid(new TestClass { Email = ".email@example.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Valid_Email_With_Dot_After_Name_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsTrue(condition.IsValid(new TestClass { Email = "email.@example.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_Invalid_Email_With_SpecialCharacters_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsFalse(condition.IsValid(new TestClass { Email = "!#$%&'*+-/=?^_`{|}~\"(),:;<>@[\\]@example.org" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_InValid_Email_With_Space_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsFalse(condition.IsValid(new TestClass { Email = "Joe Smith @localhost.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_InValid_Email_With_Wrong_Domain_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsFalse(condition.IsValid(new TestClass { Email = "JoeSmith@localhost.c" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_InValid_Email_With_Domain_Without_Dot_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsFalse(condition.IsValid(new TestClass { Email = "JoeSmith@localhost" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_InValid_Email_Without_Domain_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsFalse(condition.IsValid(new TestClass { Email = "PlainAddress" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_InValid_Email_With_Special_Characters_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsFalse(condition.IsValid(new TestClass { Email = "#@%^%#$@#$@#.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_InValid_Email_Without_Name_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsFalse(condition.IsValid(new TestClass { Email = "@example.com" }));
        }

        [Test]
        public void RubberStamp_Conditions_IsValidEmailFormatCondition_InValid_Email_Without_Sign_Test()
        {
            var condition = new IsValidEmailFormatCondition<TestClass, string>(p => p.Email);
            Assert.IsFalse(condition.IsValid(new TestClass { Email = "email.example.com" }));
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Email { get; set; }
        }
    }
}
