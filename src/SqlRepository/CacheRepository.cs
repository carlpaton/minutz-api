using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;
using Interface.Services;
using Minutz.Models.Auth0Models;

namespace SqlRepository
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IApplicationSetting _applicationSetting;

        public CacheRepository(IApplicationSetting applicationSetting)
        {
            _applicationSetting = applicationSetting;
        }

        public bool CheckUserTokenCache
            (string userIdentifier, string connectionString)
        {
            if (string.IsNullOrEmpty(userIdentifier) || string.IsNullOrEmpty(connectionString))
            {
                Console.Write("Please provide a valid meeting identifier, schema or connection string.");
                //throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
                return false;
            }
            Console.WriteLine($"userIdentifier: {userIdentifier}");
            Console.WriteLine("-----------------------------");
            Console.WriteLine($"connection: {connectionString}");
            Console.WriteLine("------");
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                var sql = $"SELECT COUNT(identifier) AS count from [app].[tokenCache] WHERE [identifier] = '{userIdentifier}' ";
                try
                {
                    var data = dbConnection.Query<int>(sql).ToList();
                    if (!data.Any())
                    {
                        return false;
                    }

                    return data.First() != 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }

        public TokenCacheModel GetUserTokenCache
            (string userIdentifier, string connectionString)
        {
            if (string.IsNullOrEmpty(userIdentifier) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                var sql = $"SELECT [token], [expire] [{_applicationSetting.Schema}].[tokenCache] WHERE [identifier] = '{userIdentifier}' ";
                try
                {
                    var data = dbConnection.Query<TokenCacheModel>(sql).ToList();
                    return !data.Any() ? null : data.First();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }
        
        public string GetUserTokenCache
            (string userIdentifier, string tokenJson ,string connectionString)
        {
            if (string.IsNullOrEmpty(userIdentifier) || string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Please provide a valid meeting identifier, schema or connection string.");
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                var sql = $"EXEC [{_applicationSetting.Schema}].[spTokenCache]'{userIdentifier}', '{tokenJson}'";
                try
                {
                    var data = dbConnection.Query<string>(sql).ToList();
                    return !data.Any() ? string.Empty : data.First();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return string.Empty;
                }
            }
        }
    }
}