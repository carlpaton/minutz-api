using Interface.Repositories;

namespace SqlRepository
{
  public class ApplicationSetupRepository : IApplicationSetupRepository
  {
    public bool Exists(string connectionString)
    {
      if (connectionString == null)
        throw new System.ArgumentNullException("connection string");
      throw new System.NotImplementedException();
    }
  }
}
