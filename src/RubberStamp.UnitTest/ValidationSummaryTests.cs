using System.Linq;
using System.Text;
using NUnit.Framework;

namespace RubberStamp.UnitTest
{
    [TestFixture]
    public class ValidationSummaryTests
    {
        [Test]
        public void RubberStamp_ValidationSummary_MergeBeforeSetError()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Error, "Some Error"));
            summary.AddResult(new ValidationResult(Severity.Warning, "Some Warning"));
            summary.AddResult(new ValidationResult(Severity.Info, "Some information"));

            var second = new ValidationSummary<Two>();
            second.Merge(summary);
            second.AddResult(new ValidationResult(Severity.Error, "Second Error"));

            Assert.IsTrue(second.ValidationResults.Count() == 4);
        }

        [Test]
        public void RubberStamp_ValidationSummary_MergeAfterSetError()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Error, "Some Error"));
            summary.AddResult(new ValidationResult(Severity.Warning, "Some Warning"));
            summary.AddResult(new ValidationResult(Severity.Info, "Some information"));

            var second = new ValidationSummary<Two>();
            second.AddResult(new ValidationResult(Severity.Error, "Second Error"));

            second.Merge(summary);

            Assert.IsTrue(second.ValidationResults.Count() == 4);
        }

        [Test]
        public void RubberStamp_ValidationSummary_ErrorMessages()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Error, "Some Error"));
            summary.AddResult(new ValidationResult(Severity.Warning, "Some Warning"));
            summary.AddResult(new ValidationResult(Severity.Info, "Some information"));
            summary.AddResult(new ValidationResult(Severity.Error, "Some more Error"));

            var message = summary.GetErrorMessage();
            var expected = new StringBuilder()
                .AppendLine("Some Error")
                .AppendLine("Some more Error")
                .ToString()
                .Trim();

            Assert.AreEqual(expected, message);
        }

        [Test]
        public void RubberStamp_ValidationSummary_WarningMessages()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Error, "Some Error"));
            summary.AddResult(new ValidationResult(Severity.Warning, "Some Warning"));
            summary.AddResult(new ValidationResult(Severity.Warning, "Warning 2"));
            summary.AddResult(new ValidationResult(Severity.Info, "Some information"));

            var message = summary.GetWarningMessage();
            var expected = new StringBuilder()
                .AppendLine("Some Warning")
                .AppendLine("Warning 2")
                .ToString()
                .Trim();

            Assert.AreEqual(expected, message);
        }

        [Test]
        public void RubberStamp_ValidationSummary_InfoMessages()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Error, "Some Error"));
            summary.AddResult(new ValidationResult(Severity.Warning, "Some Warning"));
            summary.AddResult(new ValidationResult(Severity.Info, "Some Information"));
            summary.AddResult(new ValidationResult(Severity.Info, "Another Information"));

            var message = summary.GetInfoMessage();
            var expected = new StringBuilder()
                .AppendLine("Some Information")
                .AppendLine("Another Information")
                .ToString()
                .Trim();

            Assert.AreEqual(expected, message);
        }

        [Test]
        public void RubberStamp_ValidationSummary_MessageSummary()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Info, "Some information"));
            summary.AddResult(new ValidationResult(Severity.Error, "Some Error"));
            summary.AddResult(new ValidationResult(Severity.Warning, "Some Warning"));

            var message = summary.ValidationMessage;
            var expected = new StringBuilder()
                .AppendLine("Some Error")
                .AppendLine("Some Warning")
                .Append("Some information")
                .ToString();

            Assert.AreEqual(expected, message);
        }

        [Test]
        public void RubberStamp_ValidationSummary_MessageSummary_OnlyError()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Error, "Some Error"));

            var message = summary.ValidationMessage;
            var expected = new StringBuilder()
                .Append("Some Error")
                .ToString();

            Assert.AreEqual(expected, message);
        }

        [Test]
        public void RubberStamp_ValidationSummary_MessageSummary_OnlyWarning()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Warning, "Some Warning"));

            var message = summary.ValidationMessage;
            var expected = new StringBuilder()
                .AppendLine("Some Warning")
                .ToString()
                .Trim();

            Assert.AreEqual(expected, message);
        }

        [Test]
        public void RubberStamp_ValidationSummary_MessageSummary_OnlyInfo()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Info, "Some information"));

            var message = summary.ValidationMessage;
            var expected = new StringBuilder()
                .Append("Some information")
                .ToString();

            Assert.AreEqual(expected, message);
        }

        [Test]
        public void RubberStamp_ValidationSummary_MessageSummary_OnlyErrorAndInfo()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Info, "Some information"));
            summary.AddResult(new ValidationResult(Severity.Error, "Some Error"));

            var message = summary.ValidationMessage;
            var expected = new StringBuilder()
                .AppendLine("Some Error")
                .Append("Some information")
                .ToString();

            Assert.AreEqual(expected, message);
        }

        [Test]
        public void RubberStamp_ValidationSummary_MessageSummary_OnlyErrorAndWarning()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Error, "Some Error"));
            summary.AddResult(new ValidationResult(Severity.Warning, "Some Warning"));

            var message = summary.ValidationMessage;
            var expected = new StringBuilder()
                .AppendLine("Some Error")
                .Append("Some Warning")
                .ToString();

            Assert.AreEqual(expected, message);
        }

        [Test]
        public void RubberStamp_ValidationSummary_MessageSummary_OnlyWarningAndInfo()
        {
            var summary = new ValidationSummary<One>();
            summary.AddResult(new ValidationResult(Severity.Info, "Some information"));
            summary.AddResult(new ValidationResult(Severity.Warning, "Some Warning"));

            var message = summary.ValidationMessage;
            var expected = new StringBuilder()
                .AppendLine("Some Warning")
                .AppendLine("Some information")
                .ToString()
                .Trim();

            Assert.AreEqual(expected, message);
        }

        private class One
        {
        }

        private class Two
        {
        }
    }
}
