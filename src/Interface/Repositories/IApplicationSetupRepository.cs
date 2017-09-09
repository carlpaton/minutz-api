namespace Interface.Repositories
{
  public interface IApplicationSetupRepository
  {
    bool Exists(string connectionString);
  }
}
