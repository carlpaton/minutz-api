using Interface.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace SqlRepository
{
  public class ApplicationSetupRepository : IApplicationSetupRepository
  {
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
  }
}
