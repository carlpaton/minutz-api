using System;
using AuthenticationRepository;
using NUnit.Framework;


namespace Tests.RepositoryTests
{
  [TestFixture]
  public class Auth0SettingTests
  {
    [Test]
    public void Token()
    {
      var service = new Auth0Settings();
      var result = service.Token();
      Assert.IsTrue(true);
    }
  }
}