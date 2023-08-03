using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Identity
{
    public class AppIdentityContext : IdentityDbContext<User>
    {
        public AppIdentityContext(DbContextOptions<AppIdentityContext> opt) : base(opt) { }

        public DbSet<TaxPayer> TaxPayers { get; set; }

        public DbSet<TaxReturn> TaxReturns { get; set; }

        public DbSet<TaxHistory> TaxHistorys { get; set; }

        public DbSet<Admin> Admins { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
        }

    }
}
