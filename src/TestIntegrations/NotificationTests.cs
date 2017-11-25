using System;
using Notifications;
using NUnit.Framework;

namespace TestIntegrations
{
  [TestFixture]
  public class NotificationTests
  {
    [Test]
    public void TestSendNotification()
    {
      var not = new StartupService();
      var q = not.SendSimpleMessage().IsSuccessStatusCode;
      Assert.True(true);
    }
  }
}
