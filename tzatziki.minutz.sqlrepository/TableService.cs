using System.Data.SqlClient;
using tzatziki.minutz.interfaces;

namespace tzatziki.minutz.sqlrepository
{
  public class TableService : ITableService
  {
    private const string _res = "res";
    private const string _appSchema = "app";

    public bool Initiate(string connectionString, string schema, string table, string sql)
    {
      var exists = Validate(connectionString, schema, table);
      if (!exists)
        exists = Create(connectionString, schema, sql);
      return exists;
    }

    internal string Statement(string schema, string table)
    {
      return $@"IF EXISTS(SELECT 1
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_TYPE = 'BASE TABLE'
                AND TABLE_NAME = '{table}' AND
                TABLE_SCHEMA = '{schema}' )
                SELECT 1 AS res ELSE SELECT 0 AS res;";
    }

    internal bool Validate(string connectionString, string schema, string table)
    {
      try
      {
        var result = false;
        using (SqlConnection con = new SqlConnection(connectionString))
        {
          con.Open();
          using (SqlCommand command = new SqlCommand(Statement(schema, table), con))
          {
            using (SqlDataReader reader = command.ExecuteReader())
            {
              while (reader.Read())
              {
                result = reader[_res].ToString() == "1"? true: false;
              }
            }
          }
          con.Close();
        }
        return result;
      }
      catch (System.Exception ex)
      {
        throw new System.Exception("There was a issue checking if the table exists, [TableService.cs].", ex.InnerException);
      }
    }

    internal bool Create(string connectionString, string schema, string sqlStatement)
    {
      try
      {
        var result = false;
        using (SqlConnection con = new SqlConnection(connectionString))
        {
          using (SqlCommand command = new SqlCommand($"EXEC [{_appSchema}].[{sqlStatement}]@tenant={schema}", con))
          {
            command.ExecuteNonQuery();
            result = true;
          }
        }
        return result;
      }
      catch (System.Exception ex)
      {
        throw new System.Exception($"There was a issue creating the object , [{sqlStatement}].", ex.InnerException);
      }
    }
  }
}