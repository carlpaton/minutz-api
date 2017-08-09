using Dapper;
using minutz_interface.Entities;
using minutz_interface.Repositories;
using minutz_interface.ViewModels;
using minutz_models;
using minutz_models.Entities;
using minutz_models.ViewModels;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace minutz_sqlrepository
{
	public class PersonRepository : IPersonRepository
	{
		public IUserProfile Get(string identifier, string email, string name, string picture, string connectionString)
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

		public minutz_interface.RoleEnum GetRole(string identifier, string connectionString, IUserProfile profile)
		{
			var user = GetUser(identifier, connectionString);
			if (user != null)
				return (minutz_interface.RoleEnum)Enum.Parse(typeof(minutz_interface.RoleEnum), user.Role);

			CreateUser(connectionString, profile);
			return minutz_interface.RoleEnum.Attendee;
		}

		public minutz_interface.RoleEnum GetRole(string identifier, string connectionString, IUserProfile profile, string schema)
		{
			var user = GetUser(identifier, connectionString);
			if (user != null)
				return (minutz_interface.RoleEnum)Enum.Parse(typeof(minutz_interface.RoleEnum), user.Role);

			CreateUser(connectionString, profile,schema);
			return minutz_interface.RoleEnum.Attendee;
		}

		internal IPerson GetUser(string identifier, string connectionString, string schema = "app")
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				try
				{
					dbConnection.Open();
					var data = dbConnection.Query<Person>($"select * FROM [{schema}].[Person] WHERE Identityid = '{identifier}' ");
					return data.FirstOrDefault();
				}
				catch (Exception ex)
				{
					throw (ex);
				}
			}
		}

		internal Person CreateUser(string connectionString, IUserProfile profile, string schema = "app")
		{
			using (IDbConnection dbConnection = new SqlConnection(connectionString))
			{
				dbConnection.Open();
				var userObject = new Person
				{
					FirstName = profile.FirstName,
					LastName = profile.LastName,
					FullName = $"{profile.FirstName} {profile.LastName}",
					Email = profile.EmailAddress,
					ProfilePicture = profile.ProfileImage,
					Identityid = profile.UserId,
					Role = minutz_interface.RoleEnum.Attendee.ToString(),
					Active = true
				};
				var insertQuery = $@"INSERT INTO [{schema}].[Person]([Identityid], [FirstName], [LastName], [FullName], [ProfilePicture], [Email], [Role], [Active], [InstanceId])
														 VALUES(@Identityid, @FirstName, @LastName,@FullName, @ProfilePicture, @Email, @Role, @Active, @InstanceId)";
				try
				{
					dbConnection.Execute(insertQuery, userObject);
					return userObject;
				}
				catch (Exception ex)
				{
					throw (ex);
				}
			}
		}

	}
}