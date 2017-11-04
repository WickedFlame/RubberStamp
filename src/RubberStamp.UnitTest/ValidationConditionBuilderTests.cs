using System.Linq;
using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest
{
    [TestFixture]
    public class ValidationConditionBuilderTests
    {
        [Test]
        public void RubberStamp_ValidationConditionBuilder_NotNull_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Name, con => con.IsNotNull());

            Assert.IsInstanceOf<IsNotNullCondition<TestClass, string>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_Null_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Name, con => con.IsNull());

            Assert.IsInstanceOf<IsNullCondition<TestClass, string>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_GreaterThan_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Index, con => con.GreaterThan(1));

            Assert.IsInstanceOf<GreaterThanCondition<TestClass, int>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_GreaterOrEqualThan_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Index, con => con.GreaterOrEqualThan(1));

            Assert.IsInstanceOf<GreaterOrEqualThanCondition<TestClass, int>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_LessThan_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Index, con => con.LessThan(1));

            Assert.IsInstanceOf<LessThanCondition<TestClass, int>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_LessOrEqualThan_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Index, con => con.LessOrEqualThan(1));

            Assert.IsInstanceOf<LessOrEqualThanCondition<TestClass, int>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_IsEqual_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Index, con => con.IsEqual(1));

            Assert.IsInstanceOf<IsEqualCondition<TestClass, int>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_IsNotEqual_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Index, con => con.IsNotEqual(1));

            Assert.IsInstanceOf<IsNotEqualCondition<TestClass, int>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_LengthLessOrEqualThan_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Index, con => con.LengthLessOrEqualThan(1));

            Assert.IsInstanceOf<LengthLessOrEqualThanCondition<TestClass, int>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_IsValidEmailFormat_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Email, con => con.IsValidEmailFormat());

            Assert.IsInstanceOf<IsValidEmailFormatCondition<TestClass, string>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilder_RegexCondition_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Email, con => con.Regex(@"^[a-zA-Z]"));

            Assert.IsInstanceOf<RegexCondition<TestClass, string>>(rule.Conditions.First());
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int Index { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Email { get; set; }
        }
    }
}
