using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentalManager.Models;
using RentalManager.Notification.Models;
using RentalManager.Services.AccountAccessService;
using System.Linq.Expressions;
using System.Reflection;

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
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitFeature> UnitFeatures { get; set; }
        public DbSet<UnitFeatureAssignment> UnitFeatureAssignments { get; set; }
        public DbSet<UtilityBill> UtilityBills { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionAllocation> TransactionAllocations { get; set; }
        public DbSet<TransactionRelation> TransactionRelations { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<NotificationPreference> NotificationPreferences { get; set; }
        public DbSet<InAppNotification> InAppNotifications { get; set; }



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
            ConfigureAuditFields<UnitFeature>(modelBuilder);
            ConfigureAuditFields<UnitFeatureAssignment>(modelBuilder);
            ConfigureAuditFields<UnitType>(modelBuilder);
            ConfigureAuditFields<UtilityBill>(modelBuilder);
            ConfigureAuditFields<SystemCode>(modelBuilder);
            ConfigureAuditFields<SystemCodeItem>(modelBuilder);
            ConfigureAuditFields<SystemLog>(modelBuilder);
            ConfigureAuditFields<Role>(modelBuilder);
            ConfigureAuditFields<Expense>(modelBuilder);

            // APPLY FILTERS
            ApplyGlobalFilters(modelBuilder);



            modelBuilder.Entity<ApplicationUser>()
                .Property(x => x.AccountId)
                .IsRequired();


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

            //USERROLE
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => new { x.UserId, x.RoleId, x.AccountId, x.PropertyId }).IsUnique();
                entity.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
            });


            // PERMISSIONS
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name).IsUnique();
                entity.Property(x => x.Name).IsRequired().HasMaxLength(150);
                entity.Property(x => x.Category).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => new { x.RoleId, x.PermissionId }).IsUnique();
                entity.Property(x => x.IsAllowed).IsRequired();
                entity.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Permission).WithMany().HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Cascade);
            });


            // SYSTEMCODE
            modelBuilder.Entity<SystemCode>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Code).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Code).IsUnicode(false);
            });



            // SYSTEMCODEITEM
            modelBuilder.Entity<SystemCodeItem>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Item).HasMaxLength(50).IsRequired();
                entity.Property(u => u.DisplayName).HasMaxLength(50).IsRequired();
                entity.Property(u => u.IconKey).HasMaxLength(50);
                entity.Property(u => u.GroupKey).HasMaxLength(50);
                entity.Property(u => u.Color).HasMaxLength(50); 
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.HasOne(u => u.SystemCode).WithMany(sc => sc.SystemCodeItems).HasForeignKey(u => u.SystemCodeId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(e => new { e.SystemCodeId, e.Item }).IsUnique();
            });


            // NOTIFICATION PREFERENCES
            modelBuilder.Entity<NotificationPreference>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.EventType).HasMaxLength(100).IsRequired();
                entity.Property(u => u.InAppEnabled).HasDefaultValue(true);
                entity.Property(u => u.SmsEnabled).HasDefaultValue(false);
                entity.Property(u => u.EmailEnabled).HasDefaultValue(false);
                entity.Property(u => u.IsEnabled).HasDefaultValue(false);
                entity.HasIndex(e => new
                {
                    e.AccountId,
                    e.EventType,
                    e.UserId
                });
            });


            // INAPPNOTIFICATIONS
            modelBuilder.Entity<InAppNotification>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Body)
                    .IsRequired();

                entity.Property(e => e.IsRead)
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => new { e.UserId, e.IsRead });
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
                entity.HasOne(u => u.PropertyType).WithMany().HasForeignKey(u => u.PropertyTypeId).IsRequired().OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.Name }).IsUnique();
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
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Account).WithMany().HasForeignKey(u => u.AccountId).IsRequired().OnDelete(DeleteBehavior.Restrict);
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
                entity.HasOne(u => u.Account).WithMany().HasForeignKey(u => u.AccountId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });



            // TENANT
            modelBuilder.Entity<Tenant>(entity => {
                entity.HasKey(u => u.Id);
                entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Unit).WithMany().HasForeignKey(u => u.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.TenantStatus).WithMany().HasForeignKey(u => u.TenantStatusId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });



            //LEASE
            modelBuilder.Entity<Lease>(entity =>
            {
                entity.HasKey(x => x.Id);

                // Amount
                entity.Property(x => x.RentAmount).HasColumnType("decimal(18,2)").IsRequired();

                // Dates
                entity.Property(x => x.StartDate).IsRequired();
                entity.Property(x => x.NextBillingDate).IsRequired();
                entity.Property(x => x.EndDate).IsRequired(false);
                entity.Property(x => x.RequiresDeposit).IsRequired();
                entity.Property(x => x.DepositAmount).HasColumnType("decimal(18,2)").IsRequired(false);

                // Relationships
                entity.HasOne(x => x.Unit).WithMany(x => x.Leases).HasForeignKey(x => x.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.Tenant).WithMany().HasForeignKey(x => x.TenantId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.BillingCycle).WithMany().HasForeignKey(x => x.BillingCycleId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.LeaseStatus).WithMany().HasForeignKey(x => x.LeaseStatusId).OnDelete(DeleteBehavior.Restrict);

                // Indexing
                entity.HasIndex(x => x.NextBillingDate);
                entity.HasIndex(x => x.UnitId).IsUnique();
                entity.HasIndex(x => x.LeaseStatusId);
            });



            // TRANSACTION
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.TransactionNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.Amount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(x => x.TransactionDate)
                    .IsRequired();

                entity.Property(x => x.Notes)
                    .HasMaxLength(100);

                entity.Property(x => x.ReferenceType)
                    .HasMaxLength(50);

                entity.Property(x => x.MonthFor);

                entity.Property(x => x.YearFor);

                // -------------------
                // Relationships
                // -------------------

                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Property)
                    .WithMany()
                    .HasForeignKey(x => x.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Unit)
                    .WithMany()
                    .HasForeignKey(x => x.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.PaymentMethod)
                    .WithMany()
                    .HasForeignKey(x => x.PaymentMethodId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(x => x.TransactionType)
                    .WithMany()
                    .HasForeignKey(x => x.TransactionTypeId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.TransactionCategory)
                    .WithMany()
                    .HasForeignKey(x => x.TransactionCategoryId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Account)
                    .WithMany()
                    .HasForeignKey(x => x.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                // -------------------
                // Indexes
                // -------------------

                entity.HasIndex(x => x.TransactionNumber)
                    .IsUnique();

                entity.HasIndex(x => x.TransactionDate);

                entity.HasIndex(x => new { x.PropertyId, x.MonthFor, x.YearFor });

                entity.HasIndex(x => x.ReferenceType);

                entity.HasIndex(x => x.ReferenceId);

                entity.HasIndex(x => new { x.ReferenceType, x.ReferenceId });
            });



            //TRANSACTIONALLOCATION
            modelBuilder.Entity<TransactionAllocation>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(u => u.Amount).HasColumnType("decimal(18,2)").IsRequired();

                // Relationships
                entity.HasOne(u => u.PaymentTransaction).WithMany(u => u.PaymentAllocations).HasForeignKey(u => u.PaymentTransactionId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.ChargeTransaction).WithMany(u => u.ChargeAllocations).HasForeignKey(u => u.ChargeTransactionId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });


            // TRANSACTIONRELATION
            modelBuilder.Entity<TransactionRelation>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.FromTransaction).WithMany(t => t.FromRelations).HasForeignKey(x => x.FromTransactionId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.ToTransaction).WithMany(t => t.ToRelations).HasForeignKey(x => x.ToTransactionId).OnDelete(DeleteBehavior.Restrict);

                // Relation Type
                entity.HasOne(x => x.RelationType).WithMany().HasForeignKey(x => x.RelationTypeId).OnDelete(DeleteBehavior.Restrict);
                entity.Property(x => x.Notes).HasMaxLength(500);
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



            modelBuilder.Entity<MaintenanceRequest>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Title)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(1000);


                entity.Property(x => x.RequestedAt)
                    .IsRequired();

                entity.Property(x => x.CompletedAt);

                entity.HasOne(x => x.Account)
                    .WithMany()
                    .HasForeignKey(x => x.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Status)
                    .WithMany()
                    .HasForeignKey(x => x.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Property)
                    .WithMany()
                    .HasForeignKey(x => x.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Unit)
                    .WithMany()
                    .HasForeignKey(x => x.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Tenant)
                    .WithMany()
                    .HasForeignKey(x => x.TenantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AssignedToUser)
                    .WithMany()
                    .HasForeignKey(x => x.AssignedToUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.FileName)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(x => x.FilePath)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(x => x.ContentType)
                    .HasMaxLength(100);

                entity.Property(x => x.FileSize)
                    .IsRequired();

                entity.HasOne(x => x.Account)
                    .WithMany()
                    .HasForeignKey(x => x.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Property)
                    .WithMany()
                    .HasForeignKey(x => x.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Unit)
                    .WithMany()
                    .HasForeignKey(x => x.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Tenant)
                    .WithMany()
                    .HasForeignKey(x => x.TenantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Lease)
                    .WithMany()
                    .HasForeignKey(x => x.LeaseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.PlanName)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.Amount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.StartDate)
                    .IsRequired();

                entity.Property(x => x.EndDate)
                    .IsRequired();

                entity.Property(x => x.IsTrial)
                    .HasDefaultValue(false);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.HasOne(x => x.Account)
                    .WithMany()
                    .HasForeignKey(x => x.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.GuestName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.MobileNumber)
                    .HasMaxLength(15)
                    .IsRequired();

                entity.Property(x => x.EmailAddress)
                    .HasMaxLength(100);

                entity.Property(x => x.NumberOfGuests)
                    .IsRequired();

                entity.Property(x => x.NightlyRate)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.AmountPaid)
                    .HasColumnType("decimal(18,2)");

                entity.Property(x => x.Notes)
                    .HasMaxLength(500);

                entity.HasOne(x => x.Account)
                    .WithMany()
                    .HasForeignKey(x => x.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Property)
                    .WithMany()
                    .HasForeignKey(x => x.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Unit)
                    .WithMany()
                    .HasForeignKey(x => x.UnitId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.BookingStatus)
                    .WithMany()
                    .HasForeignKey(x => x.BookingStatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.BookingSource)
                    .WithMany()
                    .HasForeignKey(x => x.BookingSourceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // UNIT
            modelBuilder.Entity<Unit>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Amount).IsRequired();
                entity.Property(u => u.Floor).HasDefaultValue(0).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);

                // relationships
                entity.HasOne(u => u.Status).WithMany().HasForeignKey(u => u.StatusId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.BillingCycle).WithMany().HasForeignKey(u => u.BillingCycleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.RentalType).WithMany().HasForeignKey(u => u.RentalTypeId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.UnitType).WithMany().HasForeignKey(u => u.UnitTypeId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Property).WithMany(u => u.Units).HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });


            // UNITFEATURE
            modelBuilder.Entity<UnitFeature>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
                entity.Property(x => x.Description).HasMaxLength(300);
                entity.Property(x => x.Icon).HasMaxLength(100);
                entity.Property(x => x.IsActive).HasDefaultValue(true);
                entity.Property(x => x.DataType).IsRequired();

                // Indexing
                entity.HasIndex(x => new {x.AccountId, x.Name}).IsUnique();

                // RelationShips
                entity.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.Restrict);
            });


            // UNITFEATUREASSIGNMENT
            modelBuilder.Entity<UnitFeatureAssignment>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Value).HasMaxLength(200);

                // Indexing
                entity.HasIndex(x => new {x.UnitId, x.UnitFeatureId}).IsUnique();

                // Relationships
                entity.HasOne(x => x.Unit).WithMany(x => x.Features).HasForeignKey(x => x.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.UnitFeature).WithMany(x => x.UnitAssignments).HasForeignKey(x => x.UnitFeatureId).OnDelete(DeleteBehavior.Restrict);
            });


            // UNITTYPE
            modelBuilder.Entity<UnitType>(entity => {
                entity.HasKey(u => u.Id);
                entity.HasOne(u => u.SystemCodeItem).WithMany().HasForeignKey(u => u.SystemCodeItemId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Account).WithMany().HasForeignKey(u => u.AccountId).OnDelete(DeleteBehavior.Cascade);
            });



            // UTILITYBILL
            modelBuilder.Entity<UtilityBill>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.IsMetered).HasDefaultValue(false);
                entity.Property(u => u.Amount).HasColumnType("decimal(18,4)").IsRequired();

                // indexing
                entity.HasIndex(x => new {x.PropertyId, x.UtilityId}).HasFilter("[UnitId] IS NULL").IsUnique();
                entity.HasIndex(x => new{x.UnitId, x.PropertyId, x.UtilityId}).HasFilter("[UnitId] IS NOT NULL").IsUnique();

                // Relationships
                entity.HasOne(u => u.BillingCycle).WithMany().HasForeignKey(u => u.BillingCycleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Utility).WithMany().HasForeignKey(u => u.UtilityId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Property).WithMany(u => u.Utilities).HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Unit).WithMany(u => u.UtilityBills).HasForeignKey(u => u.UnitId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Account).WithMany().HasForeignKey(u => u.AccountId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MeterReading>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.PreviousReading).HasColumnType("decimal(18,2)");
                entity.Property(x => x.CurrentReading).HasColumnType("decimal(18,2)");
                entity.Property(x => x.UnitsConsumed).HasColumnType("decimal(18,2)");
                entity.Property(x => x.Rate).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(x => x.ReadingDate).IsRequired();

                entity.HasOne(x => x.UtilityBill).WithMany(u => u.UtilityReadings).HasForeignKey(x => x.UtilityBillId).IsRequired().OnDelete(DeleteBehavior.Cascade);
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


            // AUDITTRAIL
            modelBuilder.Entity<AuditTrail>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.EntityName).IsRequired().HasMaxLength(100);
                entity.Property(x => x.EntityId).IsRequired();
                entity.Property(x => x.Action).IsRequired().HasMaxLength(20);
                entity.Property(x => x.OldValues).HasColumnType("nvarchar(max)");
                entity.Property(x => x.NewValues).HasColumnType("nvarchar(max)");

                entity.Property(x => x.CreatedAt).IsRequired();

                // Relationships
                entity.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId).OnDelete(DeleteBehavior.Cascade);

                // Indexing
                entity.HasIndex(x => x.EntityName);
                entity.HasIndex(x => x.EntityId);
                entity.HasIndex(x => x.CreatedAt);
                entity.HasIndex(x => new { x.EntityName, x.EntityId });
            });


        }

        //public override async Task<int> SaveChangesAsync(
        //    CancellationToken cancellationToken = default)
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
        //        else if (entry.State == EntityState.Modified)
        //        {
        //            entry.Entity.UpdatedOn = now;
        //            entry.Entity.UpdatedBy = userId > 0 ? userId : null;
        //        }
        //    }

        //    return await base.SaveChangesAsync(cancellationToken);
        //}

        private void ApplyGlobalFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                var parameter = Expression.Parameter(clrType, "e");
                Expression? combinedExpression = null;

                // Soft Delete Filter (if entity inherits AuditableEntity)
                if (typeof(AuditableEntity).IsAssignableFrom(clrType))
                {
                    var isDeletedProperty = Expression.Call(
                        typeof(EF),
                        nameof(EF.Property),
                        new[] { typeof(bool) },
                        parameter,
                        Expression.Constant("IsDeleted")
                    );

                    var isNotDeleted = Expression.Equal(
                        isDeletedProperty,
                        Expression.Constant(false)
                    );

                    combinedExpression = isNotDeleted;
                }


                // Account Filter (if entity has AccountId)
                var accountIdPropertyInfo = entityType.FindProperty("AccountId");

                if (accountIdPropertyInfo != null)
                {
                    var accountIdProperty = Expression.Call(
                        typeof(EF),
                        nameof(EF.Property),
                        new[] { typeof(int) },
                        parameter,
                        Expression.Constant("AccountId")
                    );


                    var contextAccountId = Expression.Property(
                        Expression.Constant(this),
                        nameof(CurrentAccountId)
                    );

                    var accountFilter = Expression.Equal(
                        accountIdProperty,
                        contextAccountId
                    );

                    combinedExpression = combinedExpression == null
                        ? accountFilter
                        : Expression.AndAlso(combinedExpression, accountFilter);
                }

                if (combinedExpression == null)
                    continue;

                var lambda = Expression.Lambda(combinedExpression, parameter);

                modelBuilder.Entity(clrType).HasQueryFilter(lambda);
            }
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

            modelBuilder.Entity<T>()
                .HasOne(typeof(User), "DeletedByUser")
                .WithMany()
                .HasForeignKey("DeletedBy")
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
