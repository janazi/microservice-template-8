using MicroserviceTemplate.CrossCutting.Extensions;
using MicroserviceTemplate.Domain.Entities;
using MicroserviceTemplate.Infra.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceTemplate.Infra.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureBaseEntity(modelBuilder);
        ConfigureVehiclesTable(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new BaseEntityDateInterceptor());
    }

    private static void ConfigureBaseEntity(ModelBuilder modelBuilder)
    {
        // we can use a "dummy" generic argument here in order to access the property names
        const string uIdPropertyName = nameof(BaseEntity<int>.UId);
        const string dateCreatedPropertyName = nameof(BaseEntity<int>.DateCreated);
        const string dateModifiedPropertyName = nameof(BaseEntity<int>.DateModified);
        const string isDeletedPropertyName = nameof(BaseEntity<int>.IsDeleted);

        foreach (var entityType in modelBuilder.Model
                     .GetEntityTypes().Where(t => t.ClrType
                         .HasGenericParentType(typeof(BaseEntity<>))))
        {
            modelBuilder.Entity(entityType.ClrType)
                .HasIndex(uIdPropertyName)
                .IsUnique()
                .HasFilter($"[{isDeletedPropertyName}] = 0");

            modelBuilder.Entity(entityType.ClrType)
                .Property(uIdPropertyName)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            modelBuilder.Entity(entityType.ClrType)
                .HasIndex(dateCreatedPropertyName);

            modelBuilder.Entity(entityType.ClrType)
                .Property(dateModifiedPropertyName)
                .ValueGeneratedOnAddOrUpdate();
        }
    }

    public static void ConfigureVehiclesTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>().HasKey(v => v.Id);
    }
}