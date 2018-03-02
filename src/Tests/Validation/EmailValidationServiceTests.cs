using Core.Validation;
using NUnit.Framework;

namespace Tests.Validation
{
    [TestFixture]
    public class EmailValidationServiceTests
    {
        [Test]
        public void ValidEmail_GivenEmptyString_ShouldReturnFalseAndMessage_GiveAValidEmail()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidEmail(string.Empty);
            Assert.IsFalse(result.condition);
            Assert.AreSame(result.message, validationService.InvalidEmailMessage);
        }

        [Test]
        public void ValidEmail_GiveninvalidString_ShouldReturnFalseAndMessage_GiveAValidEmail()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidEmail("foo");
            Assert.IsFalse(result.condition);
            Assert.AreSame(result.message, validationService.InvalidEmailMessage);
        }

        [Test]
        public void ValidEmail_GivenDoubleEmailAtSignString_ShouldReturnFalseAndMessage_GiveAValidEmail()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidEmail("foo@@.com");
            Assert.IsFalse(result.condition);
            Assert.AreSame(result.message, validationService.InvalidEmailMessage);
        }

        [Test]
        public void ValidEmail_GivenValidString_ShouldReturnTrueAndMessage_ValidEmail()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidEmail("foo@foo.com");
            Assert.IsTrue(result.condition);
            Assert.AreSame(result.message, validationService.ValidEmailMessage);
        }
    }
}