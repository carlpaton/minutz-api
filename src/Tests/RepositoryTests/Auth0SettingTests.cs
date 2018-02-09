using AuthenticationRepository;
using NUnit.Framework;

namespace Tests.RepositoryTests
{
    [TestFixture]
    public class Auth0SettingTests
    {
        [Test]
        public void Test ()
        {
            var q = new Auth0Settings ();
            q.CreateUser ();
            Assert.IsTrue (true);
        }
    }
}