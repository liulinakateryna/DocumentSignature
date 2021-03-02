using Database.KeyCenter.Entity;
using Microsoft.EntityFrameworkCore;

namespace Database.KeyCenter.Data
{
    public class DataContext : DbContext
    {
        public DbSet<PrivateData> PrivateData { get; set; }
        public DbSet<Keys> Keys { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmartContractPrivateKeys;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
