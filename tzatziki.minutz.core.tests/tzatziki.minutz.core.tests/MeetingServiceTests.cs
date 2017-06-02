using Microsoft.VisualStudio.TestTools.UnitTesting;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.sqlrepository;

namespace tzatziki.minutz.core.tests
{
  [TestClass]
  public class MeetingServiceTests
  {
    private readonly ITableService _tableService;
    private readonly IMeetingRepository _meetingRepository;
    private readonly string AzureConnection = @"Server=tcp:minutz.database.windows.net;Database=minutz;User ID=minutz@minutz.database.windows.net;pwd=dH*(LtVx`fv::9E!;Trusted_Connection=False;Encrypt=True;";
    private readonly string Schema = "account_235";
    private readonly string MeetingTable = "Meeting";
    private readonly string MeetingSql = "createMeetingSchema";

    public MeetingServiceTests()
    {
      _tableService = new TableService();
      _meetingRepository = new MeetingRepository(_tableService);
    }

    [TestMethod]
    public void TableService_Initiate_ShourlReturnTrue()
    {
      var tableExists = _tableService.Initiate(AzureConnection, Schema, MeetingTable, MeetingSql);
      Assert.IsTrue(tableExists);
    }

    [TestMethod]
    public void TableService_Initiate_ShourlReturnFalse()
    {
      var tableExists = _tableService.Initiate(AzureConnection, Schema, $"{MeetingTable}_1", MeetingSql);
      Assert.IsFalse(tableExists);
    }



  }
}