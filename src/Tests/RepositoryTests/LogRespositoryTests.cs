using System;
using NUnit.Framework;
using SqlRepository;

namespace Tests.RepositoryTests
{
  [TestFixture]
  public class LogRespositoryTests
  {
    [TestCase("", "app", 2, "info", "message from unit tests")]
    [TestCase("Server=tcp:127.0.0.1,1433;User ID=sa;pwd=password1234$;database=minutz;", "", 2, "info", "message from unit tests")]
    [TestCase("Server=tcp:127.0.0.1,1433;User ID=sa;pwd=password1234$;database=minutz;", "app", 2, "info", "")]
    [TestCase("Server=tcp:127.0.0.1,1433;User ID=sa;pwd=password1234$;database=minutz;", "app", 2, "", "message from unit tests")]
    public void Log_GivenInvalidInputs_ShouldThrowException(string connectionString,
                                                                      string schema,
                                                                      int eventId,
                                                                      string logLevel,
                                                                      string logMessage)
    {
      //arrange
      //var schema = "app";
      //var connectionString = string.Empty;//"Server=tcp:127.0.0.1,1433;User ID=sa;pwd=password1234$;database=minutz;";
      //var eventId = 2;
      //var loglevel = "info";
      //var logMessage = "message from unit tests";
      var logger = new LogRepository();

      //assert
      Assert.Throws<ArgumentException>(() => logger.Log(schema, connectionString, eventId, logLevel, logMessage));
    }

    [Test]
    public void Log_GivenValidConnectionString_ShouldInsertIntoDB()
    {
      //arrange
      var schema = "app";
      var connectionString = "Server=tcp:127.0.0.1,1433;User ID=sa;pwd=password1234$;database=minutz;";
      var eventId = 2;
      var loglevel = "info";
      var logMessage = "message from unit tests";
      var logger = new LogRepository();

      //act
      var result = logger.Log(schema, connectionString, eventId, loglevel, logMessage);

      //assert
      Assert.IsTrue(result);

    }
  }
}
