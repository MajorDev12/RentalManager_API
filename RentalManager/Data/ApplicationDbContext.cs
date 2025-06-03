using Microsoft.EntityFrameworkCore;
using RentalManager.Models;

namespace RentalManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<SystemCode> SystemCodes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SystemCodeItem> SystemCodeItems { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UnitType> UnitTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UtilityBill> UnitCharges { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Landlord> Landlords { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }


        // In your ApplicationDbContext.cs

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureAuditFields<Property>(modelBuilder);
            ConfigureAuditFields<User>(modelBuilder);
            ConfigureAuditFields<Payment>(modelBuilder);
            ConfigureAuditFields<Unit>(modelBuilder);
            ConfigureAuditFields<UnitType>(modelBuilder);
            ConfigureAuditFields<UtilityBill>(modelBuilder);
            ConfigureAuditFields<SystemCode>(modelBuilder);
            ConfigureAuditFields<SystemCodeItem>(modelBuilder);
            ConfigureAuditFields<SystemLog>(modelBuilder);
            ConfigureAuditFields<Role>(modelBuilder);



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
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.LastPasswordChange);
                entity.Property(u => u.NationalId).HasMaxLength(10);
                entity.Property(u => u.ProfilePhotoUrl);
                entity.Property(u => u.IsActive).HasDefaultValue(false);
                entity.HasOne(u => u.Gender).WithMany().HasForeignKey(u => u.GenderId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.UserStatus).WithMany().HasForeignKey(u => u.UserStatusId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
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
                entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
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



            // PAYMENT
            modelBuilder.Entity<Payment>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.AmountPaid).HasColumnType("decimal(18,4)").IsRequired();
                entity.Property(u => u.PaymentDate).IsRequired();
                entity.Property(u => u.TransactionCode).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.HasOne(u => u.Tenant).WithMany().HasForeignKey(u => u.TenantId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.UtilityBill).WithMany().HasForeignKey(u => u.UtilityBillId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.PaymentMethodItem).WithMany().HasForeignKey(u => u.PaymentMethod).OnDelete(DeleteBehavior.Restrict);
            });



            // UNIT
            modelBuilder.Entity<Unit>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Status).HasMaxLength(50);
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.HasOne(u => u.UnitType).WithMany().HasForeignKey(u => u.UnitTypeId).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.Property).WithMany(u => u.Units).HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });



            // UNITTYPE
            modelBuilder.Entity<UnitType>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Amount).HasColumnType("decimal(18,4)").IsRequired();
                entity.Property(u => u.Notes).HasMaxLength(100);
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });



            // UTILITYBILL
            modelBuilder.Entity<UtilityBill>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(50).IsRequired();
                entity.Property(u => u.Amount).HasColumnType("decimal(18,4)").IsRequired();
                entity.HasOne(u => u.Property).WithMany().HasForeignKey(u => u.PropertyId).OnDelete(DeleteBehavior.Restrict);
            });



            // ROLE
            modelBuilder.Entity<Role>(entity => {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).HasMaxLength(20).IsRequired();
                entity.Property(u => u.IsEnabled).HasDefaultValue(true);
                entity.HasOne(u => u.Property).WithMany(u => u.Roles).HasForeignKey(u => u.PropertyId).OnDelete(DeleteBehavior.Restrict);
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
