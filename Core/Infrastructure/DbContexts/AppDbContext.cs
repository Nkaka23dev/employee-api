using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheEmployeeAPI.Domain.Entities;

namespace TheEmployeeAPI.Infrastructure.DbContexts
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISystemClock _systemClock;

        public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IHttpContextAccessor httpContextAccessor,
        ISystemClock systemClock
        ) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _systemClock = systemClock; ;
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Benefit> Benefits { get; set; }
        public DbSet<EmployeeBenefit> EmployeeBenefits { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EmployeeBenefit>()
            .HasIndex(b => new { b.EmployeeId, b.BenefitId }).IsUnique();
            // Force table names to lowercase
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName()!.ToLower());
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName()!.ToLower());
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName()!.ToLower());
                }
                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName()!.ToLower());
                }
            }
        }
        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                    entry.Entity.CreatedOn = _systemClock?.UtcNow.UtcDateTime;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                    entry.Entity.LastModifiedOn = _systemClock?.UtcNow.UtcDateTime;
                }
            }
        }
    }
}
