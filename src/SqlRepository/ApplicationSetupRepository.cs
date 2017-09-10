using Interface.Repositories;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using Dapper;

namespace SqlRepository
{
  public class ApplicationSetupRepository : IApplicationSetupRepository
  {
    private const string SqlCatalogueKey = "#catalogue#";
    private const string SqlSchemaKey = "#schema#";
    private const string CreateApplicationCatalogueSql = "createapplicationCatalogue.sql";
    private const string CreateApplicationSchemaSql = "createapplicationSchema.sql";
    private const string CreateApplicationInstanceSql = "createapplicationInstanceTable.sql";
    private const string CreateApplicationPersonSql = "createapplicationPersonTable.sql";
    private const string CreateApplication_spCreateInstanceUserSql = "createapplication_spCreateInstanceUser.sql";

    private string CurrentLocation = "c:\foo"; //System.AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// Validate that the server that is being used exists
    /// </summary>
    /// <param name="connectionString" typeof="string">The full connectionsting of the sql database</param>
    /// <returns typeof="boolean">If connection was successfull</returns>
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
        var sql = GetCreateCatalogueScriptSqlFromFile(catalogueName);
        dbConnection.Open();
        int result = dbConnection.Execute(sql);
        if (result == -1)
          return true;
        return false;
      }
    }

    public bool CreateApplicationSchema(string connectionString, string catalogueName, string schema)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(catalogueName) || string.IsNullOrEmpty(schema))
        throw new System.ArgumentNullException("please provide a valid connectionstring, catalogue and schema");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = GetCreateSchemaScriptSqlFromFile(catalogueName, schema);
        dbConnection.Open();
        int result = dbConnection.Execute(sql);
        if (result == -1)
          return true;
        return false;
      }
    }

    public bool CreateApplicationInstance(string connectionString, string catalogueName, string schema)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(catalogueName) || string.IsNullOrEmpty(schema))
        throw new System.ArgumentNullException("please provide a valid connectionstring, catalogue and schema");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = GetCreateInstanceScriptSqlFromFile(catalogueName, schema);
        dbConnection.Open();
        int result = dbConnection.Execute(sql);
        if (result == -1)
          return true;
        return false;
      }
    }

    public bool CreateApplicationPerson(string connectionString, string catalogueName, string schema)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(catalogueName) || string.IsNullOrEmpty(schema))
        throw new System.ArgumentNullException("please provide a valid connectionstring, catalogue and schema");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = GetCreatePersonScriptSqlFromFile(catalogueName, schema);
        dbConnection.Open();
        int result = dbConnection.Execute(sql);
        if (result == -1)
          return true;
        return false;
      }
    }

    public bool CreateApplicationStoredProcedureInstanceUser(string connectionString, string catalogueName, string schema)
    {
      if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(catalogueName) || string.IsNullOrEmpty(schema))
        throw new System.ArgumentNullException("please provide a valid connectionstring, catalogue and schema");
      using (IDbConnection dbConnection = new SqlConnection(connectionString))
      {
        var sql = GetCreateStoredProcedureInstanceUserScriptSqlFromFile(catalogueName, schema);
        dbConnection.Open();
        int result = dbConnection.Execute(sql);
        if (result == -1)
          return true;
        return false;
      }
    }

    internal string GetCreateCatalogueScriptSqlFromFile(string catalogue)
    {
      var contents = File.ReadAllText($"{CurrentLocation}Scripts\\{CreateApplicationCatalogueSql}");
      return contents.Replace(SqlCatalogueKey, catalogue);
    }
    internal string GetCreateSchemaScriptSqlFromFile(string catalogue, string schema)
    {
      var contents = File.ReadAllText($"{CurrentLocation}Scripts\\{CreateApplicationSchemaSql}").Replace(SqlCatalogueKey,catalogue);
      return contents.Replace(SqlSchemaKey, schema);
    }
    internal string GetCreateInstanceScriptSqlFromFile(string catalogue, string schema)
    {
      var contents = File.ReadAllText($"{CurrentLocation}Scripts\\{CreateApplicationInstanceSql}").Replace(SqlCatalogueKey, catalogue);
      return contents.Replace(SqlSchemaKey, schema);
    }
    internal string GetCreatePersonScriptSqlFromFile(string catalogue, string schema)
    {
      var contents = File.ReadAllText($"{CurrentLocation}Scripts\\{CreateApplicationPersonSql}").Replace(SqlCatalogueKey, catalogue);
      return contents.Replace(SqlSchemaKey, schema);
    }
    internal string GetCreateStoredProcedureInstanceUserScriptSqlFromFile(string catalogue, string schema)
    {
      var contents = File.ReadAllText($"{CurrentLocation}Scripts\\{CreateApplication_spCreateInstanceUserSql}").Replace(SqlCatalogueKey, catalogue);
      return contents.Replace(SqlSchemaKey, schema);
    }
  }
}
