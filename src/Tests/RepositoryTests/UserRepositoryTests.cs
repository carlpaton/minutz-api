using System;
using AuthenticationRepository;
using Models.Auth0Models;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Interface.Services;
using SqlRepository;

namespace Tests.RepositoryTests
{
    [TestFixture]
    public class UserRepositoryTests : TestBase
    {
        //[Test]
        public void CreateToken_GivenUsernamePassword_ShouldReturnTrueAndTokenWithSuccessMessage ()
        {
            var logService = NSubstitute.Substitute.For<ILogService>();
            
            var repository = new UserRepository (logService);
            var connectionString = $"Server=127.0.0.1,1401;User ID=sa;pwd=password1234$;database=minutz-test;";

            var result = repository.GetUserByEmail ("info1@docker.durban", "app", connectionString);
            Assert.IsTrue (result.condition);
            Assert.AreSame (result.message, "Success");
        }
    }
}