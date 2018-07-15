using System;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories
{
  public interface IUserRepository
  {
    [Obsolete("To replaced with: MessageBase CheckIfNewUser(string userEmail, string meetingId, string schema, string connectionString, string masterConnectionString);")]
    bool CheckIfNewUser ((string key, string reference) reference, string authUserId, string schema, string connectionString);

    MessageBase CheckIfNewUser(string userEmail, string meetingId, string schema, string connectionString, string masterConnectionString);
    
    MessageBase CreateNewUser(AuthRestModel authUser, string connectionString);

    MessageBase CreatePerson(MeetingAttendee user, string masterConnectionString);

    string CreateNewUser (
      (string key, string reference) relationship,
      AuthRestModel authUser, string schema, string connectionString);

    PersonResponse MinutzPersonCheckIfUserExistsByEmail 
      (string email, string minutzAppConnectionString);

    string GetAuthUserIdByEmail
      (string email, string connectionString);
    
    AuthRestModel GetUser 
      (string authUserId, string schema, string connectionString);

    Person GetUserByEmail
      (string email, string schema, string connectionString);
    
    string CreateNewSchema 
      (AuthRestModel authUser,string connectionString, string masterConnectionString);

    (bool condition, string message) UpdatePerson (
      string connectionString, string schema, Person person);

    (bool condition, string message) Reset (
      string connectionString, string instanceId, string instanceName);
  }
}