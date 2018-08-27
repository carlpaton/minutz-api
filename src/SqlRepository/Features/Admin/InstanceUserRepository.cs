using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories.Feature.Admin;
using Minutz.Models.Entities;
using Minutz.Models.Message;

namespace SqlRepository.Features.Admin
{
    public class InstanceUserRepository :IInstanceUserRepository
    {
        public PersonResponse GetInstancePeople(string schema, string connectionString)
        {
            if (string.IsNullOrEmpty(schema) ||
                string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var instanceSql = $@"SELECT * FROM [APP].[Person] WHERE Related = '{schema}'";
                    var instanceData = dbConnection.Query<Person>(instanceSql).ToList();
                    return new PersonResponse
                           {
                               Code = 200,
                               Condition = true,
                               Message = "Success",
                               People = instanceData
                           };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new PersonResponse {Code = 500, Condition = false, Message = e.Message};
            }
        }
        
        public PersonResponse AddInstancePerson(Person person, string schema, string connectionString)
        {
            if (string.IsNullOrEmpty(schema) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid agenda identifier, schema or connection string.");
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(connectionString))
                {
                    dbConnection.Open();
                    var instanceSql = $@"INSERT INTO [APP].[Person] 
                                        ([IdentityId],[FirstName],[LastName],[FullName],[ProfilePicture],[Email],[Role],[Active],[InstanceId],[Related]) VALUES 
                                        ('{person.Identityid}','{person.FirstName}', '{person.LastName}', '{person.FullName}', 'default','{person.Email}','{person.Role}',1,'','{schema}')";
                    var insertData = dbConnection.Execute(instanceSql);
                    if (insertData == 1)
                    {
                        var selectInstanceSql = $@"SELECT * FROM [APP].[Person] WHERE [IdentityId] = '{person.Identityid}'";
                        var instanceData = dbConnection.Query<Person>(selectInstanceSql).FirstOrDefault();
                        if (instanceData == null)
                        {
                            return new PersonResponse
                                   {
                                       Code = 404,
                                       Condition = false,
                                       Message = "Could not find user that was added."
                                   };
                        }
                        return new PersonResponse
                               {
                                   Code = 200,
                                   Condition = true,
                                   Message = "Success",
                                   Person = instanceData
                               };
                    }

                    return new PersonResponse
                           {
                               Code = 404,
                               Condition = false,
                               Message = "Could not add new user."
                           };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new PersonResponse {Code = 500, Condition = false, Message = e.Message};
            }
        }
    }
}