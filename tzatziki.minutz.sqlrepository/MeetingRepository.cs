using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using tzatziki.minutz.interfaces;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.sqlrepository
{
	public class MeetingRepository : IMeetingRepository
	{
		private const string _createMeetingSchemaStoredProcedure = "createMeetingSchema";
		private const string _deleteMeetingSchemaStoredProcedure = "deleteMeetingSchema";
		private const string _meetingTableName = "Meeting";
		private const string _meetingAgendaTableName = "MeetingAgenda";
		private const string _meetingOwnerIdProperty = "MeetingOwnerId";
		private const string _meetingAttendee = "MeetingAttendee";
		private const string _meetingAction = "MeetingAction";
		private const string _meetingNote = "MeetingNote";
		private const string _meetingAttachment = "MeetingAttachment";

		private readonly ITableService _tableService;

		public MeetingRepository(ITableService tableService)
		{
			_tableService = tableService;
		}

		public void DeleteMeetingSchema(string connectionString, string schema, UserProfile user)
		{
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				using (SqlCommand command = new SqlCommand(_deleteMeetingSchemaStoredProcedure, con))
				{
					command.CommandType = CommandType.StoredProcedure;
					command.Parameters.Add(new SqlParameter("@tenant", schema));
					command.Parameters.Add(new SqlParameter("@userIdentity", user.InstanceId));
					command.Parameters.Add(new SqlParameter("@instanceId", schema.Split('_')[1]));
					con.Open();
					command.ExecuteNonQuery();
					con.Close();
				}
			}
		}

		public void DeleteMeetingAgendaItem(string connectionString, string schema, string agendaItemId)
		{
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				using (SqlCommand command = new SqlCommand(DeleteMeetingAgendaItemStatement(schema, agendaItemId), con))
				{
					con.Open();
					command.ExecuteNonQuery();
					con.Close();
				}
			}
		}

		public void SaveFile(string connectionString, string schema, UserProfile user, string fileName, byte[] data, string meetingId)
		{
			var createdDate = DateTime.UtcNow;
			var meetingAttendeeId = Guid.NewGuid();
			var entry = new MeetingAttachmentItem
			{
				Date = createdDate,
				FileData = data,
				FileName = fileName,
				Id = Guid.NewGuid(),
				MeetingAttendeeId = meetingAttendeeId,
				ReferanceId = Guid.Parse( meetingId)
			}; try
			{
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					using (SqlCommand command = new SqlCommand($"INSERT INTO [{schema}].[MeetingAttachment] VALUES(@Id,@ReferanceId, @FileName,@MeetingAttendeeId, @Date,@FileData)", con))
					{
						con.Open();
						command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
						command.Parameters.Add("@ReferanceId", SqlDbType.UniqueIdentifier).Value = entry.ReferanceId;
						command.Parameters.Add("@FileName", SqlDbType.VarChar).Value = entry.FileName;
						command.Parameters.Add("@MeetingAttendeeId", SqlDbType.UniqueIdentifier).Value = entry.MeetingAttendeeId;
						command.Parameters.Add("@Date", SqlDbType.DateTime2).Value = entry.Date;
						command.Parameters.Add("@FileData", SqlDbType.VarBinary, data.Length).Value = data;
						command.ExecuteNonQuery();
						con.Close();
					}
				}
			}
			catch (Exception ex)
			{
				var q = ex;
			}
		}

		public IEnumerable<Meeting> Get(string connectionString, string schema, UserProfile user)
		{
			var result = new List<Meeting>();
			if (_tableService.Initiate(connectionString, schema, _meetingTableName, _createMeetingSchemaStoredProcedure))
			{
				foreach (Meeting meeting in GetUserMeetings(connectionString, schema, user))
				{
					result.Add(ToMeeting(connectionString, schema, meeting));
				}
			}
			return result;
		}

		public Meeting Get(string connectionString, string schema, Meeting meeting, string callingUserId, bool read = false)
		{
			if (_tableService.Initiate(connectionString, schema, _meetingTableName, _createMeetingSchemaStoredProcedure))
			{
				var instance = ToMeeting(connectionString, schema, $" Id = '{meeting.Id}'");
				if (instance == null)
				{
					if (string.IsNullOrEmpty(meeting.MeetingOwnerId)) meeting.MeetingOwnerId = callingUserId;
					instance = ToMeeting(connectionString, schema, meeting, false);
					var collectionFilter = $" ReferanceId = '{instance.Id}'";
					instance.MeetingAgendaCollection = ToMeetingAgenda(connectionString, schema, collectionFilter).ToList();
					instance.MeetingAttendeeCollection = ToMeetingAttendee(connectionString, schema, collectionFilter).ToList();
					instance.MeetingNoteCollection = ToMeetingNote(connectionString, schema, collectionFilter).ToList();
					instance.MeetingAttachmentCollection = ToMeetingAttachment(connectionString, schema, collectionFilter).ToList();
				}
				else
				{
					if (read)
					{
						var collectionFilter = $" ReferanceId = '{instance.Id.ToString()}'";
						instance.MeetingAgendaCollection = ToMeetingAgenda(connectionString, schema, collectionFilter).ToList();
						instance.MeetingAttendeeCollection = ToMeetingAttendee(connectionString, schema, collectionFilter).ToList();
						instance.MeetingNoteCollection = ToMeetingNote(connectionString, schema, collectionFilter).ToList();
						instance.MeetingAttachmentCollection = ToMeetingAttachment(connectionString, schema, collectionFilter).ToList();
						return instance;
					}
					else
					{
						instance = ToMeeting(connectionString, schema, meeting, true);
						var collectionFilter = $" ReferanceId = '{instance.Id.ToString()}'";
						instance.MeetingAgendaCollection = ToMeetingAgenda(connectionString, schema, collectionFilter).ToList();
						instance.MeetingAttendeeCollection = ToMeetingAttendee(connectionString, schema, collectionFilter).ToList();
						instance.MeetingNoteCollection = ToMeetingNote(connectionString, schema, collectionFilter).ToList();
						instance.MeetingAttachmentCollection = ToMeetingAttachment(connectionString, schema, collectionFilter).ToList();
					}
				}
				return instance;
			}
			throw new Exception($"Error retrieving the meeting instance for: {schema}, {meeting.Id}");
		}

		internal IEnumerable<Meeting> GetUserMeetings(string connectionString, string schema, UserProfile user)
		{
			return ToList(schema, connectionString, $" {_meetingOwnerIdProperty} = '{user.UserId}'");
		}

		internal List<Meeting> ToList(string schema, string connectionString)
		{
			var result = new List<Meeting>();
			if (_tableService.Initiate(connectionString, schema, _meetingTableName, _createMeetingSchemaStoredProcedure))
			{
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					using (SqlCommand command = new SqlCommand(SelectMeetingStatement(schema), con))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								var meeting = ToMeeting(reader);
								var collectionFilter = $" ReferanceId = '{meeting.Id}'";
								meeting.MeetingAgendaCollection = ToMeetingAgenda(connectionString, schema, collectionFilter).ToList();
								meeting.MeetingAttendeeCollection = ToMeetingAttendee(connectionString, schema, collectionFilter).ToList();
								meeting.MeetingNoteCollection = ToMeetingNote(connectionString, schema, collectionFilter).ToList();
								meeting.MeetingAttachmentCollection = ToMeetingAttachment(connectionString, schema, collectionFilter).ToList();
								result.Add(meeting);
							}
						}
					}
				}
			}
			return result;
		}

		internal List<Meeting> ToList(string schema, string connectionString, string filter)
		{
			var result = new List<Meeting>();
			if (_tableService.Initiate(connectionString, schema, _meetingTableName, _createMeetingSchemaStoredProcedure))
			{
				if (schema != "account_00000000000000000000000000000000")
				{
					using (SqlConnection con = new SqlConnection(connectionString))
					{
						con.Open();
						using (SqlCommand command = new SqlCommand(SelectMeetingStatement(schema, filter), con))
						{
							using (SqlDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{
									var meeting = ToMeeting(reader);
									var collectionFilter = $" ReferanceId = '{meeting.Id}'";
									meeting.MeetingAgendaCollection = ToMeetingAgenda(connectionString, schema, collectionFilter).ToList();
									meeting.MeetingAttendeeCollection = ToMeetingAttendee(connectionString, schema, collectionFilter).ToList();
									meeting.MeetingNoteCollection = ToMeetingNote(connectionString, schema, collectionFilter).ToList();
									meeting.MeetingAttachmentCollection = ToMeetingAttachment(connectionString, schema, collectionFilter).ToList();

									result.Add(meeting);

								}
							}
						}
						con.Close();
					}
				}
			}
			return result;
		}

		internal Meeting ToMeeting(string connectionString, string schema, string filter)
		{
			Meeting result;
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();

				using (SqlCommand command = new SqlCommand(SelectMeetingStatement(schema, filter), con))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							return result = ToMeeting(reader);
						}
					}
				}
				con.Close();
			}
			return null;
		}

		internal IEnumerable<MeetingAgendaItem> ToMeetingAgenda(string connectionString, string schema, string filter)
		{
			var result = new List<MeetingAgendaItem>();
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();

				using (SqlCommand command = new SqlCommand(SelectMeetingAgendaStatement(schema, filter), con))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							//check if null , if null then create else update and return

							result.Add(ToMeetingAgenda(reader));
						}
					}
				}
				con.Close();
			}
			return result;
		}

		internal IEnumerable<MeetingAttendee> ToMeetingAttendee(string connectionString, string schema, string filter)
		{
			var result = new List<MeetingAttendee>();
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();
				using (SqlCommand command = new SqlCommand(SelectMeetingAttendeeStatement(schema, filter), con))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							result.Add(ToMeetingAttendee(reader));
						}
					}
				}
				con.Close();
			}
			return result;
		}

		internal IEnumerable<MeetingNoteItem> ToMeetingNote(string connectionString, string schema, string filter)
		{
			var result = new List<MeetingNoteItem>();
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();
				using (SqlCommand command = new SqlCommand(SelectMeetingNoteStatement(schema, filter), con))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							result.Add(ToMeetingNote(reader));
						}
					}
				}
				con.Close();
			}
			return result;
		}

		internal IEnumerable<MeetingAttachmentItem> ToMeetingAttachment(string connectionString, string schema, string filter)
		{
			var result = new List<MeetingAttachmentItem>();
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();
				using (SqlCommand command = new SqlCommand(SelectMeetingAttachmentStatement(schema, filter), con))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							result.Add(ToMeetingAttachment(reader));
						}
					}
				}
				con.Close();
			}
			return result;
		}

		internal Meeting ToMeeting(string connectionString, string schema, Meeting meeting, bool update = false)
		{
			if (_tableService.Initiate(connectionString, schema, _meetingTableName, _createMeetingSchemaStoredProcedure))
			{
				if (update)
				{
					using (SqlConnection con = new SqlConnection(connectionString))
					{
						con.Open();
						using (SqlCommand command = new SqlCommand(UpdateMeetingStatement(schema, meeting), con))
						{
							command.ExecuteNonQuery();
							if (meeting.MeetingAgendaCollection == null) throw new NullReferenceException("MeetingAgendaCollection is null.");
							foreach (var agendaItem in meeting.MeetingAgendaCollection)
							{
								var agendaItemFilter = $" Id = '{agendaItem.Id}'";
								var currentAgendaItem = ToMeetingAgenda(connectionString, schema, agendaItemFilter);
								if (currentAgendaItem == null || currentAgendaItem.Count() < 1)
								{
									using (SqlCommand agendaItemsCommand = new SqlCommand(InsertMeetingAgendaItemStatement(schema, agendaItem), con))
									{
										agendaItemsCommand.ExecuteNonQuery();
									}
								}
								else
								{
									using (SqlCommand agendaItemsCommand = new SqlCommand(UpdateMeetingAgendaItemStatement(schema, agendaItem), con))
									{
										agendaItemsCommand.ExecuteNonQuery();
									}
								}
							}

							if (meeting.MeetingAttendeeCollection == null) throw new NullReferenceException("MeetingAttendeeCollection is null.");
							foreach (var attendee in meeting.MeetingAttendeeCollection)
							{
								var attendeeFilter = $" PersonIdentity = '{attendee.PersonIdentity}'";
								var currentAttendee = ToMeetingAttendee(connectionString, schema, attendeeFilter);
								if (currentAttendee == null || currentAttendee.Count() < 1)
								{
									using (SqlCommand agendaItemsCommand = new SqlCommand(InsertMeetingAttendeeStatement(schema, attendee), con))
									{
										agendaItemsCommand.ExecuteNonQuery();
									}
								}
								else
								{
									using (SqlCommand agendaItemsCommand = new SqlCommand(UpdateMeetingAttendeeStatement(schema, attendee), con))
									{
										agendaItemsCommand.ExecuteNonQuery();
									}
								}
							}

							if (meeting.MeetingAttachmentCollection == null) throw new NullReferenceException("MeetingAttachmentCollection is null.");
							foreach (var agendaAttachment in meeting.MeetingAttachmentCollection)
							{
								var agendaAttachmentFilter = $" Id = '{agendaAttachment.Id}'";
								var currentAttachment = ToMeetingAttachment(connectionString, schema, agendaAttachmentFilter);
								if (currentAttachment == null)
								{
									using (SqlCommand attachmentCommand = new SqlCommand(InsertMeetingAttachmentStatement(schema, agendaAttachment), con))
									{
										attachmentCommand.ExecuteNonQuery();
									}
								}
								else
								{
									using (SqlCommand attachmentCommand = new SqlCommand(UpdateMeetingAttachmentStatement(schema, agendaAttachment), con))
									{
										attachmentCommand.ExecuteNonQuery();
									}
								}
							}

							if (meeting.MeetingNoteCollection == null) throw new NullReferenceException("MeetingNoteCollection is null.");
							foreach (var meetingNote in meeting.MeetingNoteCollection)
							{
								var agendaNoteFilter = $" Id = '{meetingNote.Id}'";
								var currentNote = ToMeetingNote(connectionString, schema, agendaNoteFilter);
								if (currentNote == null)
								{
									using (SqlCommand noteCommand = new SqlCommand(InsertMeetingNoteStatement(schema, meetingNote), con))
									{
										noteCommand.ExecuteNonQuery();
									}
								}
								else
								{
									using (SqlCommand noteCommand = new SqlCommand(UpdateMeetingNoteStatement(schema, meetingNote), con))
									{
										noteCommand.ExecuteNonQuery();
									}
								}

							}
						}
						con.Close();
					}
				}
				else
				{
					using (SqlConnection con = new SqlConnection(connectionString))
					{
						con.Open();
						var meetingFilter = $" Id = '{meeting.Id}'";
						var currentMeeting = ToMeeting(connectionString, schema, meetingFilter);
						if (currentMeeting == null)
						{
							using (SqlCommand command = new SqlCommand(InsertMeetingStatement(schema, meeting), con))
							{
								command.ExecuteNonQuery();
								if (meeting.MeetingAgendaCollection == null) meeting.MeetingAgendaCollection = new List<MeetingAgendaItem>();
								foreach (var agendaItem in meeting.MeetingAgendaCollection)
								{
									using (SqlCommand agendaItemsCommand = new SqlCommand(InsertMeetingAgendaItemStatement(schema, agendaItem), con))
									{
										agendaItemsCommand.ExecuteNonQuery();
									}
								}
								if (meeting.MeetingAttachmentCollection == null) meeting.MeetingAttachmentCollection = new List<MeetingAttachmentItem>();
								foreach (var agendaAttachment in meeting.MeetingAttachmentCollection)
								{
									using (SqlCommand attachmentCommand = new SqlCommand(InsertMeetingAttachmentStatement(schema, agendaAttachment), con))
									{
										attachmentCommand.ExecuteNonQuery();
									}
								}
								if (meeting.MeetingNoteCollection == null) meeting.MeetingNoteCollection = new List<MeetingNoteItem>();
								foreach (var meetingNote in meeting.MeetingNoteCollection)
								{
									using (SqlCommand noteCommand = new SqlCommand(InsertMeetingNoteStatement(schema, meetingNote), con))
									{
										noteCommand.ExecuteNonQuery();
									}
								}
							}
						}
						con.Close();
					}
				}
			}
			var data = ToMeeting(connectionString, schema, $" Id = '{meeting.Id}'");
			//update the items
			var collectionFilter = $" ReferanceId = '{data.Id.ToString()}'";
			data.MeetingAgendaCollection = ToMeetingAgenda(connectionString, schema, collectionFilter).ToList();
			data.MeetingAttendeeCollection = ToMeetingAttendee(connectionString, schema, collectionFilter).ToList();
			data.MeetingNoteCollection = ToMeetingNote(connectionString, schema, collectionFilter).ToList();
			data.MeetingAttachmentCollection = ToMeetingAttachment(connectionString, schema, collectionFilter).ToList();
			return data;
		}

		internal string SelectMeetingStatement(string schema)
		{
			return $"SELECT * FROM [{schema}].[Meeting] ";
		}

		///SELECT STATEMENTS

		internal string SelectMeetingStatement(string schema, String filter)
		{
			return $"SELECT * FROM [{schema}].[{_meetingTableName}] WHERE {filter}";
		}
		internal string SelectMeetingAgendaStatement(string schema, String filter)
		{
			return $"SELECT * FROM [{schema}].[{_meetingAgendaTableName}] WHERE {filter}";
		}

		internal string SelectMeetingAttendeeStatement(string schema, String filter)
		{
			return $"SELECT * FROM [{schema}].[{_meetingAttendee}] WHERE {filter}";
		}

		internal string SelectMeetingNoteStatement(string schema, String filter)
		{
			return $"SELECT * FROM [{schema}].[{_meetingNote}] WHERE {filter}";
		}

		internal string SelectMeetingAttachmentStatement(string schema, String filter)
		{
			return $"SELECT * FROM [{schema}].[{_meetingAttachment}] WHERE {filter}";
		}

		///UPDATE STATEMENTS

		internal string UpdateMeetingStatement(string schema, Meeting meeting)
		{
			var isFormal = meeting.IsFormal == true ? 1 : 0;
			var isReacurance = meeting.IsReacurance == true ? 1 : 0;
			var isPrivate = meeting.IsPrivate == true ? 1 : 0;
			var isLocked = meeting.IsLocked == true ? 1 : 0;
			return $@"UPDATE [{schema}].[{_meetingTableName}]
        SET [Name] = '{meeting.Name.EmptyIfNull()}',
        [Location] ='{meeting.Location.EmptyIfNull()}',
        [Date] = '{meeting.Date}',
        [Time] = '{meeting.Time.EmptyIfNull()}',
        [Duration] = '{meeting.Duration.EmptyIfNull()}',
        [IsReacurance] = {isReacurance},
        [IsPrivate] = {isPrivate},
        [ReacuranceType] = '{meeting.ReacuranceType}',
        [IsLocked] = {isLocked},
        [IsFormal] = {isFormal},
        [TimeZone] = '{meeting.TimeZone.EmptyIfNull()}',
        [Tag] = '{string.Join(",", meeting.Tag)}',
        [Purpose] = '{meeting.Purpose.EmptyIfNull()}',
        [Outcome] = '{meeting.Outcome.EmptyIfNull()}'
        WHERE [Id] = '{meeting.Id}'";
		}
		internal string UpdateMeetingAgendaItemStatement(string schema, MeetingAgendaItem meetingAgendaItem)
		{
			var isComplete = meetingAgendaItem.IsComplete == true ? 1 : 0;
			return $@"UPDATE [{schema}].[{_meetingAgendaTableName}] SET
        [ReferanceId] = '{meetingAgendaItem.ReferanceId}',
        [AgendaHeading] = '{meetingAgendaItem.AgendaHeading.EmptyIfNull()}',
        [AgendaText] = '{meetingAgendaItem.AgendaText.EmptyIfNull()}',
        [MeetingAttendeeId] = '{meetingAgendaItem.MeetingAttendeeId}',
        [Duration] = '{meetingAgendaItem.Duration.EmptyIfNull()}',
        [CreatedDate] = '{meetingAgendaItem.CreatedDate}',
        [IsComplete] = {isComplete}
        WHERE [Id] = '{meetingAgendaItem.Id}'";
		}
		internal string UpdateMeetingAttachmentStatement(string schema, MeetingAttachmentItem meetingAttachmentItem)
		{
			return $@"UPDATE [{schema}].[{_meetingAttachment}] SET
        [ReferanceId] = '{meetingAttachmentItem.ReferanceId}',
        [FileName] = '{meetingAttachmentItem.FileName.EmptyIfNull()}',
        [MeetingAttendeeId] = '{meetingAttachmentItem.MeetingAttendeeId}',
        [Date] = '{meetingAttachmentItem.Date.ToString()}'
        WHERE [Id] = '{meetingAttachmentItem.Id}'";
		}
		internal string UpdateMeetingNoteStatement(string schema, MeetingNoteItem meetingNote)
		{
			return $@"UPDATE [{schema}].[{_meetingNote}] SET
        [ReferanceId] = '{meetingNote.ReferanceId}',
        [NoteText] = '{meetingNote.NoteText.EmptyIfNull()}',
        [MeetingAttendeeId] = '{meetingNote.MeetingAttendeeId}',
        [CreatedDate] = '{meetingNote.CreatedDate.ToString()}'
        WHERE [Id] = '{meetingNote.Id}'";
		}
		internal string UpdateMeetingAttendeeStatement(string schema, MeetingAttendee meetingAttendee)
		{
			return $@"UPDATE [{schema}].[{_meetingAttendee}] SET
        [ReferanceId] = '{meetingAttendee.ReferanceId}',
        [PersonIdentity] = '{meetingAttendee.PersonIdentity.EmptyIfNull()}',
        [Role] = '{meetingAttendee.Role.EmptyIfNull()}'
        WHERE [Id] = '{meetingAttendee.Id}'";
		}
		///INSERT STATEMENTS

		internal string InsertMeetingStatement(string schema, Meeting meeting)
		{
			var isFormal = meeting.IsFormal == true ? 1 : 0;
			var isReacurance = meeting.IsReacurance == true ? 1 : 0;
			var isPrivate = meeting.IsPrivate == true ? 1 : 0;
			var isLocked = meeting.IsLocked == true ? 1 : 0;
			var meetingDate = meeting.Date == DateTime.MinValue ? DateTime.UtcNow.ToString() : meeting.Date.ToUniversalTime().ToString();
			if (string.IsNullOrEmpty(meeting.Name))
				meeting.Name = $"Meeting {DateTime.UtcNow.ToString()}";
			if (meeting.Tag == null) meeting.Tag = new string[] { };
			return $@"INSERT INTO [{schema}].[{_meetingTableName}] VALUES (
        '{meeting.Id}',
        '{meeting.Name.EmptyIfNull()}',
        '{meeting.Location.EmptyIfNull()}',
        '{meetingDate}',
        '{DateTime.UtcNow.ToString()}',
        '{meeting.Time.EmptyIfNull()}',
        '{meeting.Duration.EmptyIfNull()}',
        {isReacurance},
        {isPrivate},
        '{meeting.ReacuranceType.EmptyIfNull()}',
        {isLocked},
        {isFormal},
        '{meeting.TimeZone.EmptyIfNull()}',
        '{string.Join(",", meeting.Tag)}',
        '{meeting.Purpose.EmptyIfNull()}',
        '{meeting.MeetingOwnerId}',
        '{meeting.Outcome.EmptyIfNull()}'
        )";
		}
		internal string InsertMeetingAgendaItemStatement(string schema, MeetingAgendaItem meetingAgendaItem)
		{
			var isComplete = meetingAgendaItem.IsComplete == true ? 1 : 0;

			return $@"INSERT INTO [{schema}].[{_meetingAgendaTableName}] VALUES (
        '{meetingAgendaItem.Id}',
        '{meetingAgendaItem.ReferanceId}',
        '{meetingAgendaItem.AgendaHeading.EmptyIfNull()}',
        '{meetingAgendaItem.AgendaText.EmptyIfNull()}',
        '{meetingAgendaItem.MeetingAttendeeId}',
        '{meetingAgendaItem.Duration.EmptyIfNull()}',
        '{meetingAgendaItem.CreatedDate}',
        {isComplete}
        )";
		}
		internal string InsertMeetingAttendeeStatement(string schema, MeetingAttendee meetingAttendee)
		{
			return $@"INSERT INTO [{schema}].[{_meetingAttendee}] VALUES (
        '{meetingAttendee.Id}',
        '{meetingAttendee.ReferanceId}',
        '{meetingAttendee.PersonIdentity.EmptyIfNull()}',
        '{meetingAttendee.Role.EmptyIfNull()}'
        )";
		}
		internal string InsertMeetingNoteStatement(string schema, MeetingNoteItem meetingNote)
		{
			return $@"INSERT INTO [{schema}].[{_meetingNote}] VALUES (
        '{meetingNote.Id}',
        '{meetingNote.ReferanceId}',
        '{meetingNote.NoteText.EmptyIfNull()}',
        '{meetingNote.MeetingAttendeeId}',
        '{meetingNote.CreatedDate.ToString()}'
        )";
		}
		internal string InsertMeetingAttachmentStatement(string schema, MeetingAttachmentItem meetingAttachmentItem)
		{
			return $@"SELECT '{meetingAttachmentItem.Id}' AS Id,
						'{meetingAttachmentItem.ReferanceId}' AS ReferanceId,
						'{meetingAttachmentItem.FileName.EmptyIfNull()}' AS FileName,
						'{meetingAttachmentItem.MeetingAttendeeId}' AS MeetingAttendeeId,
						'{meetingAttachmentItem.Date.ToString()}' AS Date,
						'Content = CAST('{meetingAttachmentItem.FileData.ToString()}' AS VARBINARY(MAX)) AS FileData'
						INTO [{schema}].[{_meetingAttachment}]";
		}


		///DELETE STATEMENTS
		internal string DeleteMeetingAgendaItemStatement(string schema, string agendaItemId)
		{
			return $@"DELETE FROM [{schema}].[MeetingAgenda] WHERE Id = '{agendaItemId}'";
		}

		///DATA READER

		internal Meeting ToMeeting(SqlDataReader dataReader)
		{
			return new Meeting
			{
				Id = Guid.Parse(dataReader["Id"].ToString()),
				Name = dataReader["Name"].ToString(),
				Location = dataReader["Location"].ToString(),
				Date = DateTime.Parse(dataReader["Date"].ToString()),
				UpdatedDate = DateTime.Parse(dataReader["UpdatedDate"].ToString()),
				Time = dataReader["Time"].ToString(),
				Duration = Int32.Parse(dataReader["Duration"].ToString()),
				IsReacurance = bool.Parse(dataReader["IsReacurance"].ToString()),
				IsPrivate = bool.Parse(dataReader["IsPrivate"].ToString()),
				IsLocked = bool.Parse(dataReader["IsLocked"].ToString()),
				IsFormal = bool.Parse(dataReader["IsFormal"].ToString()),
				ReacuranceType = dataReader["ReacuranceType"].ToString(),
				Tag = dataReader["Tag"].ToString().Split(','),
				Purpose = dataReader["Purpose"].ToString(),
				Outcome = dataReader["Outcome"].ToString(),
				MeetingOwnerId = dataReader["MeetingOwnerId"].ToString(),
				TimeZone = dataReader["TimeZone"].ToString()
			};
		}

		internal MeetingAgendaItem ToMeetingAgenda(SqlDataReader dataReader)
		{
			return new MeetingAgendaItem
			{
				AgendaHeading = dataReader["AgendaHeading"].ToString(),
				AgendaText = dataReader["AgendaText"].ToString(),
				CreatedDate = DateTime.Parse(dataReader["CreatedDate"].ToString()),
				Duration = dataReader["Duration"].ToString(),
				Id = Guid.Parse(dataReader["Id"].ToString()),
				IsComplete = bool.Parse(dataReader["IsComplete"].ToString()),
				MeetingAttendeeId = dataReader["MeetingAttendeeId"].ToString(),
				ReferanceId = Guid.Parse(dataReader["ReferanceId"].ToString())
			};
		}

		internal MeetingAttendee ToMeetingAttendee(SqlDataReader dataReader)
		{
			return new MeetingAttendee
			{
				Id = Guid.Parse(dataReader["Id"].ToString()),
				Role = dataReader["Role"].ToString(),
				PersonIdentity = dataReader["PersonIdentity"].ToString(),
				ReferanceId = Guid.Parse(dataReader["ReferanceId"].ToString())
			};
		}

		internal MeetingAttachmentItem ToMeetingAttachment(SqlDataReader dataReader)
		{
			return new MeetingAttachmentItem
			{
				Id = Guid.Parse(dataReader["Id"].ToString()),
				Date = DateTime.Parse(dataReader["Date"].ToString()),
				FileName = dataReader["FileName"].ToString(),
				MeetingAttendeeId = Guid.Parse(dataReader["ReferanceId"].ToString()),
				ReferanceId = Guid.Parse(dataReader["ReferanceId"].ToString()),
				//FileData = byte[]
			};
		}

		internal MeetingNoteItem ToMeetingNote(SqlDataReader dataReader)
		{
			return new MeetingNoteItem
			{
				Id = Guid.Parse(dataReader["Id"].ToString()),
				CreatedDate = DateTime.Parse(dataReader["CreatedDate"].ToString()),
				NoteText = dataReader["NoteText"].ToString(),
				MeetingAttendeeId = Guid.Parse(dataReader["ReferanceId"].ToString()),
				ReferanceId = Guid.Parse(dataReader["ReferanceId"].ToString()),
			};
		}


	}
}

public static class Extensions
{
	public static string EmptyIfNull(this object value)
	{
		return value?.ToString() ?? string.Empty;
	}
}