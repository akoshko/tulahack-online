using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Tulahack.Model;

namespace Tulahack.API.Context;

public class TulahackContext : DbContext, ITulahackContext
{
    public DbSet<PersonBase> Accounts { get; set; }
    public DbSet<Contestant> Contestants { get; set; }
    public DbSet<Expert> Experts { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<ContestCase> ContestCases { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Moderator> Moderators { get; set; }
    public DbSet<ContestApplication> ContestApplications { get; set; }
    public DbSet<StorageFile> StorageFiles { get; set; }

    Task ITulahackContext.SaveChangesAsync() => SaveChangesAsync();
    Task ITulahackContext.SaveChangesAsync(CancellationToken cancellationToken) => SaveChangesAsync(cancellationToken);

    public TulahackContext(DbContextOptions<TulahackContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // how to create tables with model inheritance and entity type hierarchy mapping
        // https://learn.microsoft.com/en-us/ef/core/modeling/inheritance

        // Table-per-concrete-type configuration
        // Separate table for each entity: Accounts, Contestants, Experts and Moderators
        // modelBuilder.Entity<PersonBase>()
        //     .UseTpcMappingStrategy()
        //     .ToTable("Accounts")
        //     .Property(p => p.Id)
        //     .ValueGeneratedOnAdd();
        // modelBuilder.Entity<Contestant>()
        //     .ToTable("Contestants")
        //     .Property(p => p.Id)
        //     .ValueGeneratedOnAdd();
        // modelBuilder.Entity<Expert>()
        //     .ToTable("Experts")
        //     .Property(p => p.Id)
        //     .ValueGeneratedOnAdd();
        // modelBuilder.Entity<Moderator>()
        //     .ToTable("Moderators")
        //     .Property(p => p.Id)
        //     .ValueGeneratedOnAdd();
        // modelBuilder.Entity<Team>()
        //     .ToTable("Teams")
        //     .Property(p => p.Id)
        //     .ValueGeneratedOnAdd();

        // Table-per-hierarchy and discriminator configuration
        // Single table 'Accounts' for PersonBase, Contestant, Expert and Moderator entities
        _ = modelBuilder.Entity<PersonBase>()
            .HasDiscriminator(item => item.Role)
            .HasValue<PersonBase>(ContestRole.Visitor)
            .HasValue<Contestant>(ContestRole.Contestant)
            .HasValue<Expert>(ContestRole.Expert)
            .HasValue<Moderator>(ContestRole.Moderator)
            .HasValue<Superuser>(ContestRole.Superuser);
        _ = modelBuilder.Entity<PersonBase>()
            .ToTable("Accounts")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        _ = modelBuilder.Entity<Contestant>();
        _ = modelBuilder.Entity<Expert>();
        _ = modelBuilder.Entity<Moderator>();
        _ = modelBuilder.Entity<Superuser>();

        // ugly hack to enable inherited model type mutation
        modelBuilder.Entity<PersonBase>()
            .Property<ContestRole>(nameof(PersonBase.Role))
            .Metadata
            .SetAfterSaveBehavior(PropertySaveBehavior.Save);

        _ = modelBuilder.Entity<Team>()
            .ToTable("Teams")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

        _ = modelBuilder.Entity<ContestCase>()
            .ToTable("ContestCases")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        _ = modelBuilder.Entity<Company>()
            .ToTable("Companies")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        _ = modelBuilder.Entity<Skill>()
            .ToTable("Skills")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        _ = modelBuilder.Entity<StorageFile>()
            .ToTable("StorageFiles")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
    }

    public async Task<T> AddNewRecord<T>(T newItem) where T : class
    {
        DbSet<T> dbSet = Set<T>();
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> result = dbSet.Add(newItem);
        _ = await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<T> UpdateRecord<T>(T record) where T : class
    {
        DbSet<T> dbSet = Set<T>();
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> result = dbSet.Update(record);
        _ = await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<T> RemoveRecord<T>(T record) where T : class
    {
        DbSet<T> dbSet = Set<T>();
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> result = dbSet.Remove(record);
        _ = await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<T> AddNewRecord<T>(T newItem, CancellationToken cancellationToken) where T : class
    {
        DbSet<T> dbSet = Set<T>();
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> result = dbSet.Add(newItem);
        _ = await SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<T> UpdateRecord<T>(T record, CancellationToken cancellationToken) where T : class
    {
        DbSet<T> dbSet = Set<T>();
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> result = dbSet.Update(record);
        _ = await SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<T> RemoveRecord<T>(T record, CancellationToken cancellationToken) where T : class
    {
        DbSet<T> dbSet = Set<T>();
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> result = dbSet.Remove(record);
        _ = await SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public void ClearChangeTracker() => this.ChangeTracker.Clear();
}
