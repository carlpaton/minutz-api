using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace Interface.Repositories
{
  public interface IUserRepository
  {
    bool CheckIfNewUser (
      (string key, string reference) reference,
      string authUserId, string schema, string connectionString);

    MessageBase CreateNewUser (
      AuthRestModel authUser, string connectionString);

    string CreateNewUser (
      (string key, string reference) relationship,
      AuthRestModel authUser, string schema, string connectionString);

    PersonResponse MinutzPersonCheckIfUserExistsByEmail 
      (string email, string minutzAppConnectionString);
    
    AuthRestModel GetUser 
      (string authUserId, string schema, string connectionString);

    string CreateNewSchema 
      (AuthRestModel authUser,string connectionString, string masterConnectionString);

    (bool condition, string message) UpdatePerson (
      string connectionString, string schema, Person person);

    (bool condition, string message) Reset (
      string connectionString, string instanceId, string instanceName);
  }
}