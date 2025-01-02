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

    Task ITulahackContext.SaveChangesAsync() => SaveChangesAsync(default);
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
        modelBuilder.Entity<PersonBase>()
            .HasDiscriminator(item => item.Role)
            .HasValue<PersonBase>(ContestRole.Visitor)
            .HasValue<Contestant>(ContestRole.Contestant)
            .HasValue<Expert>(ContestRole.Expert)
            .HasValue<Moderator>(ContestRole.Moderator)
            .HasValue<Superuser>(ContestRole.Superuser);
        modelBuilder.Entity<PersonBase>()
            .ToTable("Accounts")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Contestant>();
        modelBuilder.Entity<Expert>();
        modelBuilder.Entity<Moderator>();
        modelBuilder.Entity<Superuser>();

        // ugly hack to enable inherited model type mutation
        modelBuilder.Entity<PersonBase>()
            .Property<ContestRole>(nameof(PersonBase.Role))
            .Metadata
            .SetAfterSaveBehavior(PropertySaveBehavior.Save);

        modelBuilder.Entity<Team>()
            .ToTable("Teams")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ContestCase>()
            .ToTable("ContestCases")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Company>()
            .ToTable("Companies")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Skill>()
            .ToTable("Skills")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<StorageFile>()
            .ToTable("StorageFiles")
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
    }

    public async Task<T> AddNewRecord<T>(T newItem) where T: class
    {
        var dbSet = Set<T>();
        var result = dbSet.Add(newItem);
        await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<T> UpdateRecord<T>(T record) where T: class
    {
        var dbSet = Set<T>();
        var result = dbSet.Update(record);
        await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<T> RemoveRecord<T>(T record) where T : class
    {
        var dbSet = Set<T>();
        var result = dbSet.Remove(record);
        await SaveChangesAsync();
        return result.Entity;
    }

    public async Task<T> AddNewRecord<T>(T newItem, CancellationToken cancellationToken) where T: class
    {
        var dbSet = Set<T>();
        var result = dbSet.Add(newItem);
        await SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<T> UpdateRecord<T>(T record, CancellationToken cancellationToken) where T: class
    {
        var dbSet = Set<T>();
        var result = dbSet.Update(record);
        await SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public async Task<T> RemoveRecord<T>(T record, CancellationToken cancellationToken) where T : class
    {
        var dbSet = Set<T>();
        var result = dbSet.Remove(record);
        await SaveChangesAsync(cancellationToken);
        return result.Entity;
    }

    public void ClearChangeTracker() => this.ChangeTracker.Clear();
}