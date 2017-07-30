using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;
using tzatziki.minutz.sqlrepository;

namespace tzatziki.minutz.core.tests
{
  [TestClass]
  public class MeetingServiceTests
  {
    private readonly ITableService _tableService;
    private readonly IMeetingRepository _meetingRepository;
    
    //private readonly string AzureConnection = @"Server=tcp:minutz.database.windows.net;Database=minutz;User ID=minutz@minutz.database.windows.net;pwd=dH*(LtVx`fv::9E!;Trusted_Connection=False;Encrypt=True;";
    //private readonly string Schema = "account_235";
    //private readonly string MeetingTable = "Meeting";
    //private readonly string MeetingSql = "createMeetingSchema";

    public MeetingServiceTests()
    {
      _tableService = new TableService();
      _meetingRepository = new MeetingRepository(_tableService);
    }

    //[TestMethod]
    //public void TableService_Initiate_ShouldReturnTrue()
    //{
    //  var tableExists = _tableService.Initiate(AzureConnection, Schema, MeetingTable, MeetingSql);
    //  Assert.IsTrue(tableExists);
    //}

    //[TestMethod]
    //public void TableService_Initiate_ShouldReturnFalse()
    //{
    //  var tableExists = _tableService.Initiate(AzureConnection, Schema, $"{MeetingTable}_1", MeetingSql);
    //  Assert.IsTrue(tableExists);
    //}

    //[TestMethod]
    //public void MeetingRepository_Initiate_ShouldReturnObjectInstance()
    //{
    //  var model = new Meeting { Id = Guid.NewGuid(), MeetingOwnerId = "auth0|58f14a8bb21f0766553879ec", Name = "MeetingRepository_Initiate_ShouldReturnObjectInstance" };
    //  var instance = _meetingRepository.Get(AzureConnection, Schema, model);
    //  Assert.IsNotNull(instance);
    //}

    //[TestMethod]
    //public void MeetingRepository_GetInstance_ShouldReturnObjectInstance()
    //{
    //  var model = new Meeting { Id = Guid.Parse("EE009E00-2FB1-40C8-93C2-EBFAC93089E8"), MeetingOwnerId = "auth0|58f14a8bb21f0766553879ec" ,Name = "MeetingRepository_GetInstance_ShouldReturnObjectInstance" };
    //  var instance = _meetingRepository.Get(AzureConnection, Schema, model);
    //  Assert.IsNotNull(instance);
    //}

    //[TestMethod]
    //public void MeetingRepository_GetUserMeetings_ShouldReturnObjectCollection()
    //{
    //  //var model = new Meeting { Id = Guid.Parse("EE009E00-2FB1-40C8-93C2-EBFAC93089E9"), MeetingOwnerId = "auth0|58f14a8bb21f0766553879ec", Name = "MeetingRepository_GetUserMeetings_ShouldReturnObjectCollection"};
    //  var instance = _meetingRepository.Get(AzureConnection, Schema, new UserProfile { ClientID = "auth0|58f14a8bb21f0766553879ec" });
    //  Assert.IsNotNull(instance);
    //}
  }
}