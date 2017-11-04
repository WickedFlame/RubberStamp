using System.Linq;
using RubberStamp.Conditions;
using NUnit.Framework;

namespace RubberStamp.UnitTest
{
    [TestFixture]
    public class ValidationConditionBuilderOfTTests
    {
        [Test]
        public void RubberStamp_ValidationConditionBuilderOfT_Condition_Test()
        {
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Index, con => con.Condition((t, p) => p > 10));

            Assert.IsInstanceOf<CustomPropertyCondition<TestClass, int>>(rule.Conditions.First());
        }

        [Test]
        public void RubberStamp_ValidationConditionBuilderOfT_ConditionWithExternalValue_Test()
        {
            var name = "test";
            var rule = new Validator<TestClass>()
                .AddRule(p => p.Name, con => con.Condition((t, p) => p == name));

            Assert.IsInstanceOf<CustomPropertyCondition<TestClass, string>>(rule.Conditions.First());
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Name { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public int Index { get; set; }
        }
    }
}
