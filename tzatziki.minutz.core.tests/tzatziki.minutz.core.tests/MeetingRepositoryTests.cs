using Microsoft.VisualStudio.TestTools.UnitTesting;
using tzatziki.minutz.sqlrepository;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models.Auth;
using System;
using System.Linq;

namespace tzatziki.minutz.core.tests
{
	[TestClass]
	public class MeetingRepositoryTests
	{
		private readonly IMeetingRepository _meetingRepository;

		private readonly UserProfile _user;
		private const string _connectionSting = "Server=tcp:127.0.0.1,1433;User ID=sa;pwd=Password1234;database=minutz;";
		private const string _schema = "account_b5b0338ddda0415a9cd5364d9c55b802";
		private const string MeetingId = "CC1EED77-17A7-C093-C274-6C7BF3EA28C8";
		private const string MeetingAgendaItemId = "7E0DDFFE-8B63-14FF-D31E-AB7A30C2DE47";
		public MeetingRepositoryTests()
		{
			_meetingRepository = new MeetingRepository(new TableService());

			_user = new UserProfile
			{
				Active = true,
				FirstName = "Lee-Roy",
				LastName = "Ashworth",
				Name = "Lee-Roy Ashworth",
				ClientID = "google-oauth2|117785067536320807071",
				UserId = "google-oauth2|117785067536320807071",
				InstanceId = Guid.Parse("b5b0338d-dda0-415a-9cd5-364d9c55b802"),
				EmailAddress = "leeroya@gmail.com"
			};
		}

