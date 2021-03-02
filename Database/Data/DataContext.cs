using Database.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<User> SystemUsers { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmartContract;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contract>()
                .HasOne(a => a.User)
                .WithMany(u => u.Contracts)
                .HasForeignKey(a => a.Id);

            modelBuilder.Entity<Contract>()
                 .HasOne(a => a.Document)
                 .WithMany(u => u.Contracts)
                 .HasForeignKey(a => a.DocumentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
