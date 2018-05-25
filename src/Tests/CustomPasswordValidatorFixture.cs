using System;
using AspnetAuthenticationRespository;
using Minutz.Models;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CustomPasswordValidatorFixture
    {
        private readonly string _invalidPassword;
        private readonly string _validPassword;

        public CustomPasswordValidatorFixture()
        {
            _invalidPassword = "nathan";
            _validPassword = "@Nathan001";
        }

        [Test]
        public void CheckStrength_Given_EmptyString_ShouldThrowArgumentNullException()
        {
            var validator = new CustomPasswordValidator();
            Assert.Throws<ArgumentNullException>(() => validator.CheckStrength(string.Empty));
        }
        
        [Test]
        public void CheckStrength_Given_WeakString_ShouldReturn_VeryWeak()
        {
            var validator = new CustomPasswordValidator();
            var result = validator.CheckStrength(_invalidPassword);
            Assert.AreEqual(result, PasswordScore.VeryWeak);
        }
        
        [Test]
        public void CheckStrength_Given_WeakString_ShouldReturn_Strong()
        {
            var validator = new CustomPasswordValidator();
            var result = validator.CheckStrength(_validPassword);
            Assert.AreEqual(result, PasswordScore.Strong);
        }
    }
}