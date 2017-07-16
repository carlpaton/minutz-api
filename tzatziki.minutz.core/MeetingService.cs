using tzatziki.minutz.models.Entities;
using tzatziki.minutz.models.Auth;
using System.Collections.Generic;
using tzatziki.minutz.interfaces;
using System;

namespace tzatziki.minutz.core
{
	public class MeetingService : IMeetingService
	{
		private readonly IMeetingRepository _meetingRepository;
		internal readonly string _connectionString = Environment.GetEnvironmentVariable("SQLCONNECTION");

		public string MeetingQueryStringKey
		{
			get
			{
				return "meeting";
			}
		}
		public string MeetingReferalQueryStringKey
		{
			get
			{
				return "referal";
			}
		}

		public MeetingService(IMeetingRepository meetingRepository)
		{
			_meetingRepository = meetingRepository;
		}

		public Meeting Get(string schema, Meeting meeting, string callingUserId, bool read = false)
		{
			return _meetingRepository.Get(_connectionString, schema, meeting, callingUserId, read);
		}

		public IEnumerable<Meeting> Get(string schema, UserProfile user)
		{
			return _meetingRepository.Get(_connectionString, schema, user);
		}

		public void DeleteAgenda(string schema, string agendaItemId)
		{
			_meetingRepository.DeleteMeetingAgendaItem(_connectionString, schema, agendaItemId);
		}

		public MeetingAttachmentItem SaveFile(string schema, UserProfile user, string fileName, byte[] data, string meetingId)
		{
			return _meetingRepository.SaveFile(_connectionString, schema, user, fileName, data, meetingId);
		}

		public KeyValuePair<string, byte[]> GetFileData(string schema, string fileId)
		{
			return _meetingRepository.GetFileData(_connectionString, schema, fileId);
		}

		public IEnumerable<KeyValuePair<string, string>> ExtractQueries(string returnUri)
		{
			var queries = new List<KeyValuePair<string, string>>();
			var queryCollection = returnUri.Split('?');
			foreach (var query in queryCollection)
			{
				if (query.Contains("="))
				{
					var temp = query.Split('=');
					queries.Add(new KeyValuePair<string, string>(temp[0], temp[1]));
				}
			}
			return queries;
		}
	}
}
