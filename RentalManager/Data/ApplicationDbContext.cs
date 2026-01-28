using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Data
{
    public class ApplicationDbContext 
        : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        private readonly ICurrentUser _currentUser;
        private readonly IAccountContext _accountContext;
        public int CurrentAccountId => _accountContext.AccountId;


        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IAccountContext accountContext,
            ICurrentUser currentuser)
            : base(options)
        {
            _accountContext = accountContext;
            _currentUser = currentuser;
        }


        // ===== AUTH =====
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<SystemCode> SystemCodes { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SystemCodeItem> SystemCodeItems { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyAssignment> PropertyAssignments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UtilityBill> UnitCharges { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Landlord> Landlords { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ADD AUDITFIELDS
            ConfigureAuditFields<Account>(modelBuilder);
            ConfigureAuditFields<Property>(modelBuilder);
            ConfigureAuditFields<User>(modelBuilder);
            ConfigureAuditFields<PropertyAssignment>(modelBuilder);
            ConfigureAuditFields<Transaction>(modelBuilder);
            ConfigureAuditFields<Unit>(modelBuilder);
            ConfigureAuditFields<UnitType>(modelBuilder);
            ConfigureAuditFields<UtilityBill>(modelBuilder);
            ConfigureAuditFields<SystemCode>(modelBuilder);
            ConfigureAuditFields<SystemCodeItem>(modelBuilder);
            ConfigureAuditFields<SystemLog>(modelBuilder);
            ConfigureAuditFields<Role>(modelBuilder);
            ConfigureAuditFields<Invoice>(modelBuilder);
            ConfigureAuditFields<InvoiceLine>(modelBuilder);
            ConfigureAuditFields<Expense>(modelBuilder);

            // APPLY FILTERS
            ApplyAccountFilter<Property>(modelBuilder);
            ApplyAccountFilter<Expense>(modelBuilder);
            ApplyAccountFilter<Unit>(modelBuilder);
            ApplyAccountFilter<UnitType>(modelBuilder);
            ApplyAccountFilter<UtilityBill>(modelBuilder);
            ApplyAccountFilter<Transaction>(modelBuilder);
            ApplyAccountFilter<Tenant>(modelBuilder);



            // SEED DATA
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int>
                {
                    Id = 1,
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                },
                new IdentityRole<int>
                {
                    Id = 2,
                    Name = "Owner",
                    NormalizedName = "OWNER"
                },
                new IdentityRole<int>
                {
                    Id = 3,
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole<int>
                {
                    Id = 4,
                    Name = "Landlord",
                    NormalizedName = "LANDLORD"
                },
                new IdentityRole<int>
                {
                    Id = 5,
                    Name = "Tenant",
                    NormalizedName = "TENANT"
                }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin", IsEnabled = true },
                new Role { Id = 2, Name = "Owner", IsEnabled = true },
                new Role { Id = 3, Name = "Manager", IsEnabled = true },
                new Role { Id = 4, Name = "Admin", IsEnabled = true },
                new Role { Id = 5, Name = "Landlord", IsEnabled = true },
                new Role { Id = 6, Name = "Tenant", IsEnabled = true }
            );

            //modelBuilder.Entity<SystemCodeItem>().HasData(
            //    new SystemCodeItem { Id = 1041, SystemCodeId = 16, Item = "Maintenance", Notes = "House Maintenance" },
            //    new SystemCodeItem { Id = 1042, SystemCodeId = 16, Item = "Salary", Notes = "Staff Salary Payments" },
            //    new SystemCodeItem { Id = 1043, SystemCodeId = 16, Item = "Cleaning", Notes = "Cleaning Services" },
            //    new SystemCodeItem { Id = 1044, SystemCodeId = 16, Item = "Insurance", Notes = "Property Insurance Costs" },
            //    new SystemCodeItem { Id = 1045, SystemCodeId = 16, Item = "Legal", Notes = "Legal Fees and Services" },
            //    new SystemCodeItem { Id = 1046, SystemCodeId = 16, Item = "Marketing", Notes = "Advertising and Promotion Costs" },
            //    new SystemCodeItem { Id = 1047, SystemCodeId = 16, Item = "Office Supplies", Notes = "Office and Administrative Supplies" },
            //    new SystemCodeItem { Id = 1048, SystemCodeId = 16, Item = "Security", Notes = "Security and Surveillance Expenses" },
            //    new SystemCodeItem { Id = 1049, SystemCodeId = 16, Item = "Other", Notes = "Other expenses not classified elsewhere" }
            //);




            // REFRESHTOKEN
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Token).IsUnique();
                entity.Property(x => x.ExpiresOn).IsRequired();
                entity.Property(x => x.CreatedOn).IsRequired();
                entity.Property(x => x.CreatedByIp).IsRequired();
                entity.Property(x => x.Revoked).IsRequired().HasDefaultValue(false);
                entity.Property(x => x.RevokedOn);
                entity.Property(x => x.RevokedByIp);
                entity.Property(x => x.ReplacedByToken);
                entity.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });


            // SYSTEMCODE
            modelBuilder.Entity<SystemCode>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Code).HasMaxLength(20).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);
            });



            // SYSTEMCODEITEM
            modelBuilder.Entity<SystemCodeItem>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Item).HasMaxLength(20).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.HasOne(u => u.SystemCode).WithMany(sc => sc.SystemCodeItems).HasForeignKey(u => u.SystemCodeId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });


            // ACCOUNT
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(100).IsRequired();
                entity.Property(u => u.IsActive).HasDefaultValue(true);
                entity.Property(u => u.IsTrial).HasDefaultValue(true);
                entity.Property(u => u.TrialEndsAt);
                entity.Property(u => u.SubscriptionEndsAt);
            });


            // PROPERTY 
            modelBuilder.Entity<Property>(entity =>
            {

                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Country).HasMaxLength(50).IsRequired();
                entity.Property(u => u.County).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Area).HasMaxLength(50).IsRequired();
                entity.Property(u => u.PhysicalAddress).HasMaxLength(50);
                entity.Property(u => u.Longitude).HasColumnType("decimal(9,6)");
                entity.Property(u => u.Latitude).HasColumnType("decimal(9,6)");
                entity.Property(u => u.Floor).IsRequired();
                entity.Property(u => u.EmailAddress).HasMaxLength(50).IsRequired();
                entity.Property(u => u.MobileNumber).HasMaxLength(15).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);

                // Relationships
                entity.HasOne(u => u.Account).WithMany(a => a.Properties).HasForeignKey(u => u.AccountId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });



            // USER 
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FirstName).HasMaxLength(20).IsRequired();
                entity.Property(u => u.LastName).HasMaxLength(20).IsRequired();
                entity.Property(u => u.EmailAddress).HasMaxLength(50);
                entity.Property(u => u.MobileNumber).HasMaxLength(15).IsRequired();
                entity.Property(u => u.AlternativeNumber).HasMaxLength(15);
                entity.Property(u => u.NationalId).HasMaxLength(10);
                entity.Property(u => u.ProfilePhotoUrl);
                entity.Property(u => u.IsActive).HasDefaultValue(false);

                // Relationship
                entity.HasOne(u => u.ApplicationUser).WithOne(a => a.User).HasForeignKey<User>(u => u.ApplicationUserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(u => u.Gender).WithMany().HasForeignKey(u => u.GenderId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.UserStatus).WithMany().HasForeignKey(u => u.UserStatusId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });


            // PROPERTY ASSIGNMENTS
            modelBuilder.Entity<PropertyAssignment>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.User)
                      .WithMany()
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Property)
                      .WithMany()
                      .HasForeignKey(x => x.PropertyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new { x.UserId, x.PropertyId }).IsUnique();
            });




            // USERLOGIN
            modelBuilder.Entity<UserLogin>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FailedAttempts).HasDefaultValue(0).IsRequired();
                entity.Property(u => u.Succeeded).IsRequired();
                entity.Property(u => u.IpAddress).HasMaxLength(20);
                entity.Property(u => u.UserToken).HasMaxLength(20);
                entity.Property(u => u.TokenExpiry);
                entity.Property(u => u.IsActive).HasDefaultValue(true);
                entity.Property(u => u.CreatedOn);
                entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });



            // TENANT
            modelBuilder.Entity<Tenant>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FullName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.EmailAddress).HasMaxLength(50);
                entity.Property(u => u.MobileNumber).HasMaxLength(15).IsRequired();
                entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Unit).WithMany().HasForeignKey(u => u.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.TenantStatus).WithMany().HasForeignKey(u => u.Status).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });



            // LANDLORD
            modelBuilder.Entity<Landlord>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FullName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.EmailAddress).HasMaxLength(50);
                entity.Property(u => u.MobileNumber).HasMaxLength(15).IsRequired();
                entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Restrict);
            });




            // TRANSACTION
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Amount).HasColumnType("decimal(18, 2)").IsRequired();
                entity.Property(u => u.TransactionDate).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.Property(u => u.MonthFor).HasMaxLength(10);
                entity.Property(u => u.YearFor).HasMaxLength(4);
                entity.Property(u => u.Combine).HasDefaultValue(true).IsRequired();
                entity.Property(u => u.Status).HasMaxLength(100);

                
                // Relationships
                entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Unit).WithMany().HasForeignKey(u => u.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.UtilityBill).WithMany().HasForeignKey(u => u.UtilityBillId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(u => u.PaymentMethod).WithMany().HasForeignKey(u => u.PaymentMethodId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(u => u.TransactionType).WithMany().HasForeignKey(u => u.TransactionTypeId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.TransactionCategory).WithMany().HasForeignKey(u => u.TransactionCategoryId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.ExpenseCategory).WithMany().HasForeignKey(u => u.ExpenseCategoryId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Expenses).WithMany().HasForeignKey(u => u.ExpenseId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Account).WithMany().HasForeignKey(u => u.AccountId).OnDelete(DeleteBehavior.Restrict);
            });
           





            //EXPENSE
            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Amount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);

                // Relationships
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Account).WithMany().HasForeignKey(u => u.AccountId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });




            // UNIT
            modelBuilder.Entity<Unit>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Amount).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.HasOne(u => u.Status).WithMany().HasForeignKey(u => u.StatusId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.UnitType).WithMany().HasForeignKey(u => u.UnitTypeId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Property).WithMany(u => u.Units).HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });



            // UNITTYPE
            modelBuilder.Entity<UnitType>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Account).WithMany().HasForeignKey(u => u.AccountId).OnDelete(DeleteBehavior.Cascade);
            });



            // UTILITYBILL
            modelBuilder.Entity<UtilityBill>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
                entity.Property(u => u.isReccuring).HasDefaultValue(true);
                entity.Property(u => u.Amount).HasColumnType("decimal(18,4)").IsRequired();

                // Relationships
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(u => u.Account).WithMany().HasForeignKey(u => u.AccountId).OnDelete(DeleteBehavior.Cascade);
            });



            // ROLE
            modelBuilder.Entity<Role>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(20).IsRequired();
                entity.Property(u => u.IsEnabled).HasDefaultValue(true);
            });



            // SYSTEMLOG
            modelBuilder.Entity<SystemLog>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Action).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.Property(u => u.IpAddress).HasMaxLength(20);
                entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.LogLevelStatus).WithMany().HasForeignKey(u => u.LogLevel).OnDelete(DeleteBehavior.Restrict);
            });



        }

        //public override async Task<int> SaveChangesAsync(
        //     CancellationToken cancellationToken = default)
        //{
        //    var now = DateTime.UtcNow;
        //    var userId = _currentUser?.UserId;

        //    foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            entry.Entity.CreatedOn = now;
        //            entry.Entity.CreatedBy = userId > 0 ? userId : null;
        //        }

        //        if (entry.State == EntityState.Modified)
        //        {
        //            entry.Entity.UpdatedOn = now;
        //            entry.Entity.UpdatedBy = userId > 0 ? userId : null;
        //        }
        //    }

        //    return await base.SaveChangesAsync(cancellationToken);
        //}



        private void ApplyAccountFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : class, IAccountContext
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(e => EF.Property<int>(e, "AccountId") == CurrentAccountId);
        }



        private void ConfigureAuditFields<T>(ModelBuilder modelBuilder) where T : class
        {
            modelBuilder.Entity<T>()
                .HasOne(typeof(User), "CreatedByUser")
                .WithMany()
                .HasForeignKey("CreatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<T>()
                .HasOne(typeof(User), "UpdatedByUser")
                .WithMany()
                .HasForeignKey("UpdatedBy")
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
