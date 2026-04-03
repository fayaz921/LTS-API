using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LTS.API.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Case> Cases => Set<Case>();
        public DbSet<Court> Courts => Set<Court>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Petitioner> Petitioners => Set<Petitioner>();
        public DbSet<CasePetitioner> CasePetitioners => Set<CasePetitioner>();
        public DbSet<Followup> Followups => Set<Followup>();
        public DbSet<Bench> Benches => Set<Bench>();
        public DbSet<CaseDocument> CaseDocuments => Set<CaseDocument>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // apply all configurations automatically
            // it will scan and apply every IEntityTypeConfiguration in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // store enums as strings in database — more readable than integers
            modelBuilder.HasPostgresEnum<CaseStatus>();
            modelBuilder.HasPostgresEnum<UserRole>();
        }


        //Override runs automatically before every save. It sets Id and CreatedAt for new records,
        //and UpdatedAt for updated records. You write this logic once and never think about
        //it again in any handler.
        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            // automatically set CreatedAt and UpdatedAt before saving
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Id = Guid.NewGuid();
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(ct);
        }
    }
}
