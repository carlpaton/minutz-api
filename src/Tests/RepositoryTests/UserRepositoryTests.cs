using NUnit.Framework;
using Interface.Services;
using SqlRepository.User;

namespace Tests.RepositoryTests
{
    [TestFixture]
    public class UserRepositoryTests : TestBase
    {
        //[Test]
        public void CreateToken_GivenUsernamePassword_ShouldReturnTrueAndTokenWithSuccessMessage ()
        {
            var logService = NSubstitute.Substitute.For<ILogService>();
            
            var repository = new UserRepository (logService , new MinutzEncryption.Encryptor());
            var connectionString = $"Server=127.0.0.1,1401;User ID=sa;pwd=password1234$;database=minutz-test;";

            var result = repository.MinutzPersonCheckIfUserExistsByEmail ("info1@docker.durban", connectionString);
            Assert.IsTrue (result.Condition);
            Assert.AreSame (result.Message, "Success");
        }
    }
}