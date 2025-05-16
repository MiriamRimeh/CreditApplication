using CreditApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditApplication.Data
{
    public class CreditApplicationDbContext : DbContext
    {
        public CreditApplicationDbContext(DbContextOptions<CreditApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        // TODO: Check DB Properties, some display invalid object error, others don't open at all (when the app is started)
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientFinancial> ClientFinancials { get; set; }
        public DbSet<ClientAddress> ClientAddresses { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<RepaymentPlan> RepaymentPlans { get; set; }
        public DbSet<FinancialOperation> FinancialOperations { get; set; }
        public DbSet<LogTable> LogTables { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("21180011");

            modelBuilder.Entity<Client>().ToTable("Clients")
                    .HasMany(c => c.Credits);
            modelBuilder.Entity<ClientFinancial>().ToTable("ClientFinancials");
            modelBuilder.Entity<ClientAddress>().ToTable("ClientAddress");
            modelBuilder.Entity<Credit>().ToTable("Credits")
                    .HasMany(c => c.FinancialOperations);
            modelBuilder.Entity<RepaymentPlan>().ToTable("RepaymentPlan");
            modelBuilder.Entity<FinancialOperation>().ToTable("FinancialOperations");
            modelBuilder.Entity<Nomenclature>().ToTable("Nomenclature");
            modelBuilder.Entity<LogTable>().ToTable("LogTable");
            modelBuilder.Entity<Account>().ToTable("Accounts");

        }
    }
}
