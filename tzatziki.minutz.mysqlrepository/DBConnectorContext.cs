using Microsoft.EntityFrameworkCore;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.mysqlrepository
{
  public class DBConnectorContext : DbContext
  {
    private string ConnectionString;
    public DBConnectorContext(string connectionString)
    {
      ConnectionString = connectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseMySql(ConnectionString);

    public DbSet<Instance> instance { get; set; }

    public DbSet<Person> person { get; set; }
  }
}