		[TestMethod]
		public void IntegrationTest_GetMeeting_ShouldReturnOnlyOne()
		{
			var result = _meetingRepository.Get(_connectionSting, _schema, _user);
			Assert.IsNotNull(result);
			Assert.AreEqual(result.Where(i => i.Id == Guid.Parse(MeetingId)).ToList().Count , 1);
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void IntegrationTest_GetMeeting_Update_MeetingAgendaCollectionIsNUllShouldThrowException()
		{
			var result = _meetingRepository.Get(_connectionSting, _schema, _user);
			var updateMeeting = result.FirstOrDefault(i=> i.Id == Guid.Parse(MeetingId));
			updateMeeting.MeetingAgendaCollection = null;
			updateMeeting.Name = "Unit Testing";
			var update = _meetingRepository.Get(_connectionSting, _schema, updateMeeting,string.Empty, false);
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void IntegrationTest_GetMeeting_Update_MeetingAttachmentCollectionIsNUllShouldThrowException()
		{
			var result = _meetingRepository.Get(_connectionSting, _schema, _user);
			var updateMeeting = result.FirstOrDefault(i => i.Id == Guid.Parse(MeetingId));
			updateMeeting.MeetingAgendaCollection = new System.Collections.Generic.List<models.Entities.MeetingAgendaItem>();
			updateMeeting.MeetingAttachmentCollection = null;
			updateMeeting.Name = "Unit Testing";
			var update = _meetingRepository.Get(_connectionSting, _schema, updateMeeting, string.Empty,false);
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void IntegrationTest_GetMeeting_Update_MeetingNoteCollectionIsNUllShouldThrowException()
		{
			var result = _meetingRepository.Get(_connectionSting, _schema, _user);
			var updateMeeting = result.FirstOrDefault(i => i.Id == Guid.Parse(MeetingId));
			updateMeeting.MeetingAgendaCollection = new System.Collections.Generic.List<models.Entities.MeetingAgendaItem>();
			updateMeeting.MeetingAttachmentCollection = new System.Collections.Generic.List<models.Entities.MeetingAttachmentItem>();
			updateMeeting.MeetingNoteCollection = null;

			updateMeeting.Name = "Unit Testing";
			var update = _meetingRepository.Get(_connectionSting, _schema, updateMeeting, string.Empty,false);
		}

		[TestMethod]
		public void IntegrationTest_GetMeeting_UpdateName_ShouldBeEqual()
		{
			var result = _meetingRepository.Get(_connectionSting, _schema, _user);
			var updateMeeting = result.FirstOrDefault(i => i.Id == Guid.Parse(MeetingId));
			updateMeeting.MeetingAgendaCollection = new System.Collections.Generic.List<models.Entities.MeetingAgendaItem>();
			updateMeeting.MeetingAttachmentCollection = new System.Collections.Generic.List<models.Entities.MeetingAttachmentItem>();
			updateMeeting.MeetingNoteCollection = new System.Collections.Generic.List<models.Entities.MeetingNoteItem >();
			updateMeeting.Name = "Unit Testing";
			var update = _meetingRepository.Get(_connectionSting, _schema, updateMeeting, string.Empty,false);

			Assert.IsNotNull(update);
			Assert.AreEqual(updateMeeting.Name, update.Name);
		}

		[TestMethod]
		public void IntegrationTest_GetMeeting_ShouldHaveOneTopic()
		{
			var result = _meetingRepository.Get(_connectionSting, _schema, _user);
			var updateMeeting = result.FirstOrDefault(i => i.Id == Guid.Parse(MeetingId));
			Assert.AreEqual(updateMeeting.MeetingAgendaCollection.Where(i=> i.Id == Guid.Parse(MeetingAgendaItemId)).ToList().Count, 1);
		}

		[TestMethod]
		public void IntegrationTest_GetMeeting_Update_Agenda_ShouldBeEqual()
		{
			var result = _meetingRepository.Get(_connectionSting, _schema, _user);
			var updateMeeting = result.FirstOrDefault(i => i.Id == Guid.Parse(MeetingId));
			updateMeeting.MeetingAttachmentCollection = new System.Collections.Generic.List<models.Entities.MeetingAttachmentItem>();
			updateMeeting.MeetingNoteCollection = new System.Collections.Generic.List<models.Entities.MeetingNoteItem>();
			updateMeeting.Name = "Unit Testing";

			updateMeeting.MeetingAgendaCollection.FirstOrDefault().AgendaText = "Unit Testing Agenda Item";

			var update = _meetingRepository.Get(_connectionSting, _schema, updateMeeting, string.Empty,false);

			//Assert.IsNotNull(update);
			Assert.AreEqual(updateMeeting.MeetingAgendaCollection.FirstOrDefault(i=> i.Id == Guid.Parse(MeetingAgendaItemId)).AgendaText, "Unit Testing Agenda Item");
		}

		[TestMethod]
		public void IntegrationTest_GetMeeting_Add_Agenda_ShouldBeEqual()
		{
			var result = _meetingRepository.Get(_connectionSting, _schema, _user);
			var updateMeeting = result.FirstOrDefault(i => i.Id == Guid.Parse(MeetingId));
			updateMeeting.MeetingAttachmentCollection = new System.Collections.Generic.List<models.Entities.MeetingAttachmentItem>();
			updateMeeting.MeetingNoteCollection = new System.Collections.Generic.List<models.Entities.MeetingNoteItem>();
			updateMeeting.Name = "Unit Testing";

			updateMeeting.MeetingAgendaCollection.FirstOrDefault().AgendaText = "Unit Testing Agenda Item";

			var itemId = Guid.NewGuid();
			var newItem = new models.Entities.MeetingAgendaItem
			{
				AgendaHeading = $"Unit Testing Agenda Heading - {itemId.ToString()}",
				AgendaText = $"Unit Testing Agenda Text /n {DateTime.UtcNow.ToUniversalTime().ToString()}",
				CreatedDate = DateTime.UtcNow,
				Duration = "5",
				Id = itemId,
				IsComplete = false,
				MeetingAttendeeId = _user.UserId,
				ReferanceId = Guid.Parse(MeetingId)
			};
			updateMeeting.MeetingAgendaCollection.Add(newItem);

			var update = _meetingRepository.Get(_connectionSting, _schema, updateMeeting, string.Empty,false);

			Assert.IsTrue(update.MeetingAgendaCollection.Count > 1);
		}

		[TestMethod]
		public void IntegrationTest_GetMeeting_Add_Attendee_ShouldBeEqual()
		{
			var result = _meetingRepository.Get(_connectionSting, _schema, _user);
			var updateMeeting = result.FirstOrDefault(i => i.Id == Guid.Parse(MeetingId));
			
			var itemId = Guid.NewGuid();
			var newItem = new models.Entities.MeetingAttendee
			{
				Role = "Attendee",
				Id = itemId,
				PersonIdentity = _user.UserId,
				ReferanceId = Guid.Parse(MeetingId)
			};
			updateMeeting.MeetingAttendeeCollection.Add(newItem);

			var update = _meetingRepository.Get(_connectionSting, _schema, updateMeeting, string.Empty,false);

			Assert.IsTrue(update.MeetingAttendeeCollection.Count == 1);
		}


		//[TestMethod]
		//public void IntegrationTest_GetMeeting_Add_Attendee_ShouldOnlyHaveOneInstanceOfUSer()
		//{
		//	var result = _meetingRepository.Get(_connectionSting, _schema, _user);
		//	var updateMeeting = result.FirstOrDefault(i => i.Id == Guid.Parse(MeetingId));

		//	var itemId = Guid.NewGuid();
		//	var newItem = new models.Entities.MeetingAttendee
		//	{
		//		Role = "Attendee",
		//		Id = itemId,
		//		PersonIdentity = _user.UserId,
		//		ReferanceId = Guid.Parse(MeetingId)
		//	};
		//	updateMeeting.MeetingAttendeeCollection.Add(newItem);

		//	var update = _meetingRepository.Get(_connectionSting, _schema, updateMeeting, false);

		//	Assert.IsTrue(update.MeetingAttendeeCollection.Count > 1);
		//}
	}
}
