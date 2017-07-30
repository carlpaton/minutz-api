namespace tzatziki.minutz.interfaces
{
  public interface ITableService
  {
    bool Initiate(string connectionString, string schema, string table, string sql);
  }
}