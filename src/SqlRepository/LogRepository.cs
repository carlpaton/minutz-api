using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Interface.Repositories;

namespace SqlRepository {
  public class LogRepository : ILogRepository {
    public bool Log (string schema,
      string connectionString,
      int logId,
      string logLevel,
      string log) {
      if (string.IsNullOrEmpty (connectionString))
        throw new ArgumentException ("The connectionString is not supplied");

      if (string.IsNullOrEmpty (schema))
        throw new ArgumentException ("The schema was not supplied.");

      if (string.IsNullOrEmpty (logLevel))
        throw new ArgumentException ("The loglevel was not supplied.");

      if (string.IsNullOrEmpty (log))
        throw new ArgumentException ("No log message was supplied");

      using (IDbConnection dbConnection = new SqlConnection (connectionString)) {
        try {
          dbConnection.Open ();
          log.Trim('\'');
          string updateQuery = $@"INSERT INTO [{schema}].[EventLog]
                               VALUES (
                                {logId.ToString()}
                              ,'{logLevel}'
                              ,'{log}'
                              ,'{DateTime.UtcNow.ToString()}')";
          var instance = dbConnection.Execute (updateQuery);
          return instance == 1;
        } catch (Exception ex) {
          throw new Exception("Insert into eventlog:", ex.InnerException);
        }
      }
    }

    public List<Models.Entities.EventLog> Logs(string schema,string connectionString)
    {
      using (IDbConnection dbConnection = new SqlConnection (connectionString)) 
      {
        dbConnection.Open();
        var sql = $"select * from [{schema}].[EventLog]";
        var data = dbConnection.Query<Models.Entities.EventLog>(sql).ToList();
        return data;
      }
    }
  }
}