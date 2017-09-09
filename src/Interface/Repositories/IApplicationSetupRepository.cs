namespace Interface.Repositories
{
  public interface IApplicationSetupRepository
  {
    bool Exists(string connectionString);

    bool CreateApplicationCatalogue(string connectionString, string catalogueName);
  }
}
