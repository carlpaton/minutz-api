using System;
using AuthenticationRepository;
using Models.Auth0Models;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Interface.Services;

namespace Tests.RepositoryTests
{
    [TestFixture]
    public class Auth0RepositoryTests
    {
        public Auth0RepositoryTests()
        {
            Environment.SetEnvironmentVariable("CLIENTID", "WDzuh9escySpPeAF5V0t2HdC3Lmo68a-");
            Environment.SetEnvironmentVariable("DOMAIN", "minutz.eu.auth0.com");
            Environment.SetEnvironmentVariable("CLIENTSECRET", "_kVUASQWVawA2pwYry-xP53kQpOALkEj_IGLWCSspXkpUFRtE_W-Gg74phrxZkz8");
            Environment.SetEnvironmentVariable("CONNECTION", "Username-Password-Authentication");
        }
        internal string _validationMessage = "The username or password was not supplied or is incorrect. Please provide valid details.";

        [Test]
        public void CreateToken_GivenEmptyUserName_ShouldReturnFalseAndMessage ()
        {
            var logService = NSubstitute.Substitute.For<ILogService>();
            var repository = new Auth0Repository (logService);
            var result = repository.CreateToken (string.Empty, "password");
            Assert.IsFalse (result.condition);
            Assert.AreSame (result.message, this._validationMessage);
        }

        [Test]
        public void CreateToken_GivenEmptyPassword_ShouldReturnFalseAndMessage ()
        {
            var logService = NSubstitute.Substitute.For<ILogService>();
            var repository = new Auth0Repository (logService);
            var result = repository.CreateToken (string.Empty, "password");
            Assert.IsFalse (result.condition);
            Assert.AreSame (result.message, this._validationMessage);
        }

        [Test]
        public void CreateToken_GivenInvalidUsernamePassword_ShouldReturnTrueAndTokenWithSuccessMessage ()
        {
            var logService = NSubstitute.Substitute.For<ILogService>();
            var repository = new Auth0Repository (logService);
            var result = repository.CreateToken ("leeroya", "@nathan001");
            Assert.IsFalse (result.condition);
        }

        [Test]
        public void CreateToken_GivenUsernamePassword_ShouldReturnTrueAndTokenWithSuccessMessage ()
        {
            var logService = NSubstitute.Substitute.For<ILogService>();
            var repository = new Auth0Repository (logService);
            var result = repository.CreateToken ("leeroya", "@nathan01");
            Assert.IsTrue (result.condition);
            Assert.AreSame (result.message, "Success");
        }
    }
}