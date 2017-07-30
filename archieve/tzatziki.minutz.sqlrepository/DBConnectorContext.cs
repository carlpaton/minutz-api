using Microsoft.EntityFrameworkCore;
using tzatziki.minutz.models.Entities;

namespace tzatziki.minutz.sqlrepository
{
  public class DBConnectorContext : DbContext
  {
    private string ConnectionString;
    private string Schema;

    public DBConnectorContext(string connectionString, string schema)
    {
      Schema = schema;

      ConnectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Person>().ToTable("Person", Schema);
      modelBuilder.Entity<Instance>().ToTable("Instance", Schema);
      modelBuilder.Entity<User>().ToTable("User", Schema);
    }

    public DbSet<Instance> Instance { get; set; }

    public DbSet<Person> Person { get; set; }

    public DbSet<User> User { get; set; }
  }
}