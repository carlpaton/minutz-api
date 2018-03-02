using Core.Validation;
using NUnit.Framework;

namespace Tests.Validation
{
    [TestFixture]
    public class PasswordValidationTests
    {
        [Test]
        public void ValidPassword_GivenEmptyString_ShouldReturnFalseAndErrorMessage()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidPassword(string.Empty);
            Assert.IsFalse(result.condition);
            Assert.AreSame(result.message, validationService.InvalidPasswordMessage);
        }

        [Test]
        public void ValidPassword_GivenSimpleString_ShouldReturnFalseAndInvalidMessage()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidPassword("paord");
            Assert.IsFalse(result.condition);
            Assert.AreSame(result.message, validationService.InvalidPasswordStrengthMessage);
        }

        [Test]
        public void ValidPassword_GivenSimpleStringWithOneCapitalLetter_ShouldReturnFalseAndInvalidMessage()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidPassword("Paord");
            Assert.IsFalse(result.condition);
            Assert.AreSame(result.message, validationService.InvalidPasswordStrengthMessage);
        }

        [Test]
        public void ValidPassword_GivenSimpleStringWithOneCapitalLetterAndOneNumber_ShouldReturnFalseAndInvalidMessage()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidPassword("Paord1");
            Assert.IsFalse(result.condition);
            Assert.AreSame(result.message, validationService.InvalidPasswordStrengthMessage);
        }

        [Test]
        public void ValidPassword_GivenSimpleStringWithOneCapitalLetterAndOneNumberAndOneSpecialCharacter_ShouldReturnFalseAndInvalidMessage()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidPassword("Paord1@");
            Assert.IsFalse(result.condition);
            Assert.AreSame(result.message, validationService.InvalidPasswordStrengthMessage);
        }

        [Test]
        public void ValidPassword_GivenSimpleStringWithOneCapitalLetterAndOneNumberAndOneSpecialCharacterAndEightCharactersLong_ShouldReturnTrueAndErrorMessage()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidPassword("Password1@");
            Assert.IsTrue(result.condition);
            Assert.AreSame(result.message, validationService.ValidPasswordMessage);
        }
    }
}
