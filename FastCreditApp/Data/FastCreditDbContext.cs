using System;
using System.Collections.Generic;
using FastCreditApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastCreditApp.Data;

public partial class FastCreditDbContext : DbContext
{
    public FastCreditDbContext()
    {
    }

    public FastCreditDbContext(DbContextOptions<FastCreditDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientAddress> ClientAddresses { get; set; }

    public virtual DbSet<ClientFinancial> ClientFinancials { get; set; }

    public virtual DbSet<Credit> Credits { get; set; }

    public virtual DbSet<FinancialOperation> FinancialOperations { get; set; }

    public virtual DbSet<Log21180011> Log21180011 { get; set; }

    public virtual DbSet<Nomenclature> Nomenclature { get; set; }

    public virtual DbSet<RepaymentPlan> RepaymentPlans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-GOFRJLL\\MSSQLSERVER01;Database=CreditApplication;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Clients__3214EC27FEE31CD4");

            entity.ToTable("Clients", "21180011");

            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EGN)
                .HasMaxLength(10)
                .HasColumnName("EGN");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IDCardNumber)
                .HasMaxLength(20)
                .HasColumnName("IDCardNumber");
            entity.Property(e => e.IDIssueDate).HasColumnName("IDIssueDate");
            entity.Property(e => e.IDIssuer)
                .HasMaxLength(50)
                .HasColumnName("IDIssuer");
            entity.Property(e => e.IDValidityDate).HasColumnName("IDValidityDate");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(10);
        });

        modelBuilder.Entity<ClientAddress>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__ClientAd__3214EC275BF953C2");

            entity.ToTable("ClientAddress", "21180011");

            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.ClientID).HasColumnName("ClientID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Number).HasMaxLength(5);
            entity.Property(e => e.PostCode).HasMaxLength(4);
            entity.Property(e => e.StreetNeighbourhood).HasMaxLength(50);

            entity.HasOne(d => d.Client).WithMany(p => p.ClientAddresses)
                .HasForeignKey(d => d.ClientID)
                .HasConstraintName("FK__ClientAdd__Clien__693CA210");
        });

        modelBuilder.Entity<ClientFinancial>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__ClientFi__3214EC27D7A33086");

            entity.ToTable("ClientFinancials", "21180011");

            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.ClientID).HasColumnName("ClientID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontlyExpenses).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MontlyIncome).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientFinancials)
                .HasForeignKey(d => d.ClientID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientFin__Clien__534D60F1");

            entity.HasOne(d => d.EmploymentTypeNavigation).WithMany(p => p.ClientFinancials)
                .HasForeignKey(d => d.EmploymentType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClientFin__Emplo__5441852A");
        });

        modelBuilder.Entity<Credit>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Credits__3214EC27B9C58519");

            entity.ToTable("Credits", "21180011");

            entity.Property(e => e.ID).HasColumnName("ID");
            entity.Property(e => e.ClientID).HasColumnName("ClientID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreditAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Credits)
                .HasForeignKey(d => d.ClientID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Credits__ClientI__59063A47");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Credits)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("FK__Credits__Status__59FA5E80");
        });

        modelBuilder.Entity<FinancialOperation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Financia__3214EC27CF6F2DE4");

            entity.ToTable("FinancialOperations", "21180011");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreditId).HasColumnName("CreditID");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PayedAmount).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Credit).WithMany(p => p.FinancialOperations)
                .HasForeignKey(d => d.CreditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Financial__Credi__6383C8BA");

            entity.HasOne(d => d.OperationTypeNavigation).WithMany(p => p.FinancialOperations)
                .HasForeignKey(d => d.OperationType)
                .HasConstraintName("FK__Financial__Opera__6477ECF3");
        });

        modelBuilder.Entity<Log21180011>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__log_2118__3214EC27E7F82D89");

            entity.ToTable("log_21180011", "21180011");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActionDate).HasColumnType("datetime");
            entity.Property(e => e.ActionType).HasMaxLength(20);
            entity.Property(e => e.TableName).HasMaxLength(50);
        });

        modelBuilder.Entity<Nomenclature>(entity =>
        {
            entity.HasKey(e => e.NomCode).HasName("PK__Nomencla__B5581B47805B53AA");

            entity.ToTable("Nomenclature", "21180011");

            entity.Property(e => e.NomCode).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<RepaymentPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Repaymen__3214EC2700DF3CFF");

            entity.ToTable("RepaymentPlan", "21180011");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreditId).HasColumnName("CreditID");
            entity.Property(e => e.InstallmentAmount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Interest).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IsPaid).HasColumnName("isPaid");
            entity.Property(e => e.ModifiedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Principal).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Credit).WithMany(p => p.RepaymentPlans)
                .HasForeignKey(d => d.CreditId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Repayment__Credi__5EBF139D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
