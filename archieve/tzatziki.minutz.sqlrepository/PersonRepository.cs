using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using tzatziki.minutz.models.Auth;
using tzatziki.minutz.models.Entities;
using tzatziki.minutz.interfaces.Repositories;

namespace tzatziki.minutz.sqlrepository
{
	public class PersonRepository : IPersonRepository
	{
		public bool IsPerson(string email)
		{
			return false;
		}

		public UserProfile Get(string identifier, string email, string name, string picture,string connectionString)
		{
			var user = GetUser(identifier, connectionString);
			if (user != null)
			{
				return new UserProfile
				{
					InstanceId = string.IsNullOrEmpty(user.InstanceId) ? Guid.Empty : Guid.Parse(user.InstanceId),
					UserId = user.Identityid,
					Name = $"{user.FirstName} {user.LastName}",
					FirstName = user.FirstName,
					LastName = user.LastName,
					ProfileImage = user.ProfilePicture,
					EmailAddress = user.Email
				};
			}

			
			var newUserObject = new UserProfile
			{
				EmailAddress = email,
				UserId = identifier,
				ProfileImage = picture,
				Name = name
			};

			var split = name.Split(' ');
			if (split.Length > 1)
			{
				newUserObject.FirstName = split[0];
				newUserObject.LastName = split[1];
				
			}
			if (split.Length == 1)
			{
				newUserObject.FirstName = name;
				newUserObject.LastName = string.Empty;
			}

			CreateUser(connectionString, newUserObject);
			return newUserObject;
		}

		/// <summary>
		/// Get the role for a user from the connector database
		/// </summary>
		/// <param name="identifier">      Auth0 identifier</param>
		/// <param name="connectionString">database connection string</param>
		/// <param name="profile">         Auth0 populated UserProfile instance</param>
		/// <returns>Enum.RoleEnum</returns>
		public RoleEnum GetRole(string identifier, string connectionString, UserProfile profile)
		{
			var user = GetUser(identifier, connectionString);
			if (user != null)
				return (RoleEnum)Enum.Parse(typeof(RoleEnum), user.Role);

			CreateUser(connectionString, profile);
			return RoleEnum.Attendee;
		}

		public UserProfile InsertInstanceIdForUser(UserProfile user, string connectionString)
		{
			using (var context = new DBConnectorContext(connectionString, "app"))
			{
				context.Database.EnsureCreated();

				var dbUser = context.Person.FirstOrDefault(i => i.Identityid == user.UserId);
				dbUser.InstanceId = user.InstanceId.ToString();
				dbUser.ProfilePicture = user.ProfileImage;
				dbUser.Role = RoleEnum.Admin.ToString();
				user.Role = RoleEnum.Admin.ToString();

				try
				{
					context.SaveChanges();
				}
				catch (Exception ex)
				{
					throw (ex);
				}
				return user;
			}
		}

		public void CreateInstanceUser(UserProfile userProfile, string connectionString, string schema)
		{
			using (var context = new DBConnectorContext(connectionString, schema))
			{
				try
				{
					context.Database.EnsureCreated();
					var user = CreateUser(connectionString, userProfile, schema);
				}
				catch (Exception ex)
				{
					throw (ex);
				}
			}
		}

		internal models.Entities.Person GetUser(string identifier, string connectionString, string schema = "app")
		{
			using (var context = new DBConnectorContext(connectionString, schema))
			{
				try
				{
					context.Database.EnsureCreated();
					return context.Person.FirstOrDefault(i => i.Identityid == identifier);
				}
				catch (Exception ex)
				{
					throw (ex);
				}
			}
		}

		internal Person CreateUser(string connectionString, UserProfile profile, string schema = "app")
		{
			
			using (var context = new DBConnectorContext(connectionString, schema))
			{
				context.Database.EnsureCreated();
				var userObject = new Person
				{
					FirstName = profile.FirstName,
					LastName = profile.LastName,
					FullName = $"{profile.FirstName} {profile.LastName}",
					Email = profile.EmailAddress,
					ProfilePicture = profile.ProfileImage,
					Identityid = profile.UserId,
					Role = RoleEnum.Attendee.ToString(),
					Active = true
				};
				context.Person.Add(userObject);
				try
				{
					context.SaveChanges();
					return userObject;
				}
				catch (Exception ex)
				{
					throw (ex);
				}
			}
		}

		public Guid GetInstanceIdForUser(string userIdentifier, string connectionString, string schema = "app")
		{
			using (var context = new DBConnectorContext(connectionString, schema))
			{
				context.Database.EnsureCreated();
				var user = context.Person.FirstOrDefault(i => i.Identityid == userIdentifier);
				if (user == null)
					return Guid.Empty;
				if (user.InstanceId == null)
					return Guid.Empty;

				return Guid.Parse(user.InstanceId);
			}
		}

		public IEnumerable<models.Entities.Person> GetSystemUsers(string connectionString, string schema = "app")
		{
			using (var context = new DBConnectorContext(connectionString, schema))
			{
				context.Database.EnsureCreated();
				var users = context.Person.Where(i => i.Active == true).ToList();
				return users;
			}
		}

		public bool InvitePerson(UserProfile person, string connectionString, string schema)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(connectionString))
				{
					con.Open();
					using (SqlCommand command = new SqlCommand(_insertSchemaUsersStatement(person,schema), con))
					{
						command.ExecuteNonQuery();
					}
					using (SqlCommand command = new SqlCommand(_insertUsersStatement(person, "app"), con))
					{
						command.ExecuteNonQuery();
					}
					con.Close();
				}
				return true;
			}
			catch (Exception )
			{
				return false;
			}
		}

		public IEnumerable<UserProfile> GetSchemaUsers(string connectionString, string schema)
		{
			var result = new List<UserProfile>();
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();
				using (SqlCommand command = new SqlCommand(_getSchemaUsersStatement(schema), con))
				{
					using (SqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							result.Add(ToUserProfile(reader));
						}
					}
				}
				con.Close();
			}
			return result;
		}
		internal UserProfile ToUserProfile(SqlDataReader dataReader)
		{
      return new UserProfile
			{
				UserId = dataReader["Identity"].ToString(),
				Name = dataReader["FullName"].ToString(),
				EmailAddress = dataReader["Email"].ToString(),
				FirstName = dataReader["FirstName"].ToString(),
				LastName = dataReader["LastName"].ToString(),
				ProfileImage = dataReader["ProfilePicture"].ToString(),
				//Id = Guid.Parse(dataReader["Id"].ToString()),
				Active = bool.Parse(dataReader["Active"].ToString()),
				Role = dataReader["Role"].ToString()
			};
		}

		internal string _getSchemaUsersStatement(string schema)
		{
			return $@"SELECT * FROM [{schema}].[User]";
		}

		internal string _insertSchemaUsersStatement(UserProfile person, string schema)
		{
			var active = person.Active == true ? 1 : 0;
			return $@"INSERT INTO
							[{schema}].[User]
							VALUES('{person.UserId}','{person.FirstName}','{person.LastName}','{person.Name}','{person.ProfileImage}','{person.EmailAddress}','{person.Role}',{active})
							";
		}

		internal string _insertUsersStatement(UserProfile person, string schema)
		{
			var active = person.Active == true ? 1 : 0;
			return $@"INSERT INTO
							[{schema}].[Person]
							VALUES('{person.UserId}','{person.FirstName}','{person.LastName}','{person.Name}','{person.ProfileImage}','{person.EmailAddress}','{person.Role}',{active},null)
							";
		}
	}
}