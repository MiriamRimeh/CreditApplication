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

            modelBuilder.Entity<Client>()
                 .ToTable("Clients", tb => tb.HasTrigger("trg_21180011_Clients_Log"))
                 .HasMany(c => c.Credits)
                 .WithOne(c => c.Client)
                 .HasForeignKey(c => c.ClientID);

            modelBuilder.Entity<ClientFinancial>()
                .ToTable("ClientFinancials", tb => tb.HasTrigger("trg_21180011_ClientFinancials_Log"));

            modelBuilder.Entity<ClientAddress>()
                 .ToTable("ClientAddress", tb => tb.HasTrigger("trg_21180011_ClientAddress_Log"));

            modelBuilder.Entity<Credit>(entity =>
            {
                entity.ToTable("Credits", tb =>
                {
                    tb.HasTrigger("trg_21180011_Credits_Log");
                    tb.HasTrigger("trg_Credit_Activate");
                });
                entity.HasMany(c => c.FinancialOperations)
                      .WithOne(f => f.Credit)
                      .HasForeignKey(f => f.CreditID);
            });

            modelBuilder.Entity<RepaymentPlan>(entity =>
            {
                entity.ToTable("RepaymentPlan", tb => {
                    tb.HasTrigger("trg_21180011_RepaymentPlan_Log");
                    tb.HasTrigger("trg_RepaymentPlan_OnPayedOnDateUpdate");
                });
                entity.HasOne(r => r.Credit);
            });

            modelBuilder.Entity<FinancialOperation>()
                            .ToTable("FinancialOperations", tb => tb.HasTrigger("trg_21180011_FinancialOperations_Log"))
                            .HasOne(f => f.Credit)
                            .WithMany(c => c.FinancialOperations)
                            .HasForeignKey(f => f.CreditID);

            modelBuilder.Entity<Nomenclature>()
               .ToTable("Nomenclature", tb => tb.HasTrigger("trg_21180011_Nomenclature_Log"));

            modelBuilder.Entity<LogTable>().ToTable("LogTable");

            modelBuilder.Entity<Account>()
                .ToTable("Accounts", tb => tb.HasTrigger("trg_21180011_Accounts_Log"));

        }
    }
}
