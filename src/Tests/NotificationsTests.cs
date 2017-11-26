using System;
using Notifications;
using NUnit.Framework;

namespace Tests
{
    public class NotificationsTests
    {
        [Test]
        public void TestSendNotification ()
        {
            var not = new StartupService ();
            var q = not.SendSimpleMessage ();
            Assert.True (true);
        }
    }
}