using CreditApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditApplication.Data
{
    public class CreditApplicationDbContext : DbContext
    {
        public CreditApplicationDbContext(DbContextOptions<CreditApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Nomenclature> Nomenclatures { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientFinancial> ClientFinancials { get; set; }
        public DbSet<ClientAddress> ClientAddresses { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<RepaymentPlan> RepaymentPlans { get; set; }
        public DbSet<FinancialOperation> FinancialOperations { get; set; }
        public DbSet<LogTable> LogTables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Може да зададете конфигурации, ако не използвате Data Annotations

            // Пример: за таблицата Nomenclature – за първичния ключ
            modelBuilder.Entity<Nomenclature>()
                .HasKey(n => n.NomCode);

            // Ако желаете да зададете default стойности и други constraints,
            // това може да стане тук или чрез Data Annotations във моделите.
        }
    }
}
