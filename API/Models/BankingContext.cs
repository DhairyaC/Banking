using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OnlineBankingAPI.Models
{
    public partial class BankingContext : DbContext
    {
        public BankingContext()
        {
        }

        public BankingContext(DbContextOptions<BankingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountDetail> AccountDetails { get; set; }
        public virtual DbSet<Approval> Approvals { get; set; }
        public virtual DbSet<BankAdmin> BankAdmins { get; set; }
        public virtual DbSet<Beneficiary> Beneficiaries { get; set; }
        public virtual DbSet<CustAddress> CustAddresses { get; set; }
        public virtual DbSet<CustomerDetail> CustomerDetails { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-Q9MV2UV;Database=Banking;Trusted_Connection=True;");
            }
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AccountDetail>(entity =>
            {
                entity.HasKey(e => e.AccountNumber)
                    .HasName("PK__AccountD__AF91A6ACC2A58D7B");

                entity.Property(e => e.AccountNumber).HasColumnName("account_number");

                entity.Property(e => e.AccountStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("account_status");

                entity.Property(e => e.AccountType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("account_type");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.CurrentBalance)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("current_balance");

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("last_login");

                entity.Property(e => e.LoginPassword)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("login_password");

                entity.Property(e => e.TransactionPassword)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("transaction_password");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.AccountDetails)
                    .HasForeignKey(d => d.CustId)
                    .HasConstraintName("FK__AccountDe__cust___48CFD27E");
            });

            modelBuilder.Entity<Approval>(entity =>
            {
                entity.ToTable("Approval");

                /*entity.HasIndex(e => e.CustId, "UQ__Approval__A1B71F91654FB260")
                    .IsUnique();

                entity.HasIndex(e => e.Srn, "UQ__Approval__DDDF02C783F29C00")
                    .IsUnique();*/

                entity.Property(e => e.ApprovalId).HasColumnName("approval_id");

                entity.Property(e => e.AllotedTo).HasColumnName("alloted_to");

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.Srn).HasColumnName("srn");

                entity.HasOne(d => d.AllotedToNavigation)
                    .WithMany(p => p.Approvals)
                    .HasForeignKey(d => d.AllotedTo)
                    .HasConstraintName("FK__Approval__allote__45F365D3");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.Approvals)
                    .HasForeignKey(d => d.CustId)
                    .HasConstraintName("FK__Approval__cust_i__44FF419A");
            });

            modelBuilder.Entity<BankAdmin>(entity =>
            {
                entity.HasKey(e => e.AdminId)
                    .HasName("PK__BankAdmi__43AA4141DB083859");

                entity.ToTable("BankAdmin");

                entity.Property(e => e.AdminId)
                    .ValueGeneratedNever()
                    .HasColumnName("admin_id");

                entity.Property(e => e.AdminPassword)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("admin_password");
            });

            modelBuilder.Entity<Beneficiary>(entity =>
            {
                /*entity.HasIndex(e => e.BAccountNumber, "UQ__Benefici__4C3A9F7BBC38BF00")
                    .IsUnique();

                entity.HasIndex(e => e.AccountNumber, "UQ__Benefici__AF91A6AD5C6B438F")
                    .IsUnique();*/

                entity.Property(e => e.BeneficiaryId).HasColumnName("beneficiary_id");

                entity.Property(e => e.AccountNumber).HasColumnName("account_number");

                entity.Property(e => e.BAccountNumber).HasColumnName("b_account_number");

                entity.Property(e => e.Nickname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("nickname");

                /*
                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithOne(p => p.BeneficiaryAccountNumberNavigation)
                    .HasForeignKey<Beneficiary>(d => d.AccountNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Beneficia__accou__4E88ABD4");

                entity.HasOne(d => d.BAccountNumberNavigation)
                    .WithOne(p => p.BeneficiaryBAccountNumberNavigation)
                    .HasForeignKey<Beneficiary>(d => d.BAccountNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Beneficia__b_acc__4F7CD00D");
                */
            });

            modelBuilder.Entity<CustAddress>(entity =>
            {
                entity.HasKey(e => new { e.CustId, e.TypeOfAddress })
                    .HasName("PK__CustAddr__E70E69B687D1A361");

                entity.ToTable("CustAddress");

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.TypeOfAddress)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("type_of_address");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.CustState)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cust_state");

                entity.Property(e => e.Landmark)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("landmark");

                entity.Property(e => e.Line1)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("line1");

                entity.Property(e => e.Line2)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("line2");

                entity.Property(e => e.PinCode).HasColumnName("pin_code");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.CustAddresses)
                    .HasForeignKey(d => d.CustId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CustAddre__cust___3E52440B");
            });

            modelBuilder.Entity<CustomerDetail>(entity =>
            {
                entity.HasKey(e => e.CustId)
                    .HasName("PK__Customer__A1B71F903F08F874");

                entity.HasIndex(e => e.Aadhar, "UQ__Customer__059DEB9EECABC443")
                    .IsUnique();

                entity.HasIndex(e => e.MobileNumber, "UQ__Customer__30462B0F165C4016")
                    .IsUnique();

                entity.HasIndex(e => e.PanCard, "UQ__Customer__459D36142A875ECA")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Customer__AB6E6164F0B94127")
                    .IsUnique();

                entity.Property(e => e.CustId).HasColumnName("cust_id");

                entity.Property(e => e.Aadhar).HasColumnName("aadhar");

                entity.Property(e => e.ApprovalStatus)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("approval_status");

                entity.Property(e => e.DebitCard).HasColumnName("debit_card");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FathersName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("fathers_name");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.Gender)
                    //.IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.GrossAnnualIncome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("gross_annual_income");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("middle_name");

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("mobile_number");

                entity.Property(e => e.NetBanking).HasColumnName("net_banking");

                entity.Property(e => e.OccupationType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("occupation_type");

                entity.Property(e => e.PanCard)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("pan_card");

                entity.Property(e => e.PanDoc)
                    .IsUnicode(false)
                    .HasColumnName("pan_doc");

                entity.Property(e => e.SourceOfIncome)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("source_of_income");

                entity.Property(e => e.Title)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.ReferenceId)
                    .HasName("PK__Transact__8E860B28DC32E50B");

                entity.Property(e => e.ReferenceId).HasColumnName("reference_id");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("amount");

                entity.Property(e => e.FromAccNum).HasColumnName("from_acc_num");

                entity.Property(e => e.Mode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mode");

                entity.Property(e => e.PaidToAccNum).HasColumnName("paid_to_acc_num");

                entity.Property(e => e.Remark)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("remark");

                entity.Property(e => e.TranStatus)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("tran_status");

                entity.Property(e => e.TransTime)
                    .HasColumnType("datetime")
                    .HasColumnName("trans_time")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.FromAccNumNavigation)
                    .WithMany(p => p.TransactionFromAccNumNavigations)
                    .HasForeignKey(d => d.FromAccNum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transacti__from___534D60F1");

                entity.HasOne(d => d.PaidToAccNumNavigation)
                    .WithMany(p => p.TransactionPaidToAccNumNavigations)
                    .HasForeignKey(d => d.PaidToAccNum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transacti__paid___52593CB8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
