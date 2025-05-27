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

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(c => c.CreatedOn)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(c => c.ModifiedOn)
                      .HasColumnName("ModifiedOn_21180011");

                entity.ToTable("Clients", tb => tb.HasTrigger("trg_21180011_Clients_Log"))
                      .HasMany(c => c.Credits)
                      .WithOne(c => c.Client)
                      .HasForeignKey(c => c.ClientID);

                entity.HasMany(c => c.ClientFinancials)
                     .WithOne(cf => cf.Client)
                     .HasForeignKey(cf => cf.ClientID);

                entity.HasMany(c => c.ClientAddresses)
                      .WithOne(ca => ca.Client)
                      .HasForeignKey(ca => ca.ClientID);

                entity.HasMany(c => c.Accounts)
                      .WithOne(a => a.Client)
                      .HasForeignKey(a => a.ClientID);
            });


            modelBuilder.Entity<ClientFinancial>(entity =>
            {

                entity.Property(c => c.CreatedOn)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(c => c.ModifiedOn)
                      .HasColumnName("ModifiedOn_21180011");

                entity.ToTable("ClientFinancials", tb => tb.HasTrigger("trg_21180011_ClientFinancials_Log"))
                      .HasOne(c => c.Client)
                      .WithMany(c => c.ClientFinancials)
                      .HasForeignKey(c => c.ClientID);

                entity.HasOne(c => c.EmploymentTypeNomenclature)
                      .WithMany(n => n.ClientFinancials)
                      .HasForeignKey(c => c.EmploymentType);
            });


            modelBuilder.Entity<ClientAddress>(entity =>
            {
                entity.Property(c => c.CreatedOn)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(c => c.ModifiedOn)
                      .HasColumnName("ModifiedOn_21180011");


                entity.ToTable("ClientAddress", tb => tb.HasTrigger("trg_21180011_ClientAddress_Log"))
                      .HasOne(c => c.Client)
                      .WithMany(c => c.ClientAddresses)
                      .HasForeignKey(c => c.ClientID);

            });

            modelBuilder.Entity<Credit>(entity =>
            {
                entity.Property(c => c.CreatedOn)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(c => c.ModifiedOn)
                      .HasColumnName("ModifiedOn_21180011");

                entity.ToTable("Credits", tb =>
                {
                    tb.HasTrigger("trg_21180011_Credits_Log");
                    tb.HasTrigger("trg_Credit_Activate");
                });
                entity.HasOne(c => c.Client)
                      .WithMany(c => c.Credits)
                      .HasForeignKey(c => c.ClientID);

                entity.HasOne(c => c.StatusNavigation)
                        .WithMany(n => n.CreditsByStatus)
                        .HasForeignKey(c => c.Status);

                entity.HasMany(c => c.FinancialOperations)
                      .WithOne(f => f.Credit)
                      .HasForeignKey(f => f.CreditID);

                entity.HasMany(c => c.RepaymentPlans)
                      .WithOne(rp => rp.Credit)
                      .HasForeignKey(rp => rp.CreditID);
            });

            modelBuilder.Entity<RepaymentPlan>(entity =>
            {
                entity.Property(c => c.CreatedOn)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(c => c.ModifiedOn)
                      .HasColumnName("ModifiedOn_21180011");

                entity.ToTable("RepaymentPlan", tb => 
                {
                    tb.HasTrigger("trg_21180011_RepaymentPlan_Log");
                    tb.HasTrigger("trg_RepaymentPlan_OnPayedOnDateUpdate");
                });

                entity.HasOne(r => r.Credit)
                      .WithMany(c => c.RepaymentPlans)
                      .HasForeignKey(r => r.CreditID);
            });

            modelBuilder.Entity<FinancialOperation>(entity =>
            {
                entity.Property(c => c.CreatedOn)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(c => c.ModifiedOn)
                      .HasColumnName("ModifiedOn_21180011");

                entity.ToTable("FinancialOperations", tb => tb.HasTrigger("trg_21180011_FinancialOperations_Log"));

                 entity.HasOne(f => f.Credit)
                      .WithMany(c => c.FinancialOperations)
                      .HasForeignKey(f => f.CreditID);

                entity.HasOne(f => f.OperationTypeNomenclature)
                      .WithMany(n => n.FinancialOperationsByType)
                      .HasForeignKey(f => f.OperationType);

                entity.HasOne(f => f.RepaymentPlan)
                      .WithMany()
                      .HasForeignKey(f => f.RepaymentPlanID);
            });

            modelBuilder.Entity<Nomenclature>(entity =>
            {
                entity.Property(c => c.CreatedOn)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(c => c.ModifiedOn)
                      .HasColumnName("ModifiedOn_21180011");

                entity.ToTable("Nomenclature", tb => tb.HasTrigger("trg_21180011_Nomenclature_Log"));
            });

            modelBuilder.Entity<LogTable>().ToTable("LogTable");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(c => c.CreatedAt)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(c => c.ModifiedOn21180011)
                      .HasColumnName("ModifiedOn_21180011");

                entity.ToTable("Accounts", tb => tb.HasTrigger("trg_21180011_Accounts_Log"));

                entity.HasOne(a => a.Client)
                      .WithMany(c => c.Accounts)
                      .HasForeignKey(a => a.ClientID);

            });

        }
    }
}
