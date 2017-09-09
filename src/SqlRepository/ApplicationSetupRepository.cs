using Interface.Repositories;
using System.Data.SqlClient;
using Interface.Services;
using System.Data;
using System.IO;
using Dapper;

namespace SqlRepository
{
  public class ApplicationSetupRepository : IApplicationSetupRepository
  {
    private const string SqlCatalogueKey = "#catalogue#";
    private const string CreateApplicationCatalogueSql = "createapplicationCatalogue.sql";

    public bool Exists(string connectionString)
    {
      if (string.IsNullOrEmpty(connectionString))
        throw new System.ArgumentNullException("connection string is not provided.");
      try
      {
        using (IDbConnection dbConnection = new SqlConnection(connectionString))
        {
          dbConnection.Open();
          return true;
        }
      }
      catch (SqlException)
      {
        return false;
      }
    }

    /// <summary>
    /// This method creates that main catalogue that is used for the application to function
    /// </summary>
    /// <param name="connectionString"  typeof="string">tcp: sql connection string</param>
    /// <param name="catalogueName"  typeof="string">The database name to be used.</param>
    /// <returns></returns>
    public bool CreateApplicationCatalogue(string connectionString, string catalogueName)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(catalogueName))
        throw new System.ArgumentNullException("conneciton string or catalogue needs to be provided.");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = GetScriptSqlFromFile(CreateApplicationCatalogueSql, SqlCatalogueKey, catalogueName);
        dbConnection.Open();
        int result = dbConnection.Execute(sql);
        if (result == -1)
          return true;
        return false;
      }
    }

    /// <summary>
    /// This method manages the call to the file system and replaces the SQL special characters with the value passed.
    /// </summary>
    /// <param name="scriptName" typeof="string">Sql file name with file extension</param>
    /// <param name="token" typeof="string">The special character value.</param>
    /// <param name="value" typeof="string">The value to replace with.</param>
    /// <returns typeof="string">The Sql with replaced values</returns>
    internal string GetScriptSqlFromFile(string scriptName, string token, string value)
    {
      string Location = System.AppDomain.CurrentDomain.BaseDirectory;
      var contents = File.ReadAllText($"{Location}Scripts\\{scriptName}");
      return contents.Replace(token, value);
    }
  }
}
