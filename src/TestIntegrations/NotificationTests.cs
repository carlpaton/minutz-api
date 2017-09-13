using Notifications;
using Xunit;

namespace TestIntegrations
{
  public class NotificationTests
  {
    [Fact]
    public void TestSendNotification()
    {
      var not = new StartupService();
      var q = not.SendAsync().Result;
      Assert.True(true);
    }
  }
}
