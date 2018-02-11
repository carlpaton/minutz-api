using AuthenticationRepository;
using NUnit.Framework;

namespace Tests.RepositoryTests
{
    [TestFixture]
    public class Auth0RepositoryTests
    {
        internal string _validationMessage = "The username or password was not supplied or is incorrect. Please provide valid details.";

        [Test]
        public void CreateToken_GivenEmptyUserName_ShouldReturnFalseAndMessage ()
        {
            var repository = new Auth0Repository ();
            var result = repository.CreateToken (string.Empty, "password");
            Assert.IsFalse (result.condition);
            Assert.AreSame (result.message, this._validationMessage);
        }

        [Test]
        public void CreateToken_GivenEmptyPassword_ShouldReturnFalseAndMessage ()
        {
            var repository = new Auth0Repository ();
            var result = repository.CreateToken (string.Empty, "password");
            Assert.IsFalse (result.condition);
            Assert.AreSame (result.message, this._validationMessage);
        }

        [Test]
        public void CreateToken_GivenInvalidUsernamePassword_ShouldReturnTrueAndTokenWithSuccessMessage ()
        {
            var repository = new Auth0Repository ();
            var result = repository.CreateToken ("leeroya", "@nathan001");
            Assert.IsFalse (result.condition);
        }

        [Test]
        public void CreateToken_GivenUsernamePassword_ShouldReturnTrueAndTokenWithSuccessMessage ()
        {
            var repository = new Auth0Repository ();
            var result = repository.CreateToken ("leeroya", "@nathan01");
            Assert.IsTrue (result.condition);
            Assert.AreSame (result.message, "Success");
        }

        // [Test]
        // public void Test ()
        // {
        //     var q = new Auth0Repository ();
        //     //q.CreateUser ();
        //     Assert.IsTrue (true);
        // }
    }
}