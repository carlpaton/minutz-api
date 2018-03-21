using System;

namespace Tests
{
    public class TestBase
    {
        public TestBase ()
        {
            Environment.SetEnvironmentVariable ("CLIENTID", "WDzuh9escySpPeAF5V0t2HdC3Lmo68a-");
            Environment.SetEnvironmentVariable ("DOMAIN", "minutz.eu.auth0.com");
            Environment.SetEnvironmentVariable ("CLIENTSECRET", "_kVUASQWVawA2pwYry-xP53kQpOALkEj_IGLWCSspXkpUFRtE_W-Gg74phrxZkz8");
            Environment.SetEnvironmentVariable ("CONNECTION", "Username-Password-Authentication");
            Environment.SetEnvironmentVariable ("ReportUrl", "https://minutz.jsreportonline.net/api/report");
            Environment.SetEnvironmentVariable ("ReportMinutesKey", "SJKYFyoYM");
        }
    }
}