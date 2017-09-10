namespace Interface.Repositories
{
  public interface IApplicationSetupRepository
  {
    bool Exists(string connectionString);

    bool CreateApplicationCatalogue(string connectionString, string catalogueName);

    bool CreateApplicationSchema(string connectionString, string catalogueName, string schema);

    bool CreateApplicationInstance(string connectionString, string catalogueName, string schema);

    bool CreateApplicationPerson(string connectionString, string catalogueName, string schema);
  }
}
