﻿using Minutz.Models.Entities;

namespace Interface.Repositories
{
  public interface IUserRepository
  {
    bool CheckIfNewUser (
      (string key, string reference) reference,
      string authUserId, string schema, string connectionString);

    (bool condition, string message) CreateNewUser (
      AuthRestModel authUser, string schema, string connectionString);

    string CreateNewUser (
      (string key, string reference) relationship,
      AuthRestModel authUser, string schema, string connectionString);

    (bool condition, string message, Person person) GetUserByEmail (
      string email, string schema, string connectionString);
    AuthRestModel GetUser (
      string authUserId, string schema, string connectionString);

    string CreateNewSchema (
      AuthRestModel authUser, string connectionString, string masterConnectionString);

    (bool condition, string message) Reset (
      string connectionString, string instanceId, string instanceName);
  }
}